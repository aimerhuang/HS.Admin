using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Hyt.DataAccess.Report;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Util;
using Hyt.Model.SystemPredefined;
using System.Data;

namespace Hyt.DataAccess.Oracle.Report
{
    /// <summary>
    /// ReportDaoImpl
    /// </summary>
    /// <remarks>2013-9-16 黄伟 创建</remarks>
    public class ReportDaoImpl : IReportDao
    {
        #region 升舱明细

        /// <summary>
        /// 升舱明细查询
        /// </summary>
        /// <param name="para">CBReportDsorderDetail</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="totalAmount">输出，付款金额合计</param>
        /// <returns>Dic(totalCount,升舱明细集合)</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public override Dictionary<int, List<ReportDsorderDetail>> QueryUpgradeDetails(CBReportDsorderDetail para, ref decimal totalAmount,
                                                                                       int currPageIndex = 1,
                                                                                       int pageSize = 10)
        {
            string sqlSelect = @"订单编号,
                        第三方订单号,
                        升舱付款时间,
                        商城订单时间,
                        所属分支机构,
                        客户所在城市,
                        商城名称,
                        升舱来源店面,
                        物流类型,
                        产品名称,
                        付款金额,
                        发货时间,
                        未发货原因,
                        备注",
                   sqlFrom = @"vw_rp_升舱明细",
                   sqlCondition =
                       @"(@mallType is null or 商城类型=@mallType) 
                            and (@beginDate is null or 升舱时间 >=  @beginDate)
                            and (@endDate is null or 升舱时间 <=  @endDate )
                        ";
            using (var context = Context.UseSharedConnection(true))
            {
                var lstResult = context.Select<ReportDsorderDetail>(sqlSelect)
                                       .From(sqlFrom)
                                       .AndWhere(sqlCondition)
                                       .Parameter("mallType", para.MallType)
                                       .Parameter("beginDate", para.OrderBeginDate)
                                       .Parameter("endDate",
                                                  para.OrderEndDate != null
                                                      ? ((DateTime)para.OrderEndDate).AddDays(1)
                                                      : para.OrderEndDate)
                                       .Paging(currPageIndex, pageSize) //index从1开始
                                       .OrderBy("商城订单时间 desc")
                                       .QueryMany();
                var count = context.Select<int>(@"count(*)")
                                   .From(sqlFrom)
                                   .AndWhere(sqlCondition)
                                   .Parameter("mallType", para.MallType)
                                   .Parameter("beginDate", para.OrderBeginDate)
                                   .Parameter("endDate",
                                              para.OrderEndDate != null
                                                  ? ((DateTime)para.OrderEndDate).AddDays(1)
                                                  : para.OrderEndDate)
                                   .QuerySingle();

                //付款金额合计 2014-05-05 朱家宏添加
                totalAmount = context.Select<decimal>(@"sum(付款金额)")
                                   .From(sqlFrom)
                                   .AndWhere(sqlCondition)
                                   .Parameter("mallType", para.MallType)
                                   .Parameter("beginDate", para.OrderBeginDate)
                                   .Parameter("endDate",
                                              para.OrderEndDate != null
                                                  ? ((DateTime)para.OrderEndDate).AddDays(1)
                                                  : para.OrderEndDate)
                                   .QuerySingle();

                return new Dictionary<int, List<ReportDsorderDetail>> { { count, lstResult } };
            }
        }

        /// <summary>
        /// 升舱明细查询
        /// </summary>
        /// <returns>DsMallType集合</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public override List<DsMallType> GetMallType()
        {
            return Context.Sql("select * from dsmalltype")
                          .QueryMany<DsMallType>();
        }

        #endregion

        #region 销售明细

        /// <summary>
        /// 销售明细查询
        /// </summary>
        /// <param name="para">CBReportSaleDetail</param>
        /// <param name="warehouseSysNos">用户选中的仓库</param>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dic(totalCount,销售明细集合)</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public override Dictionary<int, List<RP_销售明细>> QuerySaleDetails(ref List<CBRptPaymentRecord> PaymentRecords, SalesRmaParams para, List<int> warehouseSysNos, int userSysNo,
                                                                                 int currPageIndex = 1,
                                                                                 int pageSize = 10)
        {

            var dicSql = GenSqlFromPara(para);

            string sqlSelect =
                @"下单日期,订单号,订单来源,下单门店,出库日期,会员名,ERP编码,产品名称,数量,单价,优惠,销售金额,实收金额,出库仓库,收款方式,配送方式,快递单号,
                 收货地址,收货电话,送货员,客服,结算状态,店铺名称,商城订单号,省,市,区,收货人,对内备注,出库单号,发货日期",
                   sqlFrom = @"RP_销售明细 t left join whwarehouse w on t.仓库编号 = w.sysno",
                   sqlCondition = "";
            #region sqlcondition 仓库包含逻辑
            //没有选择仓库

            //指定了仓库,没有传入仓库则
            string whereWarehouse = warehouseSysNos.Any() ? string.Format(" and t.仓库编号 in({0})", string.Join(",", warehouseSysNos)) : "and t.仓库编号 in(0)";

            sqlCondition = string.Format(
                @"
              ((@orderBeginDate is null or t.下单日期 >=  @orderBeginDate) and (@orderEndDate is null or t.下单日期 <=  @orderEndDate )) and  
              ((@beginDate is null or t.出库日期 >=  @beginDate) and (@endDate is null or t.出库日期 <=  @endDate )) 
                and   ((@FaHuobeginDate is null or t.发货日期 >=  @FaHuobeginDate) and (@FaHuoendDate is null or t.发货日期 <=  @FaHuoendDate ))
                "
              + dicSql["DelType"] + dicSql["PayType"] + dicSql["OrderSource"] + dicSql["SettlementType"]
              + @"
                and (@ProNo is null or @ProNo = '' or t.ERP编码 like '{1}%' )
                and (@CusId is null or @CusId = '' or t.会员名 =  @CusId )
                and (@OrderNo is null or @OrderNo = '' or t.订单号 =  @OrderNo )
                {0}", whereWarehouse, para.ProNo);


            #endregion

            //if (para.SettlementStatus != null && para.SettlementStatus == "待结算")
            //{
            //    para.SettlementStatus = "未结算";
            //}

            using (var context = Context.UseSharedConnection(true))
            {
                var lstResult = context.Select<RP_销售明细>(sqlSelect)
                                       .From(sqlFrom)
                                       .AndWhere(sqlCondition)
                                       .Parameter("orderBeginDate", para.OrderBeginDate)
                                       .Parameter("orderEndDate",
                                                  para.OrderEndDate != null
                                                      ? ((DateTime)para.OrderEndDate)
                                                      : para.OrderEndDate)
                                       .Parameter("beginDate", para.BeginDate)
                                       .Parameter("endDate",
                                                  para.EndDate != null
                                                      ? ((DateTime)para.EndDate)
                                                      : para.EndDate)
                                                      .Parameter("FaHuobeginDate", para.FaHuoBeginDate)
                                        .Parameter("FaHuoendDate",
                                                  para.FaHuoEndDate != null
                                                      ? ((DateTime)para.FaHuoEndDate)
                                                      : para.FaHuoEndDate)
                                       .Parameter("ProNo", para.ProNo)
                    //                   .Parameter("ProName", para.ProName)
                                       .Parameter("CusId", para.CusId)
                    //                   .Parameter("Phone", para.Phone)
                    //.Parameter("SettlementStatus",
                    //           para.SettlementStatus == "全部" ? null : para.SettlementStatus)
                    ////.Parameter("SettlementStatus",
                    ////           para.SettlementStatus == "全部" ? null : para.SettlementStatus)
                    ////.Parameter("InvStatus", para.InvStatus == "全部" ? null : para.InvStatus)
                    ////.Parameter("InvStatus", para.InvStatus == "全部" ? null : para.InvStatus)
                                       .Parameter("OrderNo", para.OrderNo)
                    //                    .Parameter("IsSelfSupport", para.IsSelfSupport)
                    ////.Parameter("OrderSource", para.OrderSource ==-1? null : para.OrderSource)
                    ////.Parameter("OrderSource", para.OrderSource == -1 ? null : para.OrderSource)

                                       .Paging(currPageIndex, pageSize) //index从1开始
                                       .OrderBy("t.订单号,t.ERP编码 desc")
                                       .QueryMany();
                var count = context.Select<int>(@"count(*)")
                                   .From(sqlFrom)
                                   .AndWhere(sqlCondition)
                                   .Parameter("orderBeginDate", para.OrderBeginDate)
                                   .Parameter("orderEndDate",
                                               para.OrderEndDate != null
                                                   ? ((DateTime)para.OrderEndDate)
                                                   : para.OrderEndDate)
                                   .Parameter("beginDate", para.BeginDate)
                                   .Parameter("endDate",
                                              para.EndDate != null ? ((DateTime)para.EndDate) : para.EndDate)
                                              .Parameter("FaHuobeginDate", para.FaHuoBeginDate)
                                        .Parameter("FaHuoendDate",
                                                  para.FaHuoEndDate != null
                                                      ? ((DateTime)para.FaHuoEndDate)
                                                      : para.FaHuoEndDate)
                                   .Parameter("ProNo", para.ProNo)
                    //               .Parameter("ProName", para.ProName)
                                   .Parameter("CusId", para.CusId)
                    //               .Parameter("Phone", para.Phone)
                    //.Parameter("SettlementStatus",
                    //           para.SettlementStatus == "全部" ? null : para.SettlementStatus)
                    ////.Parameter("SettlementStatus",
                    ////           para.SettlementStatus == "全部" ? null : para.SettlementStatus)
                    ////.Parameter("InvStatus", para.InvStatus == "全部" ? null : para.InvStatus)
                    ////.Parameter("InvStatus", para.InvStatus == "全部" ? null : para.InvStatus)
                                   .Parameter("OrderNo", para.OrderNo)
                    //               .Parameter("IsSelfSupport", para.IsSelfSupport)
                    ////.Parameter("OrderSource", para.OrderSource == -1 ? null : para.OrderSource)
                    ////.Parameter("OrderSource", para.OrderSource == -1 ? null : para.OrderSource)
                                   .QuerySingle();
                //不分页 做统计
                var lstResultAll = context.Select<RP_销售明细>("数量,优惠,销售金额,实收金额")
                                       .From(sqlFrom)
                                       .AndWhere(sqlCondition)
                                       .Parameter("orderBeginDate", para.OrderBeginDate)
                                       .Parameter("orderEndDate",
                                               para.OrderEndDate != null
                                                   ? ((DateTime)para.OrderEndDate)
                                                   : para.OrderEndDate)
                                       .Parameter("beginDate", para.BeginDate)
                                       .Parameter("endDate",
                                                  para.EndDate != null
                                                      ? ((DateTime)para.EndDate)
                                                      : para.EndDate)
                                                      .Parameter("FaHuobeginDate", para.FaHuoBeginDate)
                                        .Parameter("FaHuoendDate",
                                                  para.FaHuoEndDate != null
                                                      ? ((DateTime)para.FaHuoEndDate)
                                                      : para.FaHuoEndDate)
                                       .Parameter("ProNo", para.ProNo)
                    //                   .Parameter("ProName", para.ProName)
                                       .Parameter("CusId", para.CusId)
                    //                   .Parameter("Phone", para.Phone)
                    //.Parameter("SettlementStatus",
                    //           para.SettlementStatus == "全部" ? null : para.SettlementStatus)
                    ////.Parameter("SettlementStatus",
                    ////           para.SettlementStatus == "全部" ? null : para.SettlementStatus)
                    ////.Parameter("InvStatus", para.InvStatus == "全部" ? null : para.InvStatus)
                    ////.Parameter("InvStatus", para.InvStatus == "全部" ? null : para.InvStatus)
                                       .Parameter("OrderNo", para.OrderNo)
                    //                   .Parameter("IsSelfSupport", para.IsSelfSupport)
                    ////.Parameter("OrderSource", para.OrderSource == -1 ? null : para.OrderSource)
                    ////.Parameter("OrderSource", para.OrderSource == -1 ? null : para.OrderSource)
                                       .QueryMany();
                //不分页 做支付方式统计
                //20171010 罗勤瑶添加
                var lstAll = context.Select<CBRptPaymentRecord>("销售金额 as ALLAmount,实收金额 as Amount,收款方式 as PaymentName")
                                       .From(sqlFrom)
                                       .AndWhere(sqlCondition)
                                       .Parameter("orderBeginDate", para.OrderBeginDate)
                                       .Parameter("orderEndDate",
                                               para.OrderEndDate != null
                                                   ? ((DateTime)para.OrderEndDate)
                                                   : para.OrderEndDate)
                                       .Parameter("beginDate", para.BeginDate)
                                       .Parameter("endDate",
                                                  para.EndDate != null
                                                      ? ((DateTime)para.EndDate)
                                                      : para.EndDate)
                                                      .Parameter("FaHuobeginDate", para.FaHuoBeginDate)
                                        .Parameter("FaHuoendDate",
                                                  para.FaHuoEndDate != null
                                                      ? ((DateTime)para.FaHuoEndDate)
                                                      : para.FaHuoEndDate)
                                       .Parameter("ProNo", para.ProNo)

                                       .Parameter("CusId", para.CusId)

                                       .Parameter("OrderNo", para.OrderNo)

                                       .QueryMany();
                PaymentRecords = lstAll;
                return new Dictionary<int, List<RP_销售明细>> { { count, lstResult }, { int.MaxValue, lstResultAll } };
            }
        }

        /// <summary>
        /// 获取UI用户多选的checkboxs的sql
        /// </summary>
        /// <param name="para"></param>
        /// <returns>Dictionary: 栏位名,sql</returns>
        /// <remarks>2013-9-23 黄伟 创建</remarks>
        private Dictionary<string, string> GenSqlFromPara(SalesRmaParams para)
        {
            var dic = new Dictionary<string, string>();
            var temp = new List<string>();

            if (para.DelType != null && para.DelType.Any())
            {
                para.DelType.ForEach(p => temp.Add("t.配送编号='" + p + "'"));
                dic.Add("DelType", " and (" + string.Join(" or ", temp) + ")");
            }
            else
                dic.Add("DelType", string.Empty);

            temp.Clear();
            if (para.PickType != null && para.PickType.Any())
            {
                para.PickType.ForEach(p => temp.Add("t.售后方式编号='" + p + "'"));
                dic.Add("PickType", " and (" + string.Join(" or ", temp) + ")");
            }
            else
                dic.Add("PickType", string.Empty);

            temp.Clear();
            if (para.PayType != null && para.PayType.Any())
            {
                para.PayType.ForEach(p => temp.Add("t.收款方式编号='" + p + "'"));
                dic.Add("PayType", " and (" + string.Join(" or ", temp) + ")");
            }
            else
                dic.Add("PayType", string.Empty);

            temp.Clear();
            if (para.RMAType != null && para.RMAType.Any())
            {
                para.RMAType.ForEach(p => temp.Add("t.退款方式='" + p + "'"));
                dic.Add("RMAType", " and (" + string.Join(" or ", temp) + ")");
            }
            else
                dic.Add("RMAType", string.Empty);

            temp.Clear();
            if (para.OrderSource != null && para.OrderSource.Any())
            {
                para.OrderSource.ForEach(p => temp.Add("t.订单来源='" + p + "'"));
                dic.Add("OrderSource", " and (" + string.Join(" or ", temp) + ")");
            }
            else
                dic.Add("OrderSource", string.Empty);
            temp.Clear();
            if (para.SettlementType != null && para.SettlementType.Any())
            {
                para.SettlementType.ForEach(p => temp.Add("t.结算状态='" + p.Replace("待结算", "未结算") + "'"));
                dic.Add("SettlementType", " and (" + string.Join(" or ", temp) + ")");
            }
            else
                dic.Add("SettlementType", string.Empty);

            return dic;
        }

        #endregion

        #region 退换货明细

        /// <summary>
        /// 退换货明细查询
        /// </summary>
        /// <param name="para">CBReportSaleDetail</param>
        /// <param name="warehouseSysNos">用户选中的仓库</param>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dic(totalCount,销售明细集合)</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public override Dictionary<int, List<RP_退换货明细>> QueryRmaDetails(SalesRmaParams para, List<int> warehouseSysNos, int userSysNo,
                                                                                 int currPageIndex = 1,
                                                                                 int pageSize = 10)
        {

            var dicSql = GenSqlFromPara(para);

            string sqlSelect =
                @"订单号,订单来源,入库日期,申请日期,会员名,ERP编码,产品名称,数量,单价,优惠,退款金额,实退金额,入库仓库,收款方式,退款方式,配送方式,售后方式,收货电话,结算状态,入库单号,t.下单门店",
                   sqlFrom = @"RP_退换货明细 t left join whwarehouse w on t.仓库编号 = w.sysno",
                   sqlCondition = "";

            //指定了仓库,没有传入仓库则
            string whereWarehouse = warehouseSysNos.Any() ? string.Format(" and t.仓库编号 in({0})", string.Join(",", warehouseSysNos)) : "and t.仓库编号 in(0)";

            sqlCondition = string.Format(
                @"      (@beginDate is null or t.申请日期 >=  @beginDate)
                            and (@endDate is null or t.申请日期 <=  @endDate ) and  (@RbeginDate is null or t.入库日期 >=  @RbeginDate)
                            and (@RendDate is null or t.入库日期 <=  @RendDate )
                            "
                + dicSql["DelType"] + dicSql["PickType"] + dicSql["PayType"] + dicSql["RMAType"] + dicSql["OrderSource"] + dicSql["SettlementType"]
                + @" and (@ProNo is null or t.ERP编码 like  @ProNo )
                                and (@ProName is null or t.产品名称 like  @ProName )
                                and (@CusId is null or t.会员名 =  @CusId )
                                and (@Phone is null or t.收货电话 =  @Phone )
                                --and (@SettlementStatus is null or t.结算状态 =@SettlementStatus )
                                --and (@InvStatus is null or t.开票状态 =  @InvStatus )
                                and (@OrderNo is null or t.订单号 =  @OrderNo )
                                --and (@OrderSource is null or t.订单来源 =  @OrderSource )
                                 and (@IsSelfSupport is null or w.IsSelfSupport=@IsSelfSupport)
                                {0}
                            ", whereWarehouse);

            if (para.SettlementStatus == "-1")
            {
                para.SettlementStatus = null;
            }

            using (var context = Context.UseSharedConnection(true))
            {
                List<RP_退换货明细> lstResult = context.Select<RP_退换货明细>(sqlSelect)
                                       .From(sqlFrom)
                                       .AndWhere(sqlCondition)
                                       .Parameter("beginDate", para.BeginDate)
                                       .Parameter("endDate",
                                                  para.EndDate != null
                                                      ? ((DateTime)para.EndDate).AddDays(1)
                                                      : para.EndDate)
                                                      .Parameter("ProNo", string.IsNullOrEmpty(para.ProNo) ? null : "%"+ para.ProNo.Trim()+"%")
                                       .Parameter("ProName", string.IsNullOrEmpty(para.ProName) ? null : "%" + para.ProName.Trim() + "%")
                                       .Parameter("CusId", string.IsNullOrEmpty(para.CusId) ? null : para.CusId)
                                       .Parameter("Phone", string.IsNullOrEmpty(para.Phone) ? null : para.Phone)
                                       .Parameter("RbeginDate", para.RBeginDate)
                                       .Parameter("RendDate", para.REndDate)
                    //.Parameter("SettlementStatus",
                    //          para.SettlementStatus)
                    //.Parameter("SettlementStatus",
                    //para.SettlementStatus)
                    //.Parameter("InvStatus", para.InvStatus == "全部" ? null : para.InvStatus)
                    //.Parameter("InvStatus", para.InvStatus == "全部" ? null : para.InvStatus)
                    .Parameter("OrderNo", string.IsNullOrEmpty(para.OrderNo) ? null : para.OrderNo)
                                        .Parameter("IsSelfSupport", para.IsSelfSupport)
                    //.Parameter("OrderSource", para.OrderSource ==-1? null : para.OrderSource)
                    //.Parameter("OrderSource", para.OrderSource == -1 ? null : para.OrderSource)

                                       .Paging(currPageIndex, pageSize) //index从1开始
                                       .OrderBy("t.订单号,t.ERP编码 desc")
                                       .QueryMany();
                var count = context.Select<int>(@"count(*)")
                                   .From(sqlFrom)
                                   .AndWhere(sqlCondition)
                                   .Parameter("beginDate", para.BeginDate)
                                   .Parameter("endDate",
                                              para.EndDate != null ? ((DateTime)para.EndDate).AddDays(1) : para.EndDate)
                                              .Parameter("ProNo", string.IsNullOrEmpty(para.ProNo) ? null : "%" + para.ProNo+"%")
                                              .Parameter("ProName", string.IsNullOrEmpty(para.ProName) ? null : "%" + para.ProName.Trim() + "%")
                                              .Parameter("CusId", string.IsNullOrEmpty(para.CusId) ? null : para.CusId)
                                              .Parameter("Phone", string.IsNullOrEmpty(para.Phone) ? null : para.Phone)
                                               .Parameter("RbeginDate", para.RBeginDate)
                                            .Parameter("RendDate", para.REndDate)
                    //.Parameter("SettlementStatus",
                    //para.SettlementStatus)
                    //.Parameter("SettlementStatus",
                    //         para.SettlementStatus)
                    //.Parameter("InvStatus", para.InvStatus == "全部" ? null : para.InvStatus)
                    //.Parameter("InvStatus", para.InvStatus == "全部" ? null : para.InvStatus)
                    .Parameter("OrderNo", string.IsNullOrEmpty(para.OrderNo) ? null : para.OrderNo)
                                    .Parameter("IsSelfSupport", para.IsSelfSupport)
                    //.Parameter("OrderSource", para.OrderSource == -1 ? null : para.OrderSource)
                    //.Parameter("OrderSource", para.OrderSource == -1 ? null : para.OrderSource)
                                   .QuerySingle();
                //不分页 做统计
                var lstResultAll = context.Select<RP_退换货明细>("数量,优惠,退款金额,实退金额")
                                       .From(sqlFrom)
                                       .AndWhere(sqlCondition)
                                       .Parameter("beginDate", para.BeginDate)
                                       .Parameter("endDate",
                                                  para.EndDate != null
                                                      ? ((DateTime)para.EndDate).AddDays(1)
                                                      : para.EndDate)
                                                      .Parameter("ProNo", string.IsNullOrEmpty(para.ProNo) ? null : "%" + para.ProNo + "%")
                                                      .Parameter("ProName", string.IsNullOrEmpty(para.ProName) ? null : "%" + para.ProName.Trim() + "%")
                                                      .Parameter("CusId", string.IsNullOrEmpty(para.CusId) ? null : para.CusId)
                                                      .Parameter("Phone", string.IsNullOrEmpty(para.Phone) ? null : para.Phone)
                                                       .Parameter("RbeginDate", para.RBeginDate)
                                       .Parameter("RendDate", para.REndDate)
                    //.Parameter("SettlementStatus",
                    //           para.SettlementStatus)
                    //.Parameter("SettlementStatus",
                    //          para.SettlementStatus)
                    //.Parameter("InvStatus", para.InvStatus == "全部" ? null : para.InvStatus)
                    //.Parameter("InvStatus", para.InvStatus == "全部" ? null : para.InvStatus)
                    .Parameter("OrderNo", string.IsNullOrEmpty(para.OrderNo) ? null : para.OrderNo)
                                        .Parameter("IsSelfSupport", para.IsSelfSupport)
                    //.Parameter("OrderSource", para.OrderSource == -1 ? null : para.OrderSource)
                    //.Parameter("OrderSource", para.OrderSource == -1 ? null : para.OrderSource)
                                       .QueryMany();
                return new Dictionary<int, List<RP_退换货明细>> { { count, lstResult }, { int.MaxValue, lstResultAll } };
            }
        }

        /// <summary>
        /// 获取UI用户多选的checkboxs的sql-退换货明细
        /// </summary>
        /// <param name="para"></param>
        /// <returns>Dictionary: 栏位名,sql</returns>
        /// <remarks>2013-9-23 黄伟 创建</remarks>
        private Dictionary<string, string> GenRmaSqlFromPara(SalesRmaParams para)
        {
            var dic = new Dictionary<string, string>();
            var temp = new List<string>();

            if (para.DelType != null && para.DelType.Any())
            {
                para.DelType.ForEach(p => temp.Add("t.配送编号='" + p + "'"));
                dic.Add("DelType", " and (" + string.Join(" or ", temp) + ")");
                temp.RemoveAt(0);
            }
            else
                dic.Add("DelType", string.Empty);

            if (para.PickType != null && para.PickType.Any())
            {
                para.PickType.ForEach(p => temp.Add("t.售后方式编号='" + p + "'"));
                dic.Add("PickType", " and (" + string.Join(" or ", temp) + ")");
                temp.RemoveAt(0);
            }
            else
                dic.Add("PickType", string.Empty);

            if (para.PayType != null && para.PayType.Any())
            {
                para.PayType.ForEach(p => temp.Add("t.收款方式编号='" + p + "'"));
                dic.Add("PayType", " and (" + string.Join(" or ", temp) + ")");
                temp.RemoveAt(0);
            }
            else
                dic.Add("PayType", string.Empty);

            if (para.RMAType != null && para.RMAType.Any())
            {
                para.RMAType.ForEach(p => temp.Add("t.退款方式编号='" + p + "'"));
                dic.Add("RMAType", " and (" + string.Join(" or ", temp) + ")");
                temp.RemoveAt(0);
            }
            else
                dic.Add("RMAType", string.Empty);

            if (para.OrderSource != null && para.OrderSource.Any())
            {
                para.OrderSource.ForEach(p => temp.Add("t.订单来源='" + p + "'"));
                dic.Add("OrderSource", " and (" + string.Join(" or ", temp) + ")");
                temp.RemoveAt(0);
            }
            else
                dic.Add("OrderSource", string.Empty);

            return dic;
        }

        #endregion

        #region 市场部赠送明细

        /// <summary>
        /// 市场部赠送明细查询
        /// </summary>
        /// <param name="para">CBReportSaleDetail</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dic(totalCount,市场部赠送明细集合)</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        public override Dictionary<int, List<ReportMarketDepartmentSale>> QueryMarketPresentDetails(
            CBReportMarketDepartmentSale para,
            int currPageIndex = 1,
            int pageSize = 10)
        {
            string sqlSelect = @"m.收件人,m.地址,m.联系电话,m.订单号,m.产品名称,m.数量,m.发货时间,m.物流类型",
                   sqlFrom = @"vw_rp_市场部赠品明细 m",
                   sqlCondition =
                       @"(@beginDate is null or m.发货时间 >=  @beginDate)
                           and (@endDate is null or m.发货时间 <=  @endDate )
                        ";
            using (var context = Context.UseSharedConnection(true))
            {
                var lstResult = context.Select<ReportMarketDepartmentSale>(sqlSelect)
                                       .From(sqlFrom)
                                       .AndWhere(sqlCondition)
                                       .Parameter("beginDate", para.BeginDate)
                                       .Parameter("endDate",
                                                  para.EndDate != null
                                                      ? ((DateTime)para.EndDate).AddDays(1)
                                                      : para.EndDate)
                                       .Paging(currPageIndex, pageSize) //index从1开始
                                       .OrderBy("m.订单号 desc")
                                       .QueryMany();
                var count = context.Select<int>(@"count(*)")
                                   .From(sqlFrom)
                                   .AndWhere(sqlCondition)
                                   .Parameter("beginDate", para.BeginDate)
                                   .Parameter("endDate",
                                              para.EndDate != null ? ((DateTime)para.EndDate).AddDays(1) : para.EndDate)
                                   .QuerySingle();
                return new Dictionary<int, List<ReportMarketDepartmentSale>> { { count, lstResult } };
            }
        }
        #endregion

        #region 业绩报表

        /// <summary>
        /// 销售排行
        /// </summary>
        /// <param name="filter">filter</param>
        /// <returns>list</returns>
        /// <remarks>2013-10-22 朱家宏 创建</remarks>
        public override IList<RptSalesRanking> SelectSalesRanking(ParaRptSalesRankingFilter filter)
        {
            //            var sql = @"(select row_number() over (order by rpt.SalesQuantity desc) rowNumber,rpt.* from (
            //                        select a.商品分类 as productCategoryName,a.商品编号 as ProductSysNo,a.商品名称 as ProductName,
            //                        sum(a.销售数量) as SalesQuantity,sum(a.销售金额) as SalesAmount 
            //                        from rp_销售排行 a where {0}
            //                        group by a.商品分类,a.商品编号,a.商品名称
            //                    ) rpt ) tb";

            //            var paras = new ArrayList();
            //            var where = "1=1 ";
            //            int i = 0;
            //            if (filter.BeginDate != null)
            //            {
            //                where += " and a.统计日期>=@p0p" + i;
            //                paras.Add(filter.BeginDate);
            //                i++;
            //            }
            //            if (filter.EndDate != null)
            //            {
            //                where += " and a.统计日期<@p0p" + i;
            //                paras.Add(filter.EndDate);
            //                i++;
            //            }
            //            if (filter.ProductCategories != null)
            //            {
            //                var categories = string.Join(",", filter.ProductCategories);
            //                where +=
            //                    " and  exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col=a.商品分类编号)";
            //                paras.Add(categories);
            //                i++;
            //            }

            //            sql = string.Format(sql, where);
            //            if (filter.TakingCount > 0)
            //            {
            //                sql += " where rowNumber<=@p0p" + i;
            //                paras.Add(filter.TakingCount);
            //                i++;
            //            }

            //            var dataList = Context.Select<RptSalesRanking>("tb.*").From(sql).Parameters(paras).QueryMany();

            //            return dataList;
            var sql = @" select ROW_NUMBER() over(order by SUM(SoOrderItem.SalesAmount) DESC)  as RowNumber,
                        SUM(SoOrderItem.Quantity) as SalesQuantity,sum(SoOrderItem.SalesAmount) as SalesAmount, PdProduct.SysNo as ProductSysNo,PdProduct.Barcode,
                        PdProduct.ErpCode,PdProduct.EasName as ProductName,ProductCategoryName=(stuff
                                            ((SELECT   ''+ PdCategory.CategoryName
                                              FROM       PdCategoryAssociation inner join PdCategory on PdCategory.SysNo=PdCategoryAssociation.CategorySysNo
                                              WHERE   PdCategoryAssociation.ProductSysNo = PdProduct.SysNo and  PdCategoryAssociation.IsMaster=1  FOR xml path('')), 1, 0, ''))
                        ,ProductCategorySysNos=(stuff
                                            ((SELECT   ''+ PdCategory.SysNos
                                              FROM       PdCategoryAssociation inner join PdCategory on PdCategory.SysNo=PdCategoryAssociation.CategorySysNo
                                              WHERE   PdCategoryAssociation.ProductSysNo = PdProduct.SysNo and  PdCategoryAssociation.IsMaster=1 FOR xml path('')), 1, 0, ''))
                        from 
                        SoOrder inner join SoOrderItem on SoOrderItem.OrderSysNo=SoOrder.SysNo inner join PdProduct on SoOrderItem.ProductSysNo=PdProduct.SysNo ";

            var groupBy = @" group by PdProduct.ErpCode,PdProduct.EasName,PdProduct.Barcode, PdProduct.SysNo ";
            var paras = new ArrayList();
            var where = " SoOrder.Status>=30 and SoOrder.PayStatus=20   ";
            int i = 0;
            if (filter.BeginDate != null)
            {
                where += " and SoOrder.CreateDate>=@p0p" + i;
                paras.Add(filter.BeginDate);
                i++;
            }
            if (filter.EndDate != null)
            {
                where += " and SoOrder.CreateDate<@p0p" + i;
                paras.Add(filter.EndDate);
                i++;
            }
            if (filter.ProductCategories != null)
            {
                var categories = string.Join(",", filter.ProductCategories);
                var getproductSql = " select * from PdCategoryAssociation where CategorySysNo in (" + categories + ") ";
                List<PdCategoryAssociation> list = Context.Sql(getproductSql).QueryMany<PdCategoryAssociation>();
                string tempWhere = "";
                foreach (PdCategoryAssociation mod in list)
                {
                    if (tempWhere != "")
                    {
                        tempWhere += " , ";
                    }
                    tempWhere += mod.ProductSysNo;
                }
                if (string.IsNullOrEmpty(tempWhere))
                {
                    tempWhere = "-1";
                }
                where += " and  PdProduct.SysNo in (" + tempWhere + ") ";
                //where +=
                //    " and  exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col=a.商品分类编号)";
                //paras.Add(categories);
                i++;
            }

            if (where != "")
            {
                sql += " where " + where;
            }
            sql += groupBy + " order by SUM(SoOrderItem.SalesAmount) Desc ";
            //sql = string.Format(sql, where);
            //if (filter.TakingCount > 0)
            //{
            //    sql += " where rowNumber<=@p0p" + i;
            //    paras.Add(filter.TakingCount);
            //    i++;
            //}

            var dataList = Context.Sql(sql, null).Parameters(paras).QueryMany<RptSalesRanking>();

            return dataList;
        }

        /// <summary>
        /// 运营综述
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>list</returns>
        /// <remarks>2013-10-29 余勇 创建</remarks>
        public override IList<CBReportBusinessSummary> QueryBusinessSummary(ParaRptBusinessSummaryFilter filter)
        {
//            const string sql =
//                 @"(select 流量,访客,下单数,销售额,退款总额,净销售额,客单价,转换率,日期 from rp_运营综述
//                   ) tb";
            string sql = @"( select 
		convert(datetime,CONVERT(varchar(100), CreateDate, 111)) as 日期,
		 count(SysNo) as 下单数,sum(soorder.OrderAmount) as 销售额,
		( select count(tb.CustomerSysNo) from (select distinct sd.CustomerSysNo  from soorder sd where convert(datetime,CONVERT(varchar(100), sd.CreateDate, 111))=convert(datetime,CONVERT(varchar(100), soorder.CreateDate, 111)) ) tb )  as 客单价
		,
		(select sum(RcReturn.RefundTotalAmount) from RcReturn where RmaType=20 and Status=50 and convert(datetime,CONVERT(varchar(100), RefundDate, 111))=convert(datetime,CONVERT(varchar(100), soorder.CreateDate, 111)) group by  convert(datetime,CONVERT(varchar(100), RefundDate, 111)) ) as 退款总额
		from soorder
        group by convert(datetime,CONVERT(varchar(100), CreateDate, 111))  ) tb ";
            const string sqlWhere =
                @"(@0 is null or 日期 >= @0) and  --开始日期
                            (@1 is null or  日期 < @1)";
            string sqlSort = filter.Sort > 0 ? "日期" : "日期 desc";

            var paras = new object[]
                {
                    filter.BeginDate,
                    filter.EndDate
                };

            var dataList = Context.Select<CBReportBusinessSummary>("tb.*").From(sql)
                .Where(sqlWhere);
            dataList.Parameters(paras);
            IList<CBReportBusinessSummary> summaryList = dataList.OrderBy(sqlSort).QueryMany();
            foreach (var mod in summaryList)
            {
                mod.访客 = Convert.ToInt64(mod.客单价);
                mod.客单价 = Convert.ToDecimal((mod.销售额 / mod.客单价).ToString("0.00"));
                mod.净销售额 = mod.销售额 - mod.退款总额;
            }
            return summaryList;
        }

        /// <summary>
        /// 运营综述月报
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>list</returns>
        /// <remarks>2014-04-23 朱家宏 创建</remarks>
        public override IList<CBReportBusinessSummary> QueryBusinessSummaryMonthly(ParaRptBusinessSummaryFilter filter)
        {
            var sql =
                    @"(select sum(下单数) 下单数,sum(销售额) 销售额,sum(退款总额) 退款总额,sum(客单价) as 客单价,CONVERT(varchar(7),日期,120) as 日期 from 
                           ( select 
		convert(datetime,CONVERT(varchar(100), CreateDate, 111)) as 日期,
		 count(SysNo) as 下单数,sum(soorder.OrderAmount) as 销售额,
		( select count(tb.CustomerSysNo) from (select distinct sd.CustomerSysNo  from soorder sd where convert(datetime,CONVERT(varchar(100), sd.CreateDate, 111))=convert(datetime,CONVERT(varchar(100), soorder.CreateDate, 111)) ) tb )  as 客单价
		,
		(select sum(RcReturn.RefundTotalAmount) from RcReturn where RmaType=20 and Status=50 and convert(datetime,CONVERT(varchar(100), RefundDate, 111))=convert(datetime,CONVERT(varchar(100), soorder.CreateDate, 111)) group by  convert(datetime,CONVERT(varchar(100), RefundDate, 111)) ) as 退款总额
		from soorder
        group by convert(datetime,CONVERT(varchar(100), CreateDate, 111))  ) tb1
                            where {0} 
                            group by CONVERT(varchar(7),日期,120)
                       ) tb";

            var where = "1=1 ";

            var paras = new ArrayList();
            int i = 0;
            if (!string.IsNullOrWhiteSpace(filter.StartMonth))
            {
                where += " and CONVERT(varchar(7),日期,120)>=@p0p" + i;
                paras.Add(filter.StartMonth);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.EndMonth))
            {
                where += " and CONVERT(varchar(7),日期,120)<=@p0p" + i;
                paras.Add(filter.EndMonth);
                i++;
            }

            sql = string.Format(sql, where);

            string sqlSort = filter.Sort > 0 ? "日期" : "日期 desc";

            var dataList = Context.Select<CBReportBusinessSummary>("tb.*").From(sql);
            dataList.Parameters(paras);

            IList<CBReportBusinessSummary> summaryList = dataList.OrderBy(sqlSort).QueryMany();
            foreach (var mod in summaryList)
            {
                mod.访客 = Convert.ToInt64(mod.客单价);
                mod.客单价 = Convert.ToDecimal((mod.销售额 / mod.客单价).ToString("0.00"));
                mod.净销售额 = mod.销售额 - mod.退款总额;
            }
            return summaryList;
            //return dataList.OrderBy(sqlSort).QueryMany();
        }

        /// <summary>
        /// 门店会员消费报表查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>list</returns>
        /// <remarks>2014-01-06 余勇 创建</remarks>
        public override Pager<CBReportShopCustomerConsume> QueryShopCustomerConsume(ParaRptShopCustomerConsumeFilter filter)
        {
            const string sql =
                @"(select * from RP_绩效_门店会员消费
                        where (@0 is null or 日期 = @0)
                   ) tb";

            var paras = new object[]
                {
                    Convert.ToDateTime(filter.Reptdt)
                };
            var dataList = Context.Select<CBReportShopCustomerConsume>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBReportShopCustomerConsume>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id
            };

            pager.TotalRows = dataCount.QuerySingle();
            var list = dataList.OrderBy("tb.门店名称").Paging(pager.CurrentPage, filter.PageSize).QueryMany();
            pager.Rows = list;

            return pager;
        }

        /// <summary>
        /// 电商中心绩效分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>list</returns>
        /// <remarks>
        /// 2013-12-10 黄志勇 添加
        /// 2013-12-19 朱家宏 修改
        /// </remarks>
        public override Pager<RP_绩效_电商中心> GetListEBusinessCenter(ParaEBusinessCenterPerformanceFilter filter)
        {
            //            const string sql =
            //                @"(select a.分销商商城编号,a.分销商,sum(a.升舱金额_百城达) 升舱金额_百城达,sum(a.升舱金额_第三方) 升舱金额_第三方,sum(a.升舱单量_百城达) 升舱单量_百城达,sum(a.升舱单量_第三方) 升舱单量_第三方 
            //from RP_绩效_电商中心 a
            //where   
            //        (:DateStart is null or a.统计日期 >= :DateStart) and  --开始日期
            //        (:DateEnd is null or  a.统计日期 <= :DateEnd) and     --结束日期
            //        (:DealerMallSysNo is null or a.分销商商城编号 = :DealerMallSysNo)      --分销商商城编号     
            //group by a.分销商商城编号,a.分销商
            //        ) tb";

            //            var paras = new object[]
            //                {
            //                    filter.DateStart,       filter.DateStart,
            //                    filter.DateEnd,         filter.DateEnd.HasValue ? filter.DateEnd.Value.AddDays(1) : filter.DateEnd,
            //                    filter.DealerMallSysNo, filter.DealerMallSysNo
            //                };
            const string sql = @"(select a.分销商商城编号,a.分销商,a.升舱金额_百城达,a.升舱金额_第三方,a.升舱单量_百城达,a.升舱单量_第三方,a.统计日期 
            from RP_绩效_电商中心 a
            where   
                    (@0 is null or a.统计日期 = @0)  --月份
                    ) tb";

            var paras = new object[]
                {
                    Convert.ToDateTime(filter.Month)
                };

            var dataList = Context.Select<RP_绩效_电商中心>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<RP_绩效_电商中心>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id
            };

            pager.TotalRows = dataCount.QuerySingle();
            var list = dataList.OrderBy("tb.分销商商城编号 desc").Paging(pager.CurrentPage, filter.PageSize).QueryMany();
            pager.Rows = list;

            return pager;
        }

        /// <summary>
        /// 客服绩效分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>list</returns>
        /// <remarks>2013-12-11 黄志勇 添加</remarks>
        public override Pager<RP_绩效_客服> GetListServicePerformance(ParaServicePerformanceFilter filter)
        {
            const string sql =
                @"(select a.客服编号,a.客服名,a.单量,a.订单金额,a.新增会员, a.统计日期
                from RP_绩效_客服 a                                                   
                where  
                        (@0 is null or a.客服编号 = @0) and      --客服编号 
                        (@1 is null or  a.统计日期 = @1)               --统计日期    
                        ) tb";

            var paras = new object[]
                {
                    filter.ServiceNo,
                    Convert.ToDateTime(filter.Reptdt)
                };

            var dataList = Context.Select<RP_绩效_客服>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<RP_绩效_客服>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id
            };

            pager.TotalRows = dataCount.QuerySingle();
            var list = dataList.OrderBy("tb.客服编号,tb.统计日期 desc").Paging(pager.CurrentPage, filter.PageSize).QueryMany();
            pager.Rows = list;

            return pager;
        }

        /// <summary>
        /// 门店新增会员分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>list</returns>
        /// <remarks>2013-12-11 黄志勇 添加</remarks>
        public override Pager<RP_绩效_门店新增会员> GetListShopNewCustomer(ParaShopNewCustomerFilter filter)
        {
            var sql = string.Empty;
            object[] paras;
            if (filter.HasAllShop)
            {
                sql =
                @"(select a.门店编号,a.门店名称,a.新增会员总数,a.消费金额满30的会员数,a.新增会员销售, a.统计日期
                    from RP_绩效_门店新增会员 a                                                   
                    where  
                            (@0 is null or a.门店编号 = @0) and            --门店编号 
                            (@1 is null or  a.统计日期 = @1)               --统计日期    
                            ) tb";
                paras = new object[]
                {
                    filter.ShopNo,
                    Convert.ToDateTime(filter.Reptdt)
                };
            }
            else
            {
                sql =
                    @"(select a.门店编号,a.门店名称,a.新增会员总数,a.消费金额满30的会员数,a.新增会员销售, a.统计日期
                    from RP_绩效_门店新增会员 a    
                    left join SyUserWarehouse b
                    on a.门店编号 = b.warehousesysno                                              
                    where b.UserSysNo = @0  and                                 --系统用户编号  
                            (@1 is null or a.门店编号 = @1) and            --门店编号 
                            (@2 is null or  a.统计日期 = @2)               --统计日期  
                    ) tb";
                paras = new object[]
                {
                    filter.UserNo,
                    filter.ShopNo,
                    Convert.ToDateTime(filter.Reptdt),
                };
            }

            var dataList = Context.Select<RP_绩效_门店新增会员>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<RP_绩效_门店新增会员>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id
            };

            pager.TotalRows = dataCount.QuerySingle();
            var list = dataList.OrderBy("tb.统计日期 desc, tb.门店名称 Collate Chinese_PRC_Stroke_ci_as").Paging(pager.CurrentPage, filter.PageSize).QueryMany();
            pager.Rows = list;

            return pager;
        }

        /// <summary>
        /// 门店新增会员明细分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>list</returns>
        /// <remarks>2013-12-11 黄志勇 添加</remarks>
        public override Pager<rp_ShopNewCustomerDetail> GetListShopNewCustomerDetail(ParaShopNewCustomerFilter filter)
        {
            var sql = string.Empty;
            object[] paras;
            int i = 0;
            if (filter.HasAllShop)
            {
                //有所有门店权限
                sql = @"(select * from rp_ShopNewCustomerDetail where CONVERT(varchar(7),RegisterDate,120) = @0 and (@1 is null or warehousesysno=@1) ) tb";
                paras = new object[] { filter.Reptdt, filter.ShopNo };

            }
            else
            {
                //拥有部份门店权限
                sql = @"(select d.* from rp_ShopNewCustomerDetail d
                        inner join SyUserWarehouse uw ON uw.WarehouseSysNo = d.WarehouseSysNo
                        where uw.UserSysNo = @0 and CONVERT(varchar(7),d.RegisterDate,120) = @1 and (@2 is null or d.warehousesysno=@2) ) tb";
                paras = new object[] { filter.UserNo, filter.Reptdt, filter.ShopNo };
            }

            var dataList = Context.Select<rp_ShopNewCustomerDetail>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<rp_ShopNewCustomerDetail>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id
            };

            pager.TotalRows = dataCount.QuerySingle();
            var list = dataList.OrderBy("tb.Warehousename Collate Chinese_PRC_Stroke_ci_as, tb.RegisterDate desc").Paging(pager.CurrentPage, filter.PageSize).QueryMany();
            pager.Rows = list;

            return pager;
        }

        /// <summary>
        /// 查询门店新增会员明细
        /// </summary>
        /// <param name="customerSysno">客户编号</param>
        /// <returns>门店新增会员明细</returns>
        /// <remarks>2014-01-15 黄志勇 创建</remarks>
        public override rp_ShopNewCustomerDetail SelectShopNewCustomerDetail(int customerSysno)
        {
            return Context.Sql("select * from rp_ShopNewCustomerDetail where CustomerSysno=@CustomerSysno")
                 .Parameter("CustomerSysno", customerSysno).QuerySingle<rp_ShopNewCustomerDetail>();
        }

        /// <summary>
        /// 新增门店新增会员明细
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2014-01-15 黄志勇 创建</remarks>
        public override int InsertShopNewCustomerDetail(rp_ShopNewCustomerDetail entity)
        {
            return Context.Insert("rp_ShopNewCustomerDetail", entity)
                          .Column("CustomerSysno", entity.CustomerSysno)
                          .Column("WarehouseSysNo", entity.WarehouseSysNo)
                          .Column("Warehousename", entity.Warehousename)
                          .Column("IndoorStaffSysNo", entity.IndoorStaffSysNo)
                          .Column("IndoorStaffName", entity.IndoorStaffName)
                          .Column("CustomerName", entity.CustomerName)
                          .Column("MobilePhoneNumber", entity.MobilePhoneNumber)
                          .Column("Amount", entity.Amount)
                          .Column("RegisterDate", entity.RegisterDate)
                          .Execute();
        }

        /// <summary>
        /// 更新门店新增会员明细消费金额
        /// </summary>
        /// <param name="customerSysno">客户编号</param>
        /// <param name="amount">消费金额</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2014-01-15 黄志勇 创建</remarks>
        public override int UpdateShopNewCustomerDetail(int customerSysno, decimal amount)
        {
            return Context.Update("rp_ShopNewCustomerDetail")
            .Column("Amount", amount)
            .Where("CustomerSysno", customerSysno)
            .Execute();
        }

        /// <summary>
        /// 获取全部客服
        /// </summary>
        /// <returns>list</returns>
        /// <remarks>2013-12-11 黄志勇 添加</remarks>
        public override List<SyUser> GetAllService()
        {
            const string sql = @"select a.* from SyUser a
inner join SyGroupUser b
on a.SysNo = b.UserSysNo
where  b.GroupSysno=2";
            return Context.Sql(sql).QueryMany<SyUser>();
        }
        #endregion

        #region 业务员绩效

        /// <summary>
        /// 业务员绩效查询
        /// </summary>
        /// <param name="para">RP_绩效_业务员</param>
        /// <param name="warehouseSysNos">仓库列表</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dic(totalCount,业务员绩效集合)</returns>
        /// <remarks>2013-12-11 黄伟 创建</remarks>
        public override Dictionary<int, List<RP_绩效_业务员>> QueryBusinessManPerformance(ParaBusinessManPerformance para,
                                                                                     List<int> warehouseSysNos,
                                                                                     int currPageIndex = 1,
                                                                                     int pageSize = 10)
        {
            if (string.IsNullOrEmpty(para.统计日期))
            {
                para.统计日期 = DateTime.Now.Year + "-" + DateTime.Now.Month;
            }

            string sqlSelect = "t.*",
                   sqlFrom = @"RP_绩效_业务员 t left join whwarehouse w on t.仓库编号 = w.sysno",
                   sqlCondition = string.Format(
                       @"(@calDate is null or CONVERT(varchar(7),统计日期,120) = CONVERT(varchar(7),@calDate,120))
                         and (@IsSelfSupport is null or w.IsSelfSupport=@IsSelfSupport)
                          {0}",
                       warehouseSysNos.Any()
                           ? string.Format(" and t.仓库编号 in({0})", string.Join(",", warehouseSysNos))
                           : " and t.仓库编号 in(0)");

            using (var context = Context.UseSharedConnection(true))
            {
                var lstResult = context.Select<RP_绩效_业务员>(sqlSelect)
                                       .From(sqlFrom)
                                       .AndWhere(sqlCondition)
                                       .Parameter("calDate", para.统计日期)
                                       .Parameter("IsSelfSupport", para.IsSelfSupport)
                                       .Paging(currPageIndex, pageSize) //index从1开始
                                       .OrderBy("统计日期 desc")
                                       .QueryMany();
                var count = context.Select<int>(@"count(*)")
                                   .From(sqlFrom)
                                   .AndWhere(sqlCondition)
                                   .Parameter("calDate", para.统计日期)
                                   .Parameter("IsSelfSupport", para.IsSelfSupport)
                                   .QuerySingle();
                return new Dictionary<int, List<RP_绩效_业务员>> { { count, lstResult } };
            }
        }

        #endregion

        #region 办事处绩效

        /// <summary>
        /// 办事处绩效查询
        /// </summary>
        /// <param name="para">rp_绩效_办事处</param>
        /// <param name="warehouseSysNos">仓库列表</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dic(totalCount,办事处绩效集合)</returns>
        /// <remarks>2013-12-11 黄伟 创建</remarks>
        public override Dictionary<int, List<rp_绩效_办事处>> QueryOfficePerformance(rp_绩效_办事处 para,
                                                                                List<int> warehouseSysNos,
                                                                                int currPageIndex = 1,
                                                                                int pageSize = 10)
        {
            string sqlSelect = "t.*",
                   sqlFrom = @"rp_绩效_办事处 t",
                   sqlCondition = string.Format(
                       @"(@calDate is null or CONVERT(varchar(7),统计日期,120)= CONVERT(varchar(7),@calDate,120))
                          {0}",
                       warehouseSysNos.Any()
                           ? string.Format(" and t.办事处编号 in({0})", string.Join(",", warehouseSysNos))
                           : "");

            using (var context = Context.UseSharedConnection(true))
            {
                var lstResult = context.Select<rp_绩效_办事处>(sqlSelect)
                                       .From(sqlFrom)
                                       .AndWhere(sqlCondition)
                                       .Parameter("calDate", para.统计日期)
                                       .Paging(currPageIndex, pageSize) //index从1开始
                                       .OrderBy("统计日期 desc")
                                       .QueryMany();
                var count = context.Select<int>(@"count(*)")
                                   .From(sqlFrom)
                                   .AndWhere(sqlCondition)
                                   .Parameter("calDate", para.统计日期)
                                   .QuerySingle();
                return new Dictionary<int, List<rp_绩效_办事处>> { { count, lstResult } };
            }
        }

        #endregion

        #region 仓库内勤绩效
        /// <summary>
        /// 获取仓库内勤绩效报表
        /// </summary>
        /// <param name="pagerFilter">页面传入的过滤条件</param>
        /// <param name="currentUserSysNo">当前用户系统编号</param>
        /// <param name="hasAllWarehouse">是否具有全部仓库权限</param>
        /// <returns>Pager of rp_仓库内勤</returns>
        /// <remarks>2013-12-11 沈强 创建</remarks>
        public override Pager<rp_仓库内勤> SearchWarehouseInsideStaff(
            Pager<ParaWarehouseInsideStaffFilter> pagerFilter, int currentUserSysNo, bool hasAllWarehouse)
        {
            var filter = pagerFilter.PageFilter;
            var pager = new Pager<rp_仓库内勤>
            {
                CurrentPage = pagerFilter.CurrentPage,
                PageSize = pagerFilter.PageSize
            };

            #region 查询用sql

            string selectSql = @"select * from
                                    (
                                        select a.* 
                                            ,row_number() over (order by a.统计日期 desc,a.仓库,a.内勤编号) FLUENTDATA_ROWNUMBER 
                                        from rp_绩效_仓库内勤 a
                                        where (@0 is null or a.仓库编号 in({2})) 
                                              and (@1 is null or a.统计日期 = @1)
                                              and (@2 is null or exists(select 1 from SyUserWarehouse e  where e.WarehouseSysNo = a.仓库编号 and e.UserSysNo = @3))
                                    ) tempTable
                                    where fluentdata_RowNumber between {0} and {1}
                                    order by fluentdata_RowNumber";

            string countSql = @"select count(1) 
                                    from rp_绩效_仓库内勤 a
                                    where (@0 is null or a.仓库编号 in({0})) 
                                            and (@1 is null or a.统计日期 = @1)
                                            and (@2 is null or exists(select 1 from SyUserWarehouse e  where e.WarehouseSysNo = a.仓库编号 and e.UserSysNo = @3))";
            #endregion

            #region 设置默认参数
            //设置分页
            int beginNum = pager.PageSize * (pager.CurrentPage - 1) + 1;
            int endNum = beginNum + pager.PageSize - 1;

            //设置选择的仓库条件
            string warehouseSysNo = null;
            string sysNos = "null";
            if (filter.WarehouseSysNos != null && filter.WarehouseSysNos.Count > 0)
            {
                warehouseSysNo = "notNull";
                sysNos = string.Join(",", filter.WarehouseSysNos);
            }

            selectSql = string.Format(selectSql, beginNum, endNum, sysNos);
            countSql = string.Format(countSql, sysNos);
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                #region 设置查询参数
                //查询所有仓库，还是只查询有权限的部分仓库
                string allWarehouse = hasAllWarehouse ? null : "notNull";

                var param = new object[]
                    {
                        warehouseSysNo,
                        Convert.ToDateTime(filter.DateCalculated),
                        allWarehouse,
                        currentUserSysNo
                    };
                #endregion
                pager.TotalRows = context.Sql(countSql).Parameters(param).QuerySingle<int>();
                pager.Rows = context.Sql(selectSql).Parameters(param).QueryMany<rp_仓库内勤>();
            }

            return pager;
        }

        #endregion

        #region 配送报表数据
        /// <summary>
        /// 获取配送报表数据
        /// </summary>
        /// <param name="yyyymm">年月</param>
        /// <param name="pageindex">页数</param>
        /// <returns>配送报表数据</returns>
        /// <remarks>2014-02-18 朱成果 创建</remarks>
        public override List<CBPickingReportItem> GetPickingReport(string yyyymm, int pageindex)
        {
            string sql = @"(
select  c.ordersysno,e.sysno as shopno,e.shopname,b.name,b.streetaddress,
b.mobilephonenumber,b.areasysno,t.StockOutAmount,t.Receivable,
f.ProductSysNo,f.ProductName,f.ProductQuantity,f.OriginalPrice,f.RealSalesAmount,g.erpcode,k.mallorderid,t.status,func_getaereapath(b.areasysno) areaallname,t.createddate,t.notesysno as  stockoutno,t.remarks,d.freightamount ,t.expressno ,j.deliverytypename               
                                    from lgdeliveryitem t 
                                    left join soreceiveaddress b on t.addresssysno=b.sysno
                                    left join whstockout c on t.notesysno=c.sysno 
                                    left join soorder d on d.ordersource=80 and c.ordersysno=d.sysno 
                                    inner join DsDealerMall e on e.sysno=d.OrderSourceSysNo
                                    left outer join WhStockOutItem f on f.stockoutsysno=c.sysno
                                    left outer join PdProduct g on f.productsysno=g.sysno
                                    left outer join DsOrderItemAssociation m on m.soorderitemsysno=f.orderitemsysno
                                    left outer join DsOrderItem n on n.sysno=m.dsorderitemsysno
                                    left outer join DsOrder k on k.sysno=n.dsordersysno
                                    left join lgdeliverytype j on c.deliverytypesysno=j.sysno
                                    where t.status<>-10 and CONVERT(varchar(6),t.createddate,112)='{0}'
                                    ) tb";
            return Context.Select<CBPickingReportItem>("tb.*").From(string.Format(sql, yyyymm)).OrderBy("shopno,stockoutno desc").Paging(pageindex, 2000).QueryMany();
        }

        #endregion

        #region 优惠卡统计报表
        /// <summary>
        /// 优惠卡统计报表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>list</returns>
        /// <remarks>2014-02-26 朱家宏 创建</remarks>
        public override IList<CBRptCouponCard> QueryCouponCards(ParaRptCouponCardFilter filter)
        {
            var sql =
                @"select sysno as CardTypeSysNo,typename as CardTypeName,sum(totalNumber) totalNumber,sum(usedNumber) as usedNumber from 
                (
                select b.sysno,b.typename,count(b.sysno) as totalNumber,0 as usedNumber from spcouponcard a
                inner join spcouponcardtype b on a.cardtypesysno=b.sysno
                where b.status={1} group by b.sysno,b.typename
                union all
                select b.sysno,b.typename,0 as totalNumber,count(b.sysno) as usedNumber from spcouponcard a
                inner join spcouponcardtype b on a.cardtypesysno=b.sysno
                where b.status={1} and a.activationtime<>to_date('0001-01-01','yyyy-mm-dd') {0} group by b.sysno,b.typename
                ) tb group by sysno,typename
                order by sysno";
            var where = "";
            if (filter.BeginDate != null)
            {
                where += " and a.activationtime>=to_date('" + filter.BeginDate + "','yyyy-mm-dd hh24:mi:ss')";
            }
            if (filter.EndDate != null)
            {
                where += " and a.activationtime<to_date('" + filter.EndDate + "','yyyy-mm-dd hh24:mi:ss')";
            }
            sql = string.Format(sql, where, (int)Model.WorkflowStatus.PromotionStatus.优惠券卡类型状态.启用);

            return Context.Sql(sql).QueryMany<CBRptCouponCard>();
        }

        /// <summary>
        /// 优惠卡统计报表2
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>分页</returns>
        /// <remarks>2014-04-02 朱家宏 创建</remarks>
        public override Pager<CBRptCouponCard> QueryCouponCardsNew(ParaRptCouponCardFilter filter)
        {
            var sql =
                @"(select a.sysno,a.areaname,a.warehousename,sum(case when a.receivetime>=@p0p0 and a.receivetime<@p0p1 and a.couponsysno=0 then 1 else 0 end ) as totalNumber,
                        sum(case when  a.status={0} and a.auditordate>=@p0p0 and a.auditordate<@p0p1 and a.couponsysno!=0 then a.couponamount else 0 end) as totalAmount
                        from rp_优惠卡统计报表 a {1}
                        group by a.warehousename,a.areaname,a.sysno
                    ) tb";


            var where = "where 1=1";

            var paras = new ArrayList { filter.BeginDate, filter.EndDate };
            int i = 2;
            if (!string.IsNullOrWhiteSpace(filter.Warehouses))
            {
                where += " and exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col = warehouseSysno) ";
                paras.Add(filter.Warehouses);
                i++;
            }

            if (filter.CouponCardTypeSysNo != null)
            {
                where += " and CardTypeSysNo in (@p0p" + i + ") ";
                paras.Add(filter.CouponCardTypeSysNo);
                i++;
            }

            sql = string.Format(sql, Model.WorkflowStatus.PromotionStatus.优惠券状态.已使用.GetHashCode(), where);

            var dataList = Context.Select<CBRptCouponCard>("areaname,warehousename,totalNumber,totalAmount").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBRptCouponCard>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
            };

            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.sysno desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }

        #endregion

        #region 仓库销售排行报表
        /// <summary>
        /// 仓库销售排行报表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>数据</returns>
        /// <remarks>2014-04-04 朱成果 创建</remarks>
        public override List<RptSalesRanking> QueryWarehouseProductSales(ParaWarehouseProductSalesFilter filter)
        {
            var select = @"  ROW_NUMBER() over(order by tb.DealerName,tb.WarehouseName,tb.SalesQuantity DESC)  as RowNumber,  tb.* ";
            var sql = @"
               (select ROW_NUMBER() over(order by DsDealer.DealerName,WhWarehouse.WarehouseName,SUM(SoOrderItem.Quantity) DESC)  as RowNumber, 
                    DsDealer.DealerName ,WhWarehouse.WarehouseName,SoOrderItem.ProductSysNo,SoOrderItem.ProductName,
                    ProductCategoryName=(stuff
                                            ((SELECT   '' + PdCategory.CategoryName
                                              FROM       PdCategoryAssociation inner join PdCategory on PdCategory.SysNo=PdCategoryAssociation.CategorySysNo
                                              WHERE   PdCategoryAssociation.ProductSysNo = SoOrderItem.ProductSysNo and  PdCategoryAssociation.IsMaster=1 FOR xml path('')), 1, 0, ''))
                    ,ProductCategorySysNos=(stuff
                                            ((SELECT   '' + PdCategory.SysNos
                                              FROM       PdCategoryAssociation inner join PdCategory on PdCategory.SysNo=PdCategoryAssociation.CategorySysNo
                                              WHERE   PdCategoryAssociation.ProductSysNo = SoOrderItem.ProductSysNo and  PdCategoryAssociation.IsMaster=1 FOR xml path('')), 1, 0, '')),
                   SUM(SoOrderItem.Quantity) as SalesQuantity ,sum(SoOrderItem.SalesAmount) as SalesAmount,PdProductStock.StockQuantity
                from 
               DsDealer inner join 
               DsDealerWharehouse on DsDealer.SysNo=DsDealerWharehouse.DealerSysNo 
               inner join WhWarehouse on WhWarehouse.SysNo=DsDealerWharehouse.WarehouseSysNo
               right join SoOrder on SoOrder.DefaultWarehouseSysNo=WhWarehouse.SysNo 
               inner join SoOrderItem on SoOrder.SysNo=SoOrderItem.OrderSysNo 
               inner join PdProductStock on PdProductStock.WarehouseSysNo=WhWarehouse.SysNo and PdProductStock.PdProductSysNo=SoOrderItem.ProductSysNo
                where
              {0}
               group by DsDealer.DealerName,WhWarehouse.WarehouseName,SoOrderItem.ProductSysNo,SoOrderItem.ProductName,PdProductStock.StockQuantity
                ) tb ";

            var paras = new ArrayList();
            var where = "1=1 and SoOrder.Status>=30 ";
            int i = 0;
            if (filter.BeginDate.HasValue)
            {
                where += " and SoOrder.CreateDate>=@p0p" + i;
                paras.Add(filter.BeginDate);
                i++;
            }
            if (filter.EndDate.HasValue)
            {
                where += " and SoOrder.CreateDate<@p0p" + i;
                paras.Add(filter.EndDate.Value.AddDays(1));
                i++;
            }
            if (!string.IsNullOrEmpty(filter.WhWarehouseIDS))
            {
                where += " and  WhWarehouse.SysNo in (" + filter.WhWarehouseIDS + ")  ";
                paras.Add(filter.WhWarehouseIDS);
                i++;
            }
            if (filter.ProductCategories != null)
            {
                //var categories = string.Join(",", filter.ProductCategories);
                string proCate="";
                foreach(int cate in  filter.ProductCategories)
                {
                    if(proCate!="")
                    {
                        proCate +=" or ";
                    }
                    proCate+=" ProductCategorySysNos like '%,"+cate+",%' ";
                }
                sql += " where (" + proCate + ") ";
                //where +=
                //    " and  exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col=a.商品分类编号)";
                //paras.Add(categories);
                //i++;
            }
            sql = string.Format(sql, where);
            //if (filter.TakingCount > 0)
            //{
            //    select = " top " + filter.TakingCount + "  " + select;
            //    //sql += " where rowNumber<=@p0p" + i;
            //    //paras.Add(filter.TakingCount);
            //    //i++;
            //}
            sql += " order by tb.DealerName,tb.WarehouseName,tb.SalesQuantity  desc ";
            var dataList = Context.Select<RptSalesRanking>(select).From(sql).Parameters(paras).QueryMany();
            return dataList;
        }

        #endregion

        #region 销量统计报表

        /// <summary>
        /// 销量统计报表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>分页</returns>
        /// <remarks>2014-04-08 朱家宏 创建</remarks>
        public override Pager<CBRptSales> QuerySales(ParaRptSalesFilter filter)
        {
            var sql =
                @" 
               
               DsDealer inner join 
               DsDealerWharehouse on DsDealer.SysNo=DsDealerWharehouse.DealerSysNo 
               inner join WhWarehouse on WhWarehouse.SysNo=DsDealerWharehouse.WarehouseSysNo where {0}";

            var paras = new ArrayList();
            var where = "1=1";
            int i = 0;
            //if (!string.IsNullOrWhiteSpace(filter.BeginDate))
            //{
            //    where += " and 统计日期>=:BeginDate ";
            //    paras.Add(filter.BeginDate);
            //}
            //if (!string.IsNullOrWhiteSpace(filter.EndDate))
            //{
            //    where += " and 统计日期<:EndDate ";
            //    paras.Add(filter.EndDate);
            //}
            //if (!string.IsNullOrWhiteSpace(filter.BeginDate))
            //{
            //    where += " and 统计日期>=:BeginDate ";
            //    paras.Add(filter.BeginDate);
            //}
            //if (!string.IsNullOrWhiteSpace(filter.Month))
            //{
            //    where += " and 统计日期 =@p0p" + i;
            //    paras.Add(Convert.ToDateTime(filter.Month));
            //    i++;
            //}
            if (!string.IsNullOrWhiteSpace(filter.Warehouses))
            {
                where += " and WhWarehouse.SysNo in (" + filter.Warehouses + ") ";
                //paras.Add(filter.Warehouses);
                //i++;
            }
           
            sql = string.Format(sql, where);
            
            ///门店线下货品统计
            string selectData = @" '" + filter.Month + @"' as Month ,  DsDealer.DealerName as AreaName ,WhWarehouse.WarehouseName as Warehouse
                                                        ,CountOfStore =(select count(*) from DsPosOrder inner join DsDealerWharehouse on DsPosOrder.DsSysNo=DsDealerWharehouse.DealerSysNo where DsPosOrder.DsSysNo=DsDealer.SysNo and DsDealerWharehouse.WarehouseSysNo=WhWarehouse.SysNo and year(DsPosOrder.SaleTime) =" + filter.Month.Split('-')[0] + @" and month(DsPosOrder.SaleTime)=" + filter.Month.Split('-')[1] + @")
                                                        ,SummationOfStore =(select sum(DsPosOrder.TotalSellValue) from DsPosOrder inner join DsDealerWharehouse on DsPosOrder.DsSysNo=DsDealerWharehouse.DealerSysNo where DsPosOrder.DsSysNo=DsDealer.SysNo and DsDealerWharehouse.WarehouseSysNo=WhWarehouse.SysNo and year(DsPosOrder.SaleTime) =" + filter.Month.Split('-')[0] + @" and month(DsPosOrder.SaleTime)=" + filter.Month.Split('-')[1] + @")
                                                        ,CountOfHytDsf=(select count(*) from SoOrder where SoOrder.DealerSysNo=DsDealer.SysNo and SoOrder.DefaultWarehouseSysNo=WhWarehouse.SysNo and year(CreateDate) =" + filter.Month.Split('-')[0] + @" and month(CreateDate)=" + filter.Month.Split('-')[1] + @" ) 
                                                        ,SummationOfHytDsf=(select sum(SoOrder.CashPay) from SoOrder where SoOrder.DealerSysNo=DsDealer.SysNo and SoOrder.DefaultWarehouseSysNo=WhWarehouse.SysNo and year(CreateDate) =" + filter.Month.Split('-')[0] + @" and month(CreateDate)=" + filter.Month.Split('-')[1] + @"   ) ";
            ////检查是否有线下收银的销售表 2016-06-20 杨云奕 添加
            int checkTable= Context.Sql("select count(*) from sysobjects where name='DsPosOrder'").QuerySingle<int>();
            if(checkTable==0)
            {
                ///没有线下收银销售表，那么就不进行门店销售统计 2016-06-20 杨云奕 添加
                selectData = @" '" + filter.Month + @"' as Month ,  DsDealer.DealerName as AreaName ,WhWarehouse.WarehouseName as Warehouse
                                                         ,CountOfStore =(select count(*) from SoOrder where SoOrder.DealerSysNo=DsDealer.SysNo and SoOrder.DefaultWarehouseSysNo=WhWarehouse.SysNo and year(CreateDate) =" + filter.Month.Split('-')[0] + @" and month(CreateDate)=" + filter.Month.Split('-')[1] + @" and SoOrder.OrderSource='" + (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.门店下单 + @"') 
                                                        ,SummationOfStore = (select sum(SoOrder.CashPay) from SoOrder where SoOrder.DealerSysNo=DsDealer.SysNo and SoOrder.DefaultWarehouseSysNo=WhWarehouse.SysNo and year(CreateDate) =" + filter.Month.Split('-')[0] + @" and month(CreateDate)=" + filter.Month.Split('-')[1] + @"   and SoOrder.OrderSource='" + (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.门店下单 + @"') 
                                                        ,CountOfHytDsf=(select count(*) from SoOrder where SoOrder.DealerSysNo=DsDealer.SysNo and SoOrder.DefaultWarehouseSysNo=WhWarehouse.SysNo and year(CreateDate) =" + filter.Month.Split('-')[0] + @" and month(CreateDate)=" + filter.Month.Split('-')[1] + @" and (SoOrder.OrderSource='" + (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.PC网站 + @"' or SoOrder.OrderSource='" + (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.手机商城 + @"' ) ) 
                                                        ,SummationOfHytDsf=(select sum(SoOrder.CashPay) from SoOrder where SoOrder.DealerSysNo=DsDealer.SysNo and SoOrder.DefaultWarehouseSysNo=WhWarehouse.SysNo and year(CreateDate) =" + filter.Month.Split('-')[0] + @" and month(CreateDate)=" + filter.Month.Split('-')[1] + @" and (SoOrder.OrderSource='" + (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.PC网站 + @"' or SoOrder.OrderSource='" + (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.手机商城 + @"' )  ) ";
            }
            var dataList = Context.Select<CBRptSales>(selectData).From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBRptSales>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
            };

            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("WhWarehouse.SysNo").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }


        /// <summary>
        /// 销量统计报表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>分页</returns>
        /// <remarks>2014-04-08 朱家宏 创建</remarks>
        public override Pager<CBRptSales> QuerySalesByDay(ParaRptSalesFilter filter)
        {
            var sql =
                @" 
               
               DsDealer inner join 
               DsDealerWharehouse on DsDealer.SysNo=DsDealerWharehouse.DealerSysNo 
               inner join WhWarehouse on WhWarehouse.SysNo=DsDealerWharehouse.WarehouseSysNo where {0}";

            var paras = new ArrayList();
            var where = "1=1";
            int i = 0;
            //if (!string.IsNullOrWhiteSpace(filter.BeginDate))
            //{
            //    where += " and 统计日期>=:BeginDate ";
            //    paras.Add(filter.BeginDate);
            //}
            //if (!string.IsNullOrWhiteSpace(filter.EndDate))
            //{
            //    where += " and 统计日期<:EndDate ";
            //    paras.Add(filter.EndDate);
            //}
            //if (!string.IsNullOrWhiteSpace(filter.BeginDate))
            //{
            //    where += " and 统计日期>=:BeginDate ";
            //    paras.Add(filter.BeginDate);
            //}
            //if (!string.IsNullOrWhiteSpace(filter.Month))
            //{
            //    where += " and 统计日期 =@p0p" + i;
            //    paras.Add(Convert.ToDateTime(filter.Month));
            //    i++;
            //}
            if (!string.IsNullOrWhiteSpace(filter.Warehouses))
            {
                where += " and WhWarehouse.SysNo in (" + filter.Warehouses + ") ";
                //paras.Add(filter.Warehouses);
                //i++;
            }
           
            sql = string.Format(sql, where);

            ///门店线下货品统计
            string selectData = @" '" + filter.Month + @"' as Month ,  DsDealer.DealerName as AreaName ,WhWarehouse.WarehouseName as Warehouse
                                                        ,CountOfStore =(select count(*) from DsPosOrder inner join DsDealerWharehouse on DsPosOrder.DsSysNo=DsDealerWharehouse.DealerSysNo where DsPosOrder.DsSysNo=DsDealer.SysNo and DsDealerWharehouse.WarehouseSysNo=WhWarehouse.SysNo and DsPosOrder.SaleTime>='"+filter.BeginDate +" 00:00:00' and DsPosOrder.SaleTime<='"+filter.EndDate +@" 23:59:59' )
                                                        ,SummationOfStore =(select sum(DsPosOrder.TotalSellValue) from DsPosOrder inner join DsDealerWharehouse on DsPosOrder.DsSysNo=DsDealerWharehouse.DealerSysNo where DsPosOrder.DsSysNo=DsDealer.SysNo and DsDealerWharehouse.WarehouseSysNo=WhWarehouse.SysNo  and DsPosOrder.SaleTime>='" + filter.BeginDate + " 00:00:00' and DsPosOrder.SaleTime<='" + filter.EndDate + @" 23:59:59')
                                                        ,CountOfStoreByReturn =(select count(*) from DsPosReturnOrder inner join DsPosOrder on [DsPosReturnOrder].OrderSysNo=DsPosOrder.sysNo inner join DsDealerWharehouse on DsPosOrder.DsSysNo=DsDealerWharehouse.DealerSysNo where DsPosOrder.DsSysNo=DsDealer.SysNo and DsDealerWharehouse.WarehouseSysNo=WhWarehouse.SysNo and DsPosReturnOrder.ReturnTime>='" + filter.BeginDate + " 00:00:00' and DsPosReturnOrder.ReturnTime<='" + filter.EndDate + @" 23:59:59' )
                                                        ,SummationOfStoreByReturn =(select sum(DsPosReturnOrder.TotalReturnValue) from DsPosReturnOrder inner join DsPosOrder on [DsPosReturnOrder].OrderSysNo=DsPosOrder.sysNo inner join DsDealerWharehouse on DsPosOrder.DsSysNo=DsDealerWharehouse.DealerSysNo where DsPosOrder.DsSysNo=DsDealer.SysNo and DsDealerWharehouse.WarehouseSysNo=WhWarehouse.SysNo  and DsPosReturnOrder.ReturnTime>='" + filter.BeginDate + " 00:00:00' and DsPosReturnOrder.ReturnTime<='" + filter.EndDate + @" 23:59:59')
                                                        
                                                        ,CountOfHytDsf=(select count(*) from SoOrder where  SoOrder.DefaultWarehouseSysNo=WhWarehouse.SysNo  and SoOrder.CreateDate>='" + filter.BeginDate + " 00:00:00' and SoOrder.CreateDate<='" + filter.EndDate + @" 23:59:59'  and SoOrder.Status>=30) 
                                                        ,SummationOfHytDsf=(select sum(SoOrder.CashPay) from SoOrder where  SoOrder.DefaultWarehouseSysNo=WhWarehouse.SysNo  and SoOrder.CreateDate>='" + filter.BeginDate + " 00:00:00' and SoOrder.CreateDate<='" + filter.EndDate + @" 23:59:59'  and SoOrder.Status>=30) ";
            ////检查是否有线下收银的销售表 2016-06-20 杨云奕 添加
            int checkTable = Context.Sql("select count(*) from sysobjects where name='DsPosOrder'").QuerySingle<int>();
            if (checkTable == 0)
            {
                ///没有线下收银销售表，那么就不进行门店销售统计 2016-06-20 杨云奕 添加
                ///SoOrder.DealerSysNo=DsDealer.SysNo and
                selectData = @" '" + filter.Month + @"' as Month ,  DsDealer.DealerName as AreaName ,WhWarehouse.WarehouseName as Warehouse
                                                         ,CountOfStore =(select count(*) from SoOrder where  SoOrder.DealerSysNo=DsDealer.SysNo and SoOrder.DefaultWarehouseSysNo=WhWarehouse.SysNo  and DsPosOrder.SaleTime>='" + filter.BeginDate + " 00:00:00' and DsPosOrder.SaleTime<='" + filter.EndDate + @" 23:59:59' and SoOrder.OrderSource='" + (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.门店下单 + @"') 
                                                        ,SummationOfStore = (select sum(SoOrder.CashPay) from SoOrder where SoOrder.DealerSysNo=DsDealer.SysNo and SoOrder.DefaultWarehouseSysNo=WhWarehouse.SysNo  and DsPosOrder.SaleTime>='" + filter.BeginDate + " 00:00:00' and DsPosOrder.SaleTime<='" + filter.EndDate + @" 23:59:59'   and SoOrder.OrderSource='" + (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.门店下单 + @"') 
                                                        ,CountOfHytDsf=(select count(*) from SoOrder where  SoOrder.DefaultWarehouseSysNo=WhWarehouse.SysNo  and SoOrder.CreateDate>='" + filter.BeginDate + " 00:00:00' and SoOrder.CreateDate<='" + filter.EndDate + @" 23:59:59' and (SoOrder.OrderSource='" + (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.PC网站 + @"' or SoOrder.OrderSource='" + (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.手机商城 + @"' or SoOrder.OrderSource='" + (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.客服下单 + @"' ) ) 
                                                        ,SummationOfHytDsf=(select sum(SoOrder.CashPay) from SoOrder where  SoOrder.DefaultWarehouseSysNo=WhWarehouse.SysNo  and SoOrder.CreateDate>='" + filter.BeginDate + " 00:00:00' and SoOrder.CreateDate<='" + filter.EndDate + @" 23:59:59' and (SoOrder.OrderSource='" + (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.PC网站 + @"' or SoOrder.OrderSource='" + (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.手机商城 + @"' or SoOrder.OrderSource='" + (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.客服下单 + @"' )  ) ";
            }
            var dataList = Context.Select<CBRptSales>(selectData).From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBRptSales>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
            };

            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("WhWarehouse.SysNo").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }

        #endregion

        #region 升舱销量统计报表

        /// <summary>
        /// 升舱销量统计报表
        /// </summary>
        /// <param name="filter">参数</param>
        /// <returns>报表</returns>
        /// <remarks>2014-04-16 朱家宏 创建</remarks>
        public override Pager<CBRptUpgradeSales> QueryUpgradeSales(ParaRptUpgradeSalesFilter filter)
        {
            var sql =
                 @"(select * from rp_升舱销量统计报表
                    where  {0}
                    ) tb";

            var paras = new ArrayList();
            var where = "1=1";
            if (filter.StatsDate != null)
            {
                where += " and StatsDate=@p0p0 ";
                paras.Add(filter.StatsDate);
            }

            sql = string.Format(sql, where);

            var dataList = Context.Select<CBRptUpgradeSales>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBRptUpgradeSales>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
            };

            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.StatsDate").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }

        #endregion

        #region 快递100相关报表
        /// <summary>
        /// 获取快递100服务月统计报表
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        /// <remarks>2014-05-20 朱成果 创建</remarks>
        public override List<CBMonthExpress> GetMonthExpressListByYear(int year)
        {
            string sql = string.Format(@"
                with tb as 
                (
                  select CONVERT(varchar(7),t.PostTime,120) as YearMonth,PostResultStatus
                         from LgExpressInfo t where CONVERT(varchar(4),t.PostTime,120)='{0}' 
                )
                select YearMonth,COUNT (CASE WHEN PostResultStatus = 200 THEN 1  ELSE NULL END) SuccessFlgs,COUNT (CASE WHEN PostResultStatus <> 200 THEN 1  ELSE NULL END) FailFlgs,count(*) as AllFlgs
                from tb
                group by YearMonth
                order by YearMonth asc
                ", year);
            return Context.Sql(sql).QueryMany<CBMonthExpress>();
        }


        /// <summary>
        /// 获取快递100服务明细
        /// </summary>
        /// <param name="filter">筛选</param>
        /// <returns></returns>
        /// <remarks>2014-05-20 朱成果 创建</remarks>
        public override Pager<CBLgExpressDetail> GetLgExpressList(ParaExpressInfoFilter filter)
        {
            string where = "1=1";
            if (filter.IsSuccess.HasValue)
            {
                if (filter.IsSuccess.Value)
                {
                    where += " and m.PostResultStatus=200";

                }
                else
                {
                    where += " and m.PostResultStatus<>200";
                }
            }
            ArrayList lst = new ArrayList();
            int i = 0;
            if (filter.StartTime.HasValue)
            {
                where += " and m.PostTime>=@p0p" + i;
                lst.Add(filter.StartTime.Value);
                i++;
            }
            if (filter.EndTime.HasValue)
            {
                where += " and m.PostTime<@p0p" + i;
                lst.Add(filter.EndTime.Value.AddDays(1));
                i++;
            }
            Pager<CBLgExpressDetail> r = new Pager<CBLgExpressDetail>();
            string selectTB = @"
                            LgExpressInfo m
                            inner join SoOrder  n
                            on m.transactionsysno=n.transactionsysno
                            inner join LgDeliveryItem f on f.notetype=10 and f.transactionsysno=m.transactionsysno and TRIM(nvl(f.expressno,'-1'))=nvl(m.expressno,'-1') and f.status<>-10";
            r.CurrentPage = filter.Id;
            r.PageSize = filter.PageSize;
            if (lst.Count > 0)
            {

                r.TotalRows = Context.Select<int>("count(0)").From(selectTB)
                                                            .Where(where)
                                                            .Parameters(lst)
                                                            .QuerySingle();
                r.Rows = Context.Select<CBLgExpressDetail>("m.*,n.sysno as OrderSysNo,f.notesysno as OutStockNo")
                                                        .From(selectTB)
                                                         .Where(where)
                                                         .Parameters(lst)
                                                         .OrderBy("m.PostTime")
                                                         .Paging(filter.Id, filter.PageSize).QueryMany();
            }
            else
            {
                r.TotalRows = Context.Select<int>("count(0)").From(selectTB)
                                                       .Where(where)
                                                       .QuerySingle();
                r.Rows = Context.Select<CBLgExpressDetail>("m.*,n.sysno as OrderSysNo,f.notesysno as OutStockNo")
                                                     .From(selectTB)
                                                     .Where(where)
                                                     .OrderBy("m.PostTime")
                                                     .Paging(filter.Id, filter.PageSize).QueryMany();
            }
            return r;
        }

        /// <summary>
        /// 快递100报表查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>list</returns>
        /// <remarks>2014-05-19 余勇 创建</remarks>
        public override IList<CBRptLgExpressInfo> QueryLgExpress(ParaRptLgExpressFilter filter)
        {
            var sql = @"(SELECT CONVERT(varchar(7),a.PostTime,120) AS 统计日期,
                               count(b.SysNo) AS 成功单量，
                               count(c.SysNo) AS 失败单量,
                               count(a.SysNo) AS 总单量
                          FROM  LgExpressInfo a
                          left join  (select SysNo from LgExpressInfo where PostResultStatus=200) b
                            ON a.SysNo = b.SYSNO
                          left join (select SysNo from LgExpressInfo where PostResultStatus<>200) c
                            ON a.SysNo = c.SYSNO
                          where {0}  
                            group by CONVERT(varchar(7),a.PostTime,120)
                      ) tb";

            var paras = new ArrayList();
            var where = "1=1 ";

            if (!string.IsNullOrEmpty(filter.Year))
            {
                where += " and CONVERT(varchar(4),a.PostTime,120) =@p0p0";
                paras.Add(filter.Year);
            }

            sql = string.Format(sql, where);

            return Context.Select<CBRptLgExpressInfo>("tb.*").From(sql).Parameters(paras).QueryMany();
        }

        /// <summary>
        /// 快递100报表明细查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>list</returns>
        /// <remarks>2014-05-19 余勇 创建</remarks>
        public override Pager<LgExpressInfo> QueryLgExpressDetail(
            ParaRptLgExpressFilter filter)
        {
            var sql = @"(select *
                            from  LgExpressInfo
                            where {0}
                      ) tb";

            var where = "1=1 ";

            var paras = new ArrayList();
            int i = 0;
            if (!string.IsNullOrWhiteSpace(filter.Month))
            {
                where += " and CONVERT(varchar(7),PostTime,120) =@p0p" + i;
                paras.Add(filter.Month);
                i++;
            }
            if (filter.DataType.HasValue)
            {
                int dataType = filter.DataType.Value;
                if (dataType == 1)
                {
                    where += " and PostResultStatus = 200";
                    paras.Add(filter.DataType);
                }
                else if (dataType == 2)
                {
                    where += " and PostResultStatus <> 200";
                    paras.Add(filter.DataType);
                }
            }

            sql = string.Format(sql, where);

            var dataList = Context.Select<LgExpressInfo>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<LgExpressInfo>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
            };

            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.SysNo").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }
        #endregion

        #region 区域销售统计报表
        /// <summary>
        /// 区域销售统计报表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="lstResultAll">合计</param>
        /// <returns>列表</returns>
        /// <remarks>2014-08-11 余勇 创建
        /// </remarks>
        public override Pager<CBRptRegionalSales> QueryRegionalSales(ParaRptRegionalSales filter, out CBRptRegionalSales lstResultAll)
        {
            string sql = @"(select row_number() over (order by 省,市,区) RowNumber,
                            办事处 AreaName,
                            省 Province,
                            市 City,
                            区 Area,
                            sum((case  when (是否第三方快递 = 0 and 金额 > 0) then
                                              1
                                             else
                                              0
                                   end
                                 )
                                ) CountOfHytBcd,
                            sum((case  when (是否第三方快递 = 1 and 金额 > 0) then
                                              1
                                             else
                                              0
                                   end
                                 )
                                ) CountOfHytDsf,    
                            sum((case when 是否第三方快递 = 0 then
                                              金额
                                             else
                                              0
                                           end)) SummationOfHytBcd,
                            sum((case when 是否第三方快递 = 1 then
                                              金额
                                             else
                                              0
                                           end)) SummationOfHytDsf              

                            from (
                                select 
                                bso.sysno 办事处编号,
                                bso.name 办事处,
                                a.省,
                                a.市,
                                a.区,
                                --b.地区编号 客户地区编号,
                                (case when 配送编号=1 then 0
                                     else 1
                                      end ) 
                                      是否第三方快递,
                                a.实收金额 金额
                                from  (select 省,市,区,收货地区编号,仓库编号,配送编号,sum(实收金额) 实收金额,订单来源,结算状态,订单号,CONVERT(varchar(7),出库日期,120) 出库日期
                                             from rp_销售明细 
                                     group by 省,市,区,收货地区编号,仓库编号,配送编号,订单来源,结算状态,订单号,CONVERT(varchar(7),出库日期,120)) a  
                                 left join (select areasysno,min(warehousesysno) warehousesysno  from WhWarehouseArea group by areasysno) wwha on a.收货地区编号=wwha.areasysno
                                 left join  WhWarehouse whw on wwha.warehousesysno=whw.sysno
                                 left join (select  min(organizationsysno) organizationsysno ,warehousesysno from BsOrganizationWarehouse group by warehousesysno) bsowh
                                    on whw.sysno = bsowh.warehousesysno
                                 left join bsorganization bso
                                    on bsowh.organizationsysno = bso.sysno
                                where {0}
                            )tem1
                            group by 
                            办事处编号,
                            办事处,
                            省,
                            市,
                            区) tb";
            var where = " 配送编号 In (1,3) and 订单来源<>100 and 结算状态='已结算'";

            var paras = new ArrayList();
            int i = 0;
            if (!string.IsNullOrWhiteSpace(filter.Month))
            {
                where += " and 出库日期 =@p0p" + i;
                paras.Add(filter.Month);
                i++;
            }

            if (!string.IsNullOrWhiteSpace(filter.Area))
            {
                List<string> areaList = filter.Area.Split(',').ToList();
                var first = areaList.First().Split('-').ToList();
                if (first.Count == 3)
                {
                    where += string.Format(" and  省||'-'||市||'-'||区   in ('{0}')", areaList.Join("','"));
                }
                else if (first.Count == 2)
                {
                    where += string.Format(" and  省||'-'||市  in ('{0}')", areaList.Join("','"));
                }
                else
                {
                    where += string.Format(" and  省 in ('{0}')", areaList.Join("','"));
                }

            }

            sql = string.Format(sql, where);

            var dataList = Context.Select<CBRptRegionalSales>("tb.*").From(sql);
            //付款金额合计 2014-05-05 朱家宏添加
            var dataAll = Context.Select<CBRptRegionalSales>(@"count(0) 'RowCount', sum(CountOfHytBcd) CountOfHytBcd,sum(CountOfHytDsf) CountOfHytDsf,sum(SummationOfHytBcd) SummationOfHytBcd,sum(SummationOfHytDsf) SummationOfHytDsf").From(sql);


            dataList.Parameters(paras);
            dataAll.Parameters(paras);

            var pager = new Pager<CBRptRegionalSales>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
            };

            var rows = dataList.OrderBy("tb.RowNumber").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            lstResultAll = dataAll.QuerySingle();
            pager.TotalRows = lstResultAll.RowCount;
            pager.Rows = rows;

            return pager;
        }

        #endregion

        #region 加盟商当日达对账报表
        /// <summary>
        /// 加盟商当日达对账报表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>列表</returns>
        /// <remarks>2014-08-21 余勇 创建</remarks>
        public override Pager<RP_非自营销售明细> GetFranchiseesSaleDetail(ParaFranchiseesSaleDetail filter)
        {
            var sql = @"(SELECT 订单号,出库日期,商城订单号,商城名称,ERP编码,产品名称,数量,单价,优惠,销售金额,实收金额,下单门店,订单来源,
                            订单来源编号,仓库编号,出库仓库,收款方式,配送方式,订单状态,出库单状态,结算状态,结算日期,升舱时间,c.ErpCode 加盟商ERP编号,c.ErpName 加盟商ERP名称,
                            (case when 收款单状态='已确认' and CONVERT(varchar(7),收款单确认时间,120)>'{0}' then '待确认' else 收款单状态 end) 收款单状态,
                            (case when 收款单状态='已确认' and CONVERT(varchar(7),收款单确认时间,120)>'{1}' then to_date('0001-01-01','yyyy-mm-dd') else 收款单确认时间 end) 收款单确认时间
                          FROM  RP_非自营销售明细 a 
                          left join DsDealerWharehouse b on a.仓库编号=b.WarehouseSysNo
                          left join DsDealer c on b.dealersysno=c.sysno
                          where {2}  
                      ) tb";

            var paras = new ArrayList();
            var where = " 配送编号=1 ";
            int i = 0;
            if (!string.IsNullOrWhiteSpace(filter.Month))
            {
                where += " and (CONVERT(varchar(7),出库日期,120)=@p0p" + i + " or (CONVERT(varchar(7),出库日期,120)<=@p0p" + i + " and CONVERT(varchar(7),收款单确认时间,120)=@p0p" + i + ")) ";
                paras.Add(filter.Month);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.OrderNo))
            {
                where += "  and (@p0p" + i + " is null or 订单号 =  @p0p" + i + " ) ";
                paras.Add(filter.OrderNo);
                i++;
            }
            var whereWarehouse = filter.WhSelected.Any() ? string.Format(" and 仓库编号 in({0})", string.Join(",", filter.WhSelected)) : "and 仓库编号 in(0)";

            if (filter.WhSelected != null && filter.WhSelected.Any())
            {
                where += whereWarehouse;
            }
            sql = string.Format(sql, filter.Month, filter.Month, where);

            var dataList = Context.Select<RP_非自营销售明细>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<RP_非自营销售明细>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
            };

            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.订单号,tb.ERP编码 desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }

        /// <summary>
        /// 加盟商退换货对账报表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>列表</returns>
        /// <remarks>2014-08-21 余勇 创建</remarks>
        public override Pager<RP_非自营退换货明细> GetFranchiseesRmaDetail(ParaFranchiseesSaleDetail filter)
        {
            var sql = @"(SELECT 订单号,订单来源,订单来源编号,申请日期,入库日期,商城订单号,商城名称,ERP编码,产品名称,数量,单价,
                            优惠,退款金额,实退金额,下单门店,入库仓库,仓库编号,收款方式编号,收款方式,退款方式,配送编号,配送方式,c.ErpCode 加盟商ERP编号,c.ErpName 加盟商ERP名称,
                            售后方式,售后方式编号,结算状态,(case when 收款单状态='已确认' and CONVERT(varchar(7),收款时间,120)>'{0}' then '待确认' else 收款单状态 end) 收款单状态,
                            (case when 收款单状态='已确认' and CONVERT(varchar(7),收款时间,120)>'{1}' then to_date('0001-01-01','yyyy-mm-dd') else 收款时间 end) 收款时间,
                            源单出库仓库,源单出库仓库编号
                          FROM  RP_非自营退换货明细 a 
                          left join DsDealerWharehouse b on a.仓库编号=b.WarehouseSysNo
                          left join DsDealer c on b.dealersysno=c.sysno
                          where {2}  
                      ) tb";

            var paras = new ArrayList();
            var where = " 1=1 ";

            if (!string.IsNullOrWhiteSpace(filter.Month))
            {
                where += " and (CONVERT(varchar(7),申请日期,120) =@Month or (CONVERT(varchar(7),申请日期,120) <=@Month and CONVERT(varchar(7),收款时间,120) =@Month)) ";
                paras.Add(filter.Month);
                paras.Add(filter.Month);
                paras.Add(filter.Month);
            }
            if (!string.IsNullOrWhiteSpace(filter.OrderNo))
            {
                where += "  and (@OrderNo is null or 订单号 =  @OrderNo ) ";
                paras.Add(filter.OrderNo);
                paras.Add(filter.OrderNo);
            }

            var whereWarehouse = filter.WhSelected.Any() ? string.Format(" and 仓库编号 in({0})", string.Join(",", filter.WhSelected)) : "and 仓库编号 in(0)";

            if (filter.WhSelected != null && filter.WhSelected.Any())
            {
                where += whereWarehouse;
            }
            sql = string.Format(sql, filter.Month, filter.Month, where);

            var dataList = Context.Select<RP_非自营退换货明细>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<RP_非自营退换货明细>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
            };

            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.订单号,tb.ERP编码 desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }
        #endregion

        #region 二次销售订单业务员对账报表

        /// <summary>
        /// 获取二次销售报表相关数据
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2014-09-22 朱成果 创建</remarks>
        public override Pager<CBTwoSale> GetTwoSaleList(ParaTwoSaleFilter filter)
        {
            //            string sql = @"(
            //                                select to_char(a.createdate,'yyyy-mm-dd') as CreateDate,c.warehousename as WarehouseName,b.username as UserName,count(a.sysno) as OrderCount,sum(a.cashpay) as OrderCash,a.ordersourcesysno as UserID,a.defaultwarehousesysno as WarehouseSysNo
            //                                from
            //                                SoOrder  a
            //                                inner join SyUser b
            //                                on a.ordersourcesysno=b.sysno
            //                                inner join WhWarehouse c
            //                                on c.sysno=a.defaultwarehousesysno
            //                                where  a.OrderSource=60  and  (a.imgflag='twosale' or a.imgflag='twosalewithadjustprice')  {0}
            //                                group by a.defaultwarehousesysno,to_char(a.createdate,'yyyy-mm-dd'),c.warehousename,a.ordersourcesysno,b.username
            //
            //                           )tb";
            //            string where = " and a.paytypesysno=" + PaymentType.现金预付;
            //             var paras = new ArrayList();
            //            if(filter.CreateTime.HasValue)//创建时间
            //            {
            //                where += " and to_char(a.createdate,'yyyy-mm-dd')=:CreateDate";//新的日期
            //                paras.Add(filter.CreateTime.Value.ToString("yyyy-MM-dd"));
            //            }
            //            if (filter.WarehouseSysNos != null && filter.WarehouseSysNos.Any())//仓库
            //            {
            //                where += string.Format(" and a.defaultwarehousesysno in({0})", string.Join(",", filter.WarehouseSysNos));
            //            }

            string sql = @"
                        (
                               select 
                               CONVERT(varchar(7),a.CreateDate,120) as CreateDate,a.StockName as WarehouseName,
                               a.DeliveryUserName as UserName,count(a.OrderSysNo) as OrderCount,
                               sum(a.OrderAmount) as OrderCash,DeliveryUserSysNo as UserID,StockSysNo as WarehouseSysNo
                               from Rp_业务员二次销售   a 
                               {0}
                               group by a.StockSysNo,CONVERT(varchar(7),a.CreateDate,120),a.StockName,a.DeliveryUserSysNo,a.DeliveryUserName 
                         )tb";
            var paras = new ArrayList();
            string where = " where 1=1";
            if (filter.CreateTime.HasValue)//创建时间
            {
                where += " and CONVERT(varchar(10),a.CreateDate,120)=@p0p0";//新的日期
                paras.Add(filter.CreateTime.Value.ToString("yyyy-MM-dd"));
            }
            if (filter.WarehouseSysNos != null && filter.WarehouseSysNos.Any())//仓库
            {
                where += string.Format(" and a.StockSysNo in({0})", string.Join(",", filter.WarehouseSysNos));
            }

            sql = string.Format(sql, where);
            var dataList = Context.Select<CBTwoSale>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBTwoSale>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
            };

            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.OrderCash desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }
        /// <summary>
        /// 获取二次销售详情
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2014-09-22 朱成果 创建</remarks>
        public override Pager<CBTwoSaleDetail> GetTwoSaleDetailList(ParaTwoSaleFilter filter)
        {
            string sql = @"(
                                select c.createdate as CreateTime,c.sysno as OrderNo,k.username as UserName,
                                g.account as Account,b.erpcode as ErpCode,a.productname as ProductName,
                                a.quantity as Quantity,a.salesunitprice as Price,a.changeamount as Discount,a.salesamount as SaleAmount,
                                a.salesamount+a.changeamount as RealSaleAmount,e.paymentname as PaymentType,lgt.deliverytypename as DeliveryType,d.mobilephonenumber as Moblie,CASE ls.status WHEN 20 THEN '已结算' ELSE '未结算' END as SettlementState,
                                CouponAmount+ProductDiscountAmount+OrderDiscountAmount as CouponAmount	
                                from Rp_业务员二次销售  d
                                inner join  SoOrderItem a  on a.ordersysno=d.ordersysno
                                left join PdProduct b on a.productsysno=b.sysno
                                left join SoOrder c on d.ordersysno=c.sysno
                                left join SoReceiveAddress d on d.sysno=c.receiveaddresssysno
                                left join BsPaymentType e on e.sysno=c.paytypesysno
                                left join SyUser        k on k.sysno=c.ordercreatorsysno
                                left join CrCustomer    g on g.sysno=c.customersysno
                                left outer join WhStockOutItem f on f.orderitemsysno=a.sysno 
                                left outer join WhStockOut h on h.sysno=f.stockoutsysno and h.status<>-10
                                left outer join LgDeliveryType lgt on lgt.sysno=NVL(h.deliverytypesysno,c.deliverytypesysno)
                                left outer join LgDeliveryItem ldi on ldi.NoteType=10 and ldi.NoteSysNo=h.sysno and (ldi.Status<>－10 and ldi.Status<>30) and h.status in(60,90)
                                left outer join LgSettlementItem lsi on lsi.StockOutSysNo=h.sysno and lsi.DeliverySysNo=ldi.DeliverySysNo and lsi.status<>-10
                                left outer join LgSettlement ls on ls.sysno=lsi.settlementsysno
                                where   {0}
                           ) tb";
            string where = " 1=1";
            var paras = new ArrayList();
            int i = 0;
            if (filter.CreateTime.HasValue)//创建时间
            {
                where += " and CONVERT(varchar(10),c.createdate,120)=@p0p" + i;//创建时间
                paras.Add(filter.CreateTime.Value.ToString("yyyy-MM-dd"));
                i++;
            }
            if (filter.UserID.HasValue)//业务员
            {
                where += " and  d.DeliveryUserSysNo=@p0p" + i;
                paras.Add(filter.UserID.Value);
                i++;
            }
            if (filter.SelectWarehouseSysNo.HasValue)//仓库编号
            {
                where += " and  d.StockSysNo=@p0p" + i;
                paras.Add(filter.SelectWarehouseSysNo.Value);
                i++;
            }
            sql = string.Format(sql, where);
            var dataList = Context.Select<CBTwoSaleDetail>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBTwoSaleDetail>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.CreateTime asc,RealSaleAmount desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = totalRows;
            pager.Rows = rows;
            return pager;
        }
        #endregion

        #region 办事处快递发货量统计

        /// <summary>
        /// 办事处快递发货量统计查询
        /// </summary>
        /// <param name="para">CBRptExpressLgDelivery</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="warehouseSysNos">仓库列表</param>
        /// <returns>Dic(totalCount,办事处绩效集合)</returns>
        /// <remarks>2014-09-24 余勇 创建</remarks>
        public override Dictionary<int, List<CBRptExpressLgDelivery>> QueryExpressLgDelivery(CBRptExpressLgDelivery para,
            List<int> warehouseSysNos,
            int currPageIndex = 1,
            int pageSize = 10)
        {
            if (string.IsNullOrEmpty(para.统计日期))
            {
                para.统计日期 = DateTime.Now.Year + "-" + DateTime.Now.Month;
            }
            var sql =
                @"(SELECT t.CompanyName as 快递公司,bso.name as 办事处, CONVERT(varchar(7),CreateDate,120) as 统计日期,count(1) as 总单量
                                from rp_第三方快递发货量 t 
                                inner join (select  min(organizationsysno) organizationsysno ,warehousesysno from BsOrganizationWarehouse group by warehousesysno) bsowh on t.StockSysNo = bsowh.warehousesysno 
                                inner join bsorganization bso on bsowh.organizationsysno = bso.sysno 
                                 where {0}
                                 group by CONVERT(varchar(7),t.CreateDate,120),t.CompanyName,bso.name
                                ) tb
                                ";
            var paras = new ArrayList();
            var where = " 1=1 ";
            if (!string.IsNullOrWhiteSpace(para.统计日期))
            {
                where += "  and (@p0p0 is null or CONVERT(varchar(7),t.CreateDate,120) =  @p0p0) ";
                paras.Add(para.统计日期);
            }
            var whereWarehouse = warehouseSysNos.Any()
                           ? string.Format(" and bso.SysNo in({0})", string.Join(",", warehouseSysNos))
                           : "";

            if (whereWarehouse.Any())
            {
                where += whereWarehouse;
            }
            sql = string.Format(sql, where);
            var dataList = Context.Select<CBRptExpressLgDelivery>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);
            using (var context = Context.UseSharedConnection(true))
            {
                var lstResult = dataList.OrderBy("tb.办事处,tb.快递公司").Paging(currPageIndex, pageSize).QueryMany();
                var count = dataCount.QuerySingle();
                return new Dictionary<int, List<CBRptExpressLgDelivery>> { { count, lstResult } };
            }
        }

        #endregion

        #region 会员涨势统计报表
        /// <summary>
        /// 会员涨势信息
        /// </summary>
        /// <param name="filter">会员涨势信息</param>
        /// <returns>返回会员涨势信息</returns>
        /// <remarks>2016-02-04 王耀发 创建</remarks>
        public override Pager<CBRptDealerSales> GetDealerSalesList(ParaRptDealerSalesFilter filter)
        {
            //计算会员数
            string sqlANums = "select count(*) from CrCustomer where Status=1 and DealerSysNo = d.SysNo";

            //计算会员数
            string sqlCNums = "select count(*) from CrCustomer where Status=1 and DealerSysNo = d.SysNo";
            //计算营业额
            string sqlSAmount = @"select sum(o.OrderAmount) from SoOrder o left join CrCustomer c on o.CustomerSysNo = c.SysNo 
                                   left join 
                                   (
                                    select max(fn.CreatedDate) as CreatedDate,fn.SourceSysNo
                                    from FnOnlinePayment fn 
                                    group by fn.SourceSysNo
                                   ) as fn on fn.SourceSysNo = o.SysNo where o.DealerSysNo = d.SysNo and o.PayStatus = 20 and  c.Status=1 and o.Status>=30 ";

            string sqlAAmount = @"select sum(o.OrderAmount) from SoOrder o left join CrCustomer c on o.CustomerSysNo = c.SysNo 
                                    where o.DealerSysNo = d.SysNo and o.PayStatus = 20 and  c.Status=1 and o.Status>=30 ";

            //会员是否关注服务号
            if (!string.IsNullOrEmpty(filter.Subscribe))
            {
                sqlCNums += " and Subscribe = '" + filter.Subscribe + "'";
                sqlSAmount += " and c.Subscribe = '" + filter.Subscribe + "'";
                sqlAAmount += " and c.Subscribe = '" + filter.Subscribe + "'";
            }

            //开始日期
            if (!string.IsNullOrEmpty(filter.BeginDate))
            {
                sqlCNums += " and CreatedDate >= CONVERT(datetime,'" + filter.BeginDate + "')";
                sqlSAmount += " and c.CreatedDate >= CONVERT(datetime,'" + filter.BeginDate + "')";
                sqlAAmount += " and o.CreateDate >= CONVERT(datetime,'" + filter.BeginDate + "')";
            }
            //结束日期
            if (!string.IsNullOrEmpty(filter.EndDate))
            {
                sqlCNums += " and CreatedDate <= CONVERT(datetime,'" + filter.EndDate + "')";
                sqlSAmount += " and c.CreatedDate <= CONVERT(datetime,'" + filter.EndDate + "')";
                sqlAAmount += " and o.CreateDate <= CONVERT(datetime,'" + filter.EndDate + "')";
            }
            string sql = @"select d.SysNo,d.DealerName, (" + sqlCNums + ") as CustomerNums ,";
            sql += @"isnull((" + sqlSAmount + @"),0) as SumOrderAmount ,
                    ("+sqlANums+@")  as ACustomerNums ,
                    isnull((" + sqlAAmount + @"),0) as AllSumOrderAmount
                    from DsDealer d ";
            string where = " where 1 = 1 ";

            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    where += " and d.SysNo = " + filter.DealerSysNo;
                }
                else
                {
                    where += " and d.CreatedBy = " + filter.DealerCreatedBy;
                }
            }
            if (filter.SelectedAgentSysNo != -1)
            {
                if (filter.SelectedDealerSysNo != -1)
                {
                    where += " and d.SysNo = " + filter.SelectedDealerSysNo;
                }
                else
                {
                    where += " and d.CreatedBy = " + filter.SelectedAgentSysNo;
                }
            }

            sql = "(" + sql + where + ") tb";

            var dataList = Context.Select<CBRptDealerSales>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            //var paras = new object[]
            //{
            //    filter.DealerSysNo,filter.DealerSysNo
            //};
            //dataList.Parameters(paras);
            //dataCount.Parameters(paras);

            var pager = new Pager<CBRptDealerSales>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.SumOrderAmount desc,tb.CustomerNums ,tb.AllSumOrderAmount,tb.ACustomerNums desc ").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }
        /// <summary>
        /// 查询导出会员涨势信息列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public override List<CBOutputRptDealerSales> GetExportDealerSalesList(ParaRptDealerSalesFilter filter, List<int> sysNos)
        {
            //计算会员数
            string sqlCNums = "select count(*) from CrCustomer where Status=1 and DealerSysNo = d.SysNo";
            //计算营业额
            string sqlSAmount = @"select sum(o.OrderAmount) from SoOrder o left join CrCustomer c on o.CustomerSysNo = c.SysNo left join 
                                   (
                                    select max(fn.CreatedDate) as CreatedDate,fn.SourceSysNo
                                    from FnOnlinePayment fn 
                                    group by fn.SourceSysNo
                                   ) as fn on fn.SourceSysNo = o.SysNo where o.DealerSysNo = d.SysNo and o.PayStatus = 20 ";

            //会员是否关注服务号
            if (!string.IsNullOrEmpty(filter.Subscribe))
            {
                sqlCNums += " and Subscribe = '" + filter.Subscribe + "'";
                sqlSAmount += " and c.Subscribe = '" + filter.Subscribe + "'";
            }

            //开始日期
            if (!string.IsNullOrEmpty(filter.BeginDate))
            {
                sqlCNums += " and CreatedDate >= CONVERT(datetime,'" + filter.BeginDate + "')";
                sqlSAmount += " and fn.CreatedDate >= CONVERT(datetime,'" + filter.BeginDate + "')";
            }
            //结束日期
            if (!string.IsNullOrEmpty(filter.EndDate))
            {
                sqlCNums += " and CreatedDate <= CONVERT(datetime,'" + filter.EndDate + "')";
                sqlSAmount += " and fn.CreatedDate <= CONVERT(datetime,'" + filter.EndDate + "')";
            }

            string sqlText = @"select d.SysNo,d.DealerName, (" + sqlCNums + ") as CustomerNums ,";
            sqlText += "isnull((" + sqlSAmount + "),0) as SumOrderAmount from DsDealer d ";
            string where = " where 1 = 1 ";

            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    where += " and d.SysNo = " + filter.DealerSysNo;
                }
                else
                {
                    where += " and d.CreatedBy = " + filter.DealerCreatedBy;
                }
            }
            if (filter.SelectedAgentSysNo != -1)
            {
                if (filter.SelectedDealerSysNo != -1)
                {
                    where += " and d.SysNo = " + filter.SelectedDealerSysNo;
                }
                else
                {
                    where += " and d.CreatedBy = " + filter.SelectedAgentSysNo;
                }
            }

            sqlText = "select * from (" + sqlText + where + ") tb";

            if (sysNos.Count > 0)
            {
                sqlText += " where tb.SysNo in (" + string.Join(",", sysNos) + ") ";
            }

            sqlText += " order by tb.SumOrderAmount desc,tb.CustomerNums desc ";

            List<CBOutputRptDealerSales> outDealerSales = Context.Sql(sqlText).QueryMany<CBOutputRptDealerSales>();

            return outDealerSales;
        }
        #endregion


        #region 出库单报表
        /// <summary>
        /// 获取出库单列表
        /// </summary>
        /// <param name="condition">出库单查询条件</param>
        /// <returns></returns>
        public override DataTable GetStockOutList(StockOutSearchCondition condition)
        {          
            #region 构建条件

            var parms = new List<string>();
            var where = " 1=1";
            int i = 0;
            if (condition.StartDate != null)
            {
                where += " and skt.CreatedDate>=@" + i;
                parms.Add(condition.StartDate.ToString());
                i++;
            }
            if (condition.EndDate != null)
            {
                where += " and skt.CreatedDate<@" + i;
                parms.Add(condition.EndDate.ToString());
                i++;
            }

            if (condition.StartStockOutDate != null)
            {
                where += " and skt.StockOutDate>=@" + i;
                parms.Add(condition.StartStockOutDate.ToString());
                i++;
            }
            if (condition.EndStockOutDate != null)
            {
                where += " and skt.StockOutDate<=@" + i;
                parms.Add(condition.EndStockOutDate.ToString());
                i++;
            }


            if (condition.StockOutSysNo != null)
            {
                where += " and skt.SysNo=@" + i;
                parms.Add(condition.StockOutSysNo.ToString());
                i++;
            }
            if (condition.WarehouseSysNo != null)
            {
                where += " and skt.WarehouseSysNo=@" + i;
                parms.Add(condition.WarehouseSysNo.ToString());
                i++;
            }
            if (condition.SoSysNo != null)
            {
                where += " and skt.ordersysno=@" + i;
                parms.Add(condition.SoSysNo.ToString());
                i++;
            }
            if (condition.AreaSysNo != null)
            {
                where += " and skt.RECEIVEADDRESSSYSNO=@" + i;
                parms.Add(condition.AreaSysNo.ToString());
                i++;
            }
            if (condition.DeliveryTypeSysNo != null)
            {
                where += " and skt.DeliveryTypeSysNo=@" + i;
                parms.Add(condition.DeliveryTypeSysNo.ToString());
                i++;
            }
            if (condition.InvoiceSysNo.HasValue && condition.InvoiceSysNo.Value == 1)
            {
                where += " and skt.InvoiceSysNo > 0";
            }
            else if (condition.InvoiceSysNo.HasValue && condition.InvoiceSysNo.Value == 0)
            {
                where += " and (skt.InvoiceSysNo = 0 or skt.InvoiceSysNo is null)";
            }
            if (condition.Status != null)
            {
                where += " and skt.Status=@" + i;
                parms.Add(condition.Status.ToString());
                i++;
            }
            if (!string.IsNullOrWhiteSpace(condition.TransactionSysNo))
            {
                where += " and skt.TransactionSysNo=@" + i;
                parms.Add(condition.TransactionSysNo);
                i++;
            }
            //where += " and a.deliverytypesysno != {0} and  a.deliverytypesysno != {1}";

        
            if (condition.ProductSysNo != null)
            {
                where += " and ski.ProductSysNo= @" + i;
                parms.Add(condition.ProductSysNo.ToString());
                i++;
            }
            //20170821新增商品erp查询和名称查询 罗勤尧
            if (!string.IsNullOrWhiteSpace(condition.ProductErpCode))
            {
                where += " and pdt.ErpCode=@" + i;
                parms.Add(condition.ProductErpCode);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(condition.ProductName))
            {
                where += " and pdt.ProductName like  '%" + condition.ProductName + "%' ";
                //parms.Add(condition.ProductName);
                //i++;
            }
            if (!string.IsNullOrWhiteSpace(condition.ExpressNo))
            {
                where += " and exists(select 1 from LgDeliveryItem lg where lg.notetype={2} and lg.notesysno=skt.sysno and lg.expressno = @" + i + ")";
                parms.Add(condition.ExpressNo);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(condition.ThirdPartyOrder))
            {
                where += " and a.ordersysno = (select so.sysno from soorder so where so.transactionsysno = (select ds.ordertransactionsysno from dsorder ds where ds.mallorderid = @" + i + "))";
                parms.Add(condition.ThirdPartyOrder);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(condition.ReceiveName))
            {
                where += " and rad.name = @" + i;
                parms.Add(condition.ReceiveName);
                i++;
            }
            if (condition.DsDealerMallSysNo != null)
            {
                where += " and exists(select 1 from soorder so where so.ordersource = {3} and so.sysno = skt.ordersysno and so.ordersourcesysno = @" + i + ")";
                parms.Add(condition.DsDealerMallSysNo.ToString());
                i++;
            }
            if (!string.IsNullOrWhiteSpace(condition.CustomerAccount))
            {
                where += " and exists(select 1 from soorder so inner join crcustomer cus on cus.sysno = so.customersysno where skt.ordersysno = so.sysno and cus.account = @" + i + ")";
                parms.Add(condition.CustomerAccount);
                i++;
            }

            if (!string.IsNullOrWhiteSpace(condition.OrderSysNoList))
            {
                where += " and skt.SysNo in ("+condition.OrderSysNoList+")";
            }

            if (condition.AwaitShipStatus != null && condition.AwaitShipStatus > 0)
            {
                where += " and skt.Status in (20,30,40)";
            }
            #endregion
            var arry=parms.ToArray();
            return Context.Sql(@"	    select pdt.ErpCode as '序号',wh.BackWarehouseName as '出货仓', dl.UserName as '代理商',ds.DealerName as '销售部门',
  ds.DealerName as  '电商平台名称',so.CreateDate as '订单日期','' as '发货日期',pdt.EasName as '商品品名',
  pdt.Barcode  as  '商品条形码','' as  '商品单位',ski.ProductQuantity as '申报数量',ski.ProductQuantity as  '实发',

   ( 
    CASE so.OrderSource
	 WHEN 100 
	
	 THEN (select top 1  FromStockOutItemAmount/FromStockOutItemQuantity from SoReturnOrderItem  where OrderItemSysNo=ski.OrderItemSysNo)

     ELSE ski.RealSalesAmount/ski.ProductQuantity  END
 
  ) 

   as  '申报单价'
 , 

  ( 
    CASE so.OrderSource
	 WHEN 100 
	
	 THEN ski.ProductQuantity*(select top 1  FromStockOutItemAmount/FromStockOutItemQuantity from SoReturnOrderItem  where OrderItemSysNo=ski.OrderItemSysNo)

     ELSE ski.RealSalesAmount END
 
  )  as '申报总价'

, so.FreightAmount as '运费',
  '' as '保价费',so.TaxFee as '税款','' as  '毛重（kg）' , '' as  '净重（kg）', pay.PaymentName as '收款账号'
 ,so.SysNo as  '原始订单号','10000'+CONVERT(varchar(100),skt.SysNo) as '出库单号', 
  dte.DeliveryTypeName as '选用的快递公司','' as '快递单号','' as '问题明细',rad.MobilePhoneNumber as '电商客户电话',
  
  rad.IDCardNo as '身份证',rad.Name as '收件人姓名',sf.AreaName as '收件人省份',city.AreaName as '城市'
  
  ,dq.AreaName as '区县',rad.StreetAddress as '详细地址',so.CustomerMessage as '备注', '' as '平台编码',opay.BusinessOrderSysNo as '支付交易号'
  
   from WhStockOutItem  as ski 
  left join [WhStockOut]  as skt on skt.SysNo=ski.StockOutSysNo
  left join WhWarehouse as wh on wh.SysNo=skt.WarehouseSysNo
  left join PdProduct as pdt on  pdt.SysNo=ski.ProductSysNo
  left join SoOrder as so on skt.OrderSysNO=so.SysNo
  left join DsDealer as ds on so.DealerSysNo=ds.SysNo
  left join SyUser as dl on dl.SysNo=ds.CreatedBy
  left join SoOrderItem as soi on ski.OrderItemSysNo=soi.SysNo
  left join BsPaymentType as pay on pay.SysNo=so.PayTypeSysNo
  left join LgDeliveryType as dte on dte.SysNo=skt.DeliveryTypeSysNo
  left join SoReceiveAddress as rad on rad.SysNo=so.ReceiveAddressSysNo
  left join BsArea as dq on dq.SysNo=rad.AreaSysNo 
  left join BsArea as city on city.SysNo=dq.ParentSysNo
  left join BsArea as sf on sf.SysNo=city.ParentSysNo
  left join FnOnlinePayment as opay on opay.SourceSysNo=so.SysNo and opay.Source=10  where " + where)
                .Parameters(arry)                                                                     
                .QuerySingle<DataTable>();
        }
        #endregion
        #region 支付方式统计报表
        /// <summary>
        /// 支付方式信息
        /// </summary>
        /// <param name="filter">支付方式记录信息</param>
        /// <returns>支付方式记录信息</returns>
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
        public override Pager<CBRptPaymentRecord> GetMethodPaymentRecordList(MethodPaymentRecordFilter filter)
        {
            string sql = @"select sum(fn.Amount) as Amount , fn.PaymentTypeSysNo ,i.PaymentName from FnReceiptVoucherItem fn
left join FnReceiptVoucher fr on fr.SysNo=fn.ReceiptVoucherSysNo
left join SoOrder s on s.SysNo=fr.SourceSysNo
 left join bsPaymentType i on fn.PaymentTypeSysNo =i.sysNo ";


            string where = " where 1 = 1 ";

            //开始日期
            if (!string.IsNullOrEmpty(filter.BeginDate))
            {
                where += " and s.CreateDate >= CONVERT(datetime,'" + filter.BeginDate + "')";
            }
            //结束日期
            if (!string.IsNullOrEmpty(filter.EndDate))
            {
                where += " and s.CreateDate <= CONVERT(datetime,'" + filter.EndDate + "')";
            }


            sql = "(" + sql + where + "group by fn.PaymentTypeSysNo,i.PaymentName" + ") tb";

            var dataList = Context.Select<CBRptPaymentRecord>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var pager = new Pager<CBRptPaymentRecord>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.PaymentTypeSysNo desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }

        /// <summary>
        /// 查询导出支付方式信息
        /// </summary>
        /// <param name="filter">支付方式记录信息</param>
        /// <returns></returns>
        ///<remarks>2017-10-10 罗勤瑶 创建</remarks>
        public override List<CBRptPaymentRecord> GetExportMethodPaymentRecordList(MethodPaymentRecordFilter filter)
        {

            string sql = @"select sum(fn.Amount) as Amount , fn.PaymentTypeSysNo ,i.PaymentName from FnReceiptVoucherItem fn
left join FnReceiptVoucher fr on fr.SysNo=fn.ReceiptVoucherSysNo
left join SoOrder s on s.SysNo=fr.SourceSysNo
 left join bsPaymentType i on fn.PaymentTypeSysNo =i.sysNo ";


            string where = " where 1 = 1 ";

            //开始日期
            if (!string.IsNullOrEmpty(filter.BeginDate))
            {
                where += " and s.CreateDate >= CONVERT(datetime,'" + filter.BeginDate + "')";
            }
            //结束日期
            if (!string.IsNullOrEmpty(filter.EndDate))
            {
                where += " and s.CreateDate <= CONVERT(datetime,'" + filter.EndDate + "')";
            }

            sql = "select * , '" + filter.BeginDate + "'+'到'+'" + filter.EndDate + "' as TimeString from (" + sql + where + "group by fn.PaymentTypeSysNo,i.PaymentName" + ") tb";
            sql += " order by tb.PaymentTypeSysNo desc ";

            List<CBRptPaymentRecord> outRebatesRecord = Context.Sql(sql).QueryMany<CBRptPaymentRecord>();

            return outRebatesRecord;
        }

        #endregion

        /// <summary>
        /// 经销商总销售量排名
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns>
        /// 经销商总销量排序列表
        /// </returns>
        /// <remarks>
        /// 2016-04-09 杨云奕 添加
        /// </remarks>
        public override List<Model.Common.ReportMod> DistributorSalesOrderReport(DateTime? startTime, DateTime? endTime, string orderBy)
        {
            string sql = @" select sum(CashPay) as TotalAmount , sum(Quantity) as Quantity ,DealerName as Text from (select sum(SoOrder.CashPay) as CashPay,DsDealer.DealerName,Quantity=(select sum(SoOrderItem.Quantity) from SoOrderItem where SoOrderItem.OrderSysNo=SoOrder.SysNo ) from SoOrder  inner join DsDealer on SoOrder.DealerSysNo=DsDealer.SysNo
                            where SoOrder.PayStatus=20 and SoOrder.Status>=30 and (SoOrder.CreateDate>='" + startTime.Value.ToString("yyyy-MM-dd") + @" 00:00:00' and SoOrder.CreateDate<='" + endTime.Value.ToString("yyyy-MM-dd") + @" 23:59:59')
                            group by DsDealer.DealerName,SoOrder.SysNo) tb
                            group by DealerName ";
            if(string.IsNullOrEmpty(orderBy))
            {
                orderBy = "sum(CashPay)";
            }
            sql += " order by " + orderBy + " DESC ";

            return Context.Sql(sql).QueryMany<Model.Common.ReportMod>();

        }
        /// <summary>
        /// 经销商商品销售排行
        /// </summary>
        /// <param name="DsSysNo">经销商编号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public override List<Model.Common.ReportMod> DistributorSalesProductReport(int DsSysNo, DateTime? startTime, DateTime? endTime, string orderBy)
        {
            string sql = @" select sum(SoOrderItem.SalesAmount+SoOrderItem.ChangeAmount) as TotalAmount,SUM(SoOrderItem.Quantity) as Quantity ,SoOrderItem.ProductName as Text from SoOrder inner join SoOrderItem on SoOrderItem.OrderSysNo=SoOrder.SysNo
                            where  SoOrder.PayStatus=20 and SoOrder.Status>=30 and (SoOrder.CreateDate>='" + startTime.Value.ToString("yyyy-MM-dd") + @" 00:00:00' and SoOrder.CreateDate<='" + endTime.Value.ToString("yyyy-MM-dd") + @" 23:59:59') and SoOrder.DealerSysNo=" + DsSysNo + @"
                            group by ProductName  ";
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = "sum(SoOrderItem.SalesAmount+SoOrderItem.ChangeAmount)";
            }
            else
            {
                switch (orderBy)
                {
                    case "Amount":
                        orderBy = "sum(SoOrderItem.SalesAmount+SoOrderItem.ChangeAmount)";
                        break;
                    case "Quantity":
                        orderBy = "SUM(SoOrderItem.Quantity)";
                        break;
                }
            }

            sql += " order by " + orderBy + " DESC ";
            return Context.Sql(sql).QueryMany<Model.Common.ReportMod>();
        }
        /// <summary>
        /// 单品经销商的销售排名
        /// </summary>
        /// <param name="ProSysNo">商品编号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-04-09 杨云奕 添加
        /// </remarks>
        public override List<Model.Common.ReportMod> SalesProductDistributorReport(int ProSysNo, DateTime? startTime, DateTime? endTime, string orderBy)
        {
            string where = "";
            if(startTime!=null)
            {
                where += " SoOrder.CreateDate>='" + startTime.Value.ToString("yyyy-MM-dd") + @" 00:00:00' ";
            }
            if(endTime!=null)
            {
                if(where!="")
                {
                    where += " and ";
                }
                where += " SoOrder.CreateDate<='" + endTime.Value.ToString("yyyy-MM-dd") + @" 23:59:59' ";
            }

            where = where == "" ? "" : " and (" + where + ")";

            string sql = @" select sum(SoOrderItem.SalesAmount+SoOrderItem.ChangeAmount) as TotalAmount, sum(SoOrderItem.Quantity) as  Quantity ,DsDealer.DealerName as Text
                            from 
                            SoOrder inner join SoOrderItem on SoOrderItem.OrderSysNo=SoOrder.SysNo inner join DsDealer on DsDealer.SysNo=SoOrder.DealerSysNo
                            where SoOrderItem.ProductSysNo=" + ProSysNo + @" and SoOrder.PayStatus=20 and SoOrder.Status>=30 " + where + @"
                            group by DsDealer.DealerName ";
            
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = "sum(SoOrderItem.SalesAmount+SoOrderItem.ChangeAmount)";
            }
            else
            {
                switch (orderBy)
                {
                    case "Amount":
                        orderBy = "sum(SoOrderItem.SalesAmount+SoOrderItem.ChangeAmount)";
                        break;
                    case "Quantity":
                        orderBy = "SUM(SoOrderItem.Quantity)";
                        break;
                }
            }
            sql += " order by " + orderBy + " DESC ";
            return Context.Sql(sql).QueryMany<Model.Common.ReportMod>();
        }


        #region 年度销售统计


        /// <summary>
        /// 获取年度销售统计表集合
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public override List<Model.Manual.AnnualMod> AnnualSalesStatistics(int year, int DsSysNo)
        {
            string DsWhere = "";
            string ShopWhere = "";
            if (DsSysNo > 0)
            {
                DsWhere = " DealerSysNo = " + DsSysNo + " ";
                ShopWhere = " DsPosOrder.DsSysNo = " + DsSysNo + " ";
            }
            else
            {
                DsWhere = " 1=1 ";
                ShopWhere = " 1=1 ";
            }

            List<Model.Manual.AnnualMod> list = new List<Model.Manual.AnnualMod>();
            ///销售数量集合
            string sql = @"select 年份=convert(varchar(4),soorder.CreateDate,112),类型='线上商品数量',
                                   sum(case month(soorder.CreateDate) when 1 then SoOrderItem.RealStockOutQuantity else 0 end) as 一月,
                                   sum(case month(soorder.CreateDate) when 2 then SoOrderItem.RealStockOutQuantity else 0 end) as 二月,
                                   sum(case month(soorder.CreateDate) when 3 then SoOrderItem.RealStockOutQuantity else 0 end) as 三月,
                                   sum(case month(soorder.CreateDate) when 4 then SoOrderItem.RealStockOutQuantity else 0 end) as 四月,
                                   sum(case month(soorder.CreateDate) when 5 then SoOrderItem.RealStockOutQuantity else 0 end) as 五月,
                                   sum(case month(soorder.CreateDate) when 6 then SoOrderItem.RealStockOutQuantity else 0 end) as 六月,
                                   sum(case month(soorder.CreateDate) when 7 then SoOrderItem.RealStockOutQuantity else 0 end) as 七月,
                                   sum(case month(soorder.CreateDate) when 8 then SoOrderItem.RealStockOutQuantity else 0 end) as 八月,
                                   sum(case month(soorder.CreateDate) when 9 then SoOrderItem.RealStockOutQuantity else 0 end) as 九月,
                                   sum(case month(soorder.CreateDate) when 10 then SoOrderItem.RealStockOutQuantity else 0 end) as 十月,
                                   sum(case month(soorder.CreateDate) when 11 then SoOrderItem.RealStockOutQuantity else 0 end) as 十一月,
                                   sum(case month(soorder.CreateDate) when 12 then SoOrderItem.RealStockOutQuantity else 0 end) as 十二月
                            from soorder inner join SoOrderItem on  soorder.sysno=SoOrderItem.OrderSysNo
                            where convert(varchar(4),soorder.CreateDate,112)='" + year + "' and soorder.Status>=30 group by convert(varchar(4),soorder.CreateDate,112)";
            Model.Manual.AnnualMod mod = Context.Sql(sql).QuerySingle<Model.Manual.AnnualMod>();
            list.Add(mod);

            sql = @"select 年份=convert(varchar(4),CreateDate,112),类型='线上销售金额',
                           sum(case month(CreateDate) when 1 then cashpay else 0 end) as 一月,
                           sum(case month(CreateDate) when 2 then cashpay else 0 end) as 二月,
                           sum(case month(CreateDate) when 3 then cashpay else 0 end) as 三月,
                           sum(case month(CreateDate) when 4 then cashpay else 0 end) as 四月,
                           sum(case month(CreateDate) when 5 then cashpay else 0 end) as 五月,
                           sum(case month(CreateDate) when 6 then cashpay else 0 end) as 六月,
                           sum(case month(CreateDate) when 7 then cashpay else 0 end) as 七月,
                           sum(case month(CreateDate) when 8 then cashpay else 0 end) as 八月,
                           sum(case month(CreateDate) when 9 then cashpay else 0 end) as 九月,
                           sum(case month(CreateDate) when 10 then cashpay else 0 end) as 十月,
                           sum(case month(CreateDate) when 11 then cashpay else 0 end) as 十一月,
                           sum(case month(CreateDate) when 12 then cashpay else 0 end) as 十二月
                    from soorder
                    where convert(varchar(4),CreateDate,112)='" + year + "' and Status>=30  and " + DsWhere + " group by convert(varchar(4),CreateDate,112)  ";
            mod = Context.Sql(sql).QuerySingle<Model.Manual.AnnualMod>();
            list.Add(mod);

            ///线下销售
            sql = @"select 年份=convert(varchar(4),DsPosOrder.SaleTime,112),类型='门店商品数量',
                                   sum(case month(DsPosOrder.SaleTime) when 1 then DsPosOrderItem.ProNum else 0 end) as 一月,
                                   sum(case month(DsPosOrder.SaleTime) when 2 then DsPosOrderItem.ProNum else 0 end) as 二月,
                                   sum(case month(DsPosOrder.SaleTime) when 3 then DsPosOrderItem.ProNum else 0 end) as 三月,
                                   sum(case month(DsPosOrder.SaleTime) when 4 then DsPosOrderItem.ProNum else 0 end) as 四月,
                                   sum(case month(DsPosOrder.SaleTime) when 5 then DsPosOrderItem.ProNum else 0 end) as 五月,
                                   sum(case month(DsPosOrder.SaleTime) when 6 then DsPosOrderItem.ProNum else 0 end) as 六月,
                                   sum(case month(DsPosOrder.SaleTime) when 7 then DsPosOrderItem.ProNum else 0 end) as 七月,
                                   sum(case month(DsPosOrder.SaleTime) when 8 then DsPosOrderItem.ProNum else 0 end) as 八月,
                                   sum(case month(DsPosOrder.SaleTime) when 9 then DsPosOrderItem.ProNum else 0 end) as 九月,
                                   sum(case month(DsPosOrder.SaleTime) when 10 then DsPosOrderItem.ProNum else 0 end) as 十月,
                                   sum(case month(DsPosOrder.SaleTime) when 11 then DsPosOrderItem.ProNum else 0 end) as 十一月,
                                   sum(case month(DsPosOrder.SaleTime) when 12 then DsPosOrderItem.ProNum else 0 end) as 十二月
                            from DsPosOrder inner join DsPosOrderItem on  DsPosOrder.sysno=DsPosOrderItem.pSysNo
                            where convert(varchar(4),DsPosOrder.SaleTime,112)='" + year + "' and " + ShopWhere + "  group by convert(varchar(4),DsPosOrder.SaleTime,112)";
            mod = Context.Sql(sql).QuerySingle<Model.Manual.AnnualMod>();
            list.Add(mod);

            sql = @"select 年份=convert(varchar(4),SaleTime,112),类型='门店销售金额',
                           sum(case month(SaleTime) when 1 then TotalSellValue else 0 end) as 一月,
                           sum(case month(SaleTime) when 2 then TotalSellValue else 0 end) as 二月,
                           sum(case month(SaleTime) when 3 then TotalSellValue else 0 end) as 三月,
                           sum(case month(SaleTime) when 4 then TotalSellValue else 0 end) as 四月,
                           sum(case month(SaleTime) when 5 then TotalSellValue else 0 end) as 五月,
                           sum(case month(SaleTime) when 6 then TotalSellValue else 0 end) as 六月,
                           sum(case month(SaleTime) when 7 then TotalSellValue else 0 end) as 七月,
                           sum(case month(SaleTime) when 8 then TotalSellValue else 0 end) as 八月,
                           sum(case month(SaleTime) when 9 then TotalSellValue else 0 end) as 九月,
                           sum(case month(SaleTime) when 10 then TotalSellValue else 0 end) as 十月,
                           sum(case month(SaleTime) when 11 then TotalSellValue else 0 end) as 十一月,
                           sum(case month(SaleTime) when 12 then TotalSellValue else 0 end) as 十二月
                    from DsPosOrder
                     where convert(varchar(4),DsPosOrder.SaleTime,112)='" + year + "' and " + ShopWhere + "  group by convert(varchar(4),DsPosOrder.SaleTime,112)  ";
            mod = Context.Sql(sql).QuerySingle<Model.Manual.AnnualMod>();
            list.Add(mod);

          
            Model.Manual.AnnualMod tempMod = new Model.Manual.AnnualMod();
            tempMod.年份 = year.ToString();
            tempMod.类型 = "合计总数";
            tempMod.一月 = list[0].一月 + list[2].一月;
            tempMod.二月 = list[0].二月 + list[2].二月;
            tempMod.三月 = list[0].三月 + list[2].三月;
            tempMod.四月 = list[0].四月 + list[2].四月;
            tempMod.五月 = list[0].五月 + list[2].五月;
            tempMod.六月 = list[0].六月 + list[2].六月;
            tempMod.七月 = list[0].七月 + list[2].七月;
            tempMod.八月 = list[0].八月 + list[2].八月;
            tempMod.九月 = list[0].九月 + list[2].九月;
            tempMod.十月 = list[0].十月 + list[2].十月;
            tempMod.十一月 = list[0].十一月 + list[2].十一月;
            tempMod.十二月 = list[0].十二月 + list[2].十二月;
            list.Add(tempMod);


            tempMod = new Model.Manual.AnnualMod();
            tempMod.年份 = year.ToString();
            tempMod.类型 = "合计金额";
            tempMod.一月 = list[1].一月 + list[3].一月;
            tempMod.二月 = list[1].二月 + list[3].二月;
            tempMod.三月 = list[1].三月 + list[3].三月;
            tempMod.四月 = list[1].四月 + list[3].四月;
            tempMod.五月 = list[1].五月 + list[3].五月;
            tempMod.六月 = list[1].六月 + list[3].六月;
            tempMod.七月 = list[1].七月 + list[3].七月;
            tempMod.八月 = list[1].八月 + list[3].八月;
            tempMod.九月 = list[1].九月 + list[3].九月;
            tempMod.十月 = list[1].十月 + list[3].十月;
            tempMod.十一月 = list[1].十一月 + list[3].十一月;
            tempMod.十二月 = list[1].十二月 + list[3].十二月;
            list.Add(tempMod);

            return list;
        }
        #endregion
        #region 实体店销售统计
        /// <summary>
        /// 实体店销售统计
        /// </summary>
        /// <param name="defaultWareSysNo">默认库房</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>实体店实体信息</returns>
        public override List<Model.Manual.EntityStatisticMod> SearchEntityShopStatistics(int? defaultWareSysNo, DateTime? startTime, DateTime? endTime)
        {
            string amountWhere = "";
            string quantityWhere = "";
            string secWhere = "";
            if (defaultWareSysNo != null)
            {
                amountWhere += " and b.SysNo = " + defaultWareSysNo.Value;
                secWhere += " and WhWarehouse.SysNo='" + defaultWareSysNo.Value + "'";
            }
            if (startTime != null)
            {
                amountWhere += " and a.SaleTime>='" + startTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                quantityWhere += " and DsPosOrder.SaleTime>='" + startTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
            }
            if (endTime != null)
            {
                amountWhere += " and a.SaleTime<='" + endTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                quantityWhere += " and DsPosOrder.SaleTime<='" + endTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
            }

//            string sql = @"	select b.WarehouseName as EntityName ,
//		                        sum(a.TotalSellValue) as Amount,
//		                        Quantity=(
//		                        select 
//			                        sum(DsPosOrder.TotalNum) 
//		                        from 
//			                        DsPosOrder 
//		                        where
//			                        DsPosOrder.DsSysNo= a.DsSysNo " + quantityWhere + @"
//		                        )
//	                        from DsPosOrder a inner join DsDealerWharehouse d on d.DealerSysNo = a.DsSysNo  inner join WhWarehouse b on  d.WarehouseSysNo=b.SysNo
//	                        where  b.Status=1  " + amountWhere + @"
//	                        group by b.WarehouseName , a.DsSysNo ";
            string sql = @"	select b.BackWarehouseName+'('+( case b.Status when 0 then '停用' else '运行' end )+')' as EntityName ,
		                        sum(a.TotalSellValue) as Amount,
		                        Quantity=(
		                        select 
			                        sum(DsPosOrder.TotalNum) 
		                        from 
			                        DsPosOrder 
		                        where
			                        DsPosOrder.DsSysNo= d.DealerSysNo  " + quantityWhere + @"
		                        )
	                        from DsPosOrder a inner join DsDealerWharehouse d on d.DealerSysNo = a.DsSysNo  inner join WhWarehouse b on  d.WarehouseSysNo=b.SysNo
	                        where  1=1  " + amountWhere + @"
	                        group by b.BackWarehouseName , a.DsSysNo ,d.DealerSysNo,b.Status";

            List<Model.Manual.EntityStatisticMod> entityList = Context.Sql(sql).QueryMany<Hyt.Model.Manual.EntityStatisticMod>();
            //List<Model.Manual.EntityStatisticMod> wareList = Context.Sql(
            //    " 	select b.BackWarehouseName+'('+( case b.Status when 0 then '停用' else '运行' end )+')' as EntityName , '0' as Amount, '0' as Quantity from DsDealerWharehouse d   inner join WhWarehouse b on  d.WarehouseSysNo=b.SysNo where 1=1 " + secWhere
            //    ).QueryMany<Hyt.Model.Manual.EntityStatisticMod>();
            //foreach (Model.Manual.EntityStatisticMod mod in wareList)
            //{
            //    Model.Manual.EntityStatisticMod tempMod = entityList.Find(p => p.EntityName == mod.EntityName);
            //    if (tempMod == null)
            //    {
            //        entityList.Add(mod);
            //    }
            //}

            return entityList;
        }
        #endregion

        #region 客户购买量统计
        /// <summary>
        /// 获取客户购买量统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public override List<Model.Manual.EntityStatisticMod> SearchCustomerPurchasesStatistics(DateTime? startTime, DateTime? endTime, int DsSysNo)
        {
            string DsWhere = "";
            if (DsSysNo > 0)
            {
                DsWhere = " and DealerSysNo = " + DsSysNo + " ";
            }
            else
            {
                DsWhere = " and 1=1 ";
            }

            string amountWhere = "";
            string quantityWhere = "";

            if (startTime != null)
            {
                amountWhere += " and Sorder.CreateDate>='" + startTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                quantityWhere += " and soorder.CreateDate>='" + startTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
            }
            if (endTime != null)
            {
                amountWhere += " and Sorder.CreateDate<='" + endTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                quantityWhere += " and soorder.CreateDate<='" + endTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
            }

            string sql = @"select (b.AreaName) EntityName ,
		                        sum(Sorder.cashpay) as Amount,
		                        Quantity=(
		                        select 
			                        sum(SoOrderItem.Quantity) 
		                        from 
			                        soorder inner join SoOrderItem on  soorder.sysno=SoOrderItem.OrderSysNo 
			                        inner join  SoReceiveAddress on SoReceiveAddress.SysNo=soorder.ReceiveAddressSysNo
			                        inner join BsArea xa on SoReceiveAddress.AreaSysNo=xa.SysNo
			                        inner join BsArea xb on xa.ParentSysNo=xb.SysNo 
			                        inner join BsArea xc on xb.ParentSysNo=xc.SysNo
			                        inner join BsArea xd on xc.ParentSysNo=xd.SysNo
		                        where
			                        xa.ParentSysNo= b.SysNo  and soorder.Status>=30 " + DsWhere + " " + quantityWhere + @"
		                        )
                        from 
                        SoReceiveAddress addr inner join soorder Sorder on addr.SysNo=Sorder.ReceiveAddressSysNo
                        inner join BsArea a on addr.AreaSysNo=a.SysNo
                        inner join BsArea b on a.ParentSysNo=b.SysNo 
                        inner join BsArea c on b.ParentSysNo=c.SysNo
                        inner join BsArea d on c.ParentSysNo=d.SysNo
                        where Sorder.Status>=30 " + DsWhere + "  " + amountWhere + @"
                        group by (b.AreaName),b.SysNo";
            return Context.Sql(sql).QueryMany<Hyt.Model.Manual.EntityStatisticMod>();
        }
        #endregion
        #region 分销商统计表
        public override List<Model.Manual.EntityStatisticMod> SearchDistributorsStatistics(DateTime? startTime, DateTime? endTime, int DsSysNo)
        {
            string DsWhere = "";
            if (DsSysNo > 0)
            {
                DsWhere = " and DealerSysNo = " + DsSysNo + " ";
            }
            else
            {
                DsWhere = " and 1=1 ";
            }

            string amountWhere = "";
            string quantityWhere = "";

            if (startTime != null)
            {
                amountWhere += " and Sorder.CreateDate>='" + startTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                quantityWhere += " and soorder.CreateDate>='" + startTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
            }
            if (endTime != null)
            {
                amountWhere += " and Sorder.CreateDate<='" + endTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                quantityWhere += " and soorder.CreateDate<='" + endTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
            }
            string sql = @"select (b.AreaName) EntityName ,
		                            sum(Sorder.cashpay) as Amount,
		                            Quantity=(
		                            select 
			                            sum(SoOrderItem.Quantity) 
		                            from 
			                            soorder inner join SoOrderItem on  soorder.sysno=SoOrderItem.OrderSysNo 
			                            left join  SoReceiveAddress on SoReceiveAddress.SysNo=soorder.ReceiveAddressSysNo
			                            left join BsArea xa on SoReceiveAddress.AreaSysNo=xa.SysNo
			                            left join BsArea xb on xa.ParentSysNo=xb.SysNo 
			                            left join BsArea xc on xb.ParentSysNo=xc.SysNo
			                            left join BsArea xd on xc.ParentSysNo=xd.SysNo
		                            where
			                            (((xa.ParentSysNo= b.SysNo ) and soorder.Status>=30)) " + DsWhere + " " + quantityWhere + @"
		                            )
                            from 
                            SoReceiveAddress cusSysNo right join soorder Sorder on cusSysNo.SysNo=Sorder.ReceiveAddressSysNo
                            left join BsArea a on cusSysNo.AreaSysNo=a.SysNo
                            left join BsArea b on a.ParentSysNo=b.SysNo 
                            left join BsArea c on b.ParentSysNo=c.SysNo
                            left join BsArea d on c.ParentSysNo=d.SysNo
                            where Sorder.Status>=30  " + DsWhere + " " + amountWhere + @"
                            group by (b.AreaName),b.SysNo
                            ";
            return Context.Sql(sql).QueryMany<Hyt.Model.Manual.EntityStatisticMod>();
        }
        #endregion
        #region 网上购买统计表
        public override List<Model.Manual.EntityStatisticMod> SearchOnlineSalesStatistics(DateTime? startTime, DateTime? endTime, int DsSysNo)
        {
            string DsWhere = "";
            if (DsSysNo > 0)
            {
                DsWhere = " and DealerSysNo = " + DsSysNo + " ";
            }
            else
            {
                DsWhere = " and 1=1 ";
            }

            string amountWhere = "";
            string quantityWhere = "";

            if (startTime != null)
            {
                amountWhere += " and Sorder.CreateDate>='" + startTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                quantityWhere += " and soorder.CreateDate>='" + startTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
            }
            if (endTime != null)
            {
                amountWhere += " and Sorder.CreateDate<='" + endTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                quantityWhere += " and soorder.CreateDate<='" + endTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
            }
            string sql = @"select (b.AreaName) EntityName ,
		                            sum(Sorder.cashpay) as Amount,
		                            Quantity=(
		                            select 
			                             count(*) 
		                            from 
			                            soorder inner join SoOrderItem on  soorder.sysno=SoOrderItem.OrderSysNo 
	                                    inner join whwarehouse on soorder.DefaultWarehouseSysNo=whwarehouse.SysNo
			                            left join  SoReceiveAddress on SoReceiveAddress.SysNo=soorder.ReceiveAddressSysNo
			                            left join BsArea xa on SoReceiveAddress.AreaSysNo=xa.SysNo
			                            left join BsArea xb on xa.ParentSysNo=xb.SysNo 
			                            left join BsArea xc on xb.ParentSysNo=xc.SysNo
			                            left join BsArea xd on xc.ParentSysNo=xd.SysNo
		                            where
			                            (((xa.ParentSysNo= b.SysNo  ) and soorder.Status>=30  )) " + DsWhere + "  " + quantityWhere + @"
		                            )
                            from 
                            SoReceiveAddress addr right join soorder Sorder on addr.SysNo=Sorder.ReceiveAddressSysNo
	                        inner join whwarehouse ware on Sorder.DefaultWarehouseSysNo=ware.SysNo
                            left join BsArea a on addr.AreaSysNo=a.SysNo
                            left join BsArea b on a.ParentSysNo=b.SysNo 
                            left join BsArea c on b.ParentSysNo=c.SysNo
                            left join BsArea d on c.ParentSysNo=d.SysNo
                            where Sorder.Status>=30  " + DsWhere + " " + amountWhere + @"
                            group by (b.AreaName),b.SysNo
                            ";
            return Context.Sql(sql).QueryMany<Hyt.Model.Manual.EntityStatisticMod>();
        }
        #endregion


        #region 保税商品购买统计表
        /// <summary>
        /// 保税商品统计表
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public override List<Model.Manual.EntityStatisticMod> SearchBondedSalesStatistics(DateTime? startTime, DateTime? endTime)
        {
            string amountWhere = "";
            string quantityWhere = "";

            if (startTime != null)
            {
                amountWhere += " and Sorder.CreateDate>='" + startTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                quantityWhere += " and soorder.CreateDate>='" + startTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
            }
            if (endTime != null)
            {
                amountWhere += " and Sorder.CreateDate<='" + endTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                quantityWhere += " and soorder.CreateDate<='" + endTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
            }
            string sql = @"select (b.AreaName) EntityName ,
		                            sum(Sorder.cashpay) as Amount,
		                            Quantity=(
		                            select 
			                            sum(SoOrderItem.Quantity) 
		                            from 
			                            soorder inner join SoOrderItem on  soorder.sysno=SoOrderItem.OrderSysNo 
										inner join whwarehouse on soorder.DefaultWarehouseSysNo=whwarehouse.SysNo
			                            left join  SoReceiveAddress on SoReceiveAddress.SysNo=soorder.ReceiveAddressSysNo
			                            left join BsArea xa on SoReceiveAddress.AreaSysNo=xa.SysNo
			                            left join BsArea xb on xa.ParentSysNo=xb.SysNo 
			                            left join BsArea xc on xb.ParentSysNo=xc.SysNo
			                            left join BsArea xd on xc.ParentSysNo=xd.SysNo
		                            where
			                            (xa.ParentSysNo= b.SysNo and soorder.Status>=30 and  (OrderSource= " +
                                                                                        (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.PC网站 + " or OrderSource=" +
                                                                                        (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.手机商城 + " ) and whwarehouse.WarehouseType='" + (int)Hyt.Model.WorkflowStatus.WarehouseStatus.仓库类型.保税 + @"') " +
                                                                                                                         quantityWhere + @"
		                            )
                            from 
								SoReceiveAddress addr right join soorder Sorder on addr.SysNo=Sorder.ReceiveAddressSysNo
								inner join whwarehouse ware on Sorder.DefaultWarehouseSysNo=ware.SysNo
								left join BsArea a on addr.AreaSysNo=a.SysNo
								left join BsArea b on a.ParentSysNo=b.SysNo 
								left join BsArea c on b.ParentSysNo=c.SysNo
								left join BsArea d on c.ParentSysNo=d.SysNo
							where Sorder.Status>=30 and ware.WarehouseType='" + (int)Hyt.Model.WorkflowStatus.WarehouseStatus.仓库类型.保税 + @"' and  (OrderSource= " +
                                                                         (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.PC网站 +
                                                      " or OrderSource=" +
                                                                         (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.手机商城 + " )" + amountWhere + @"
                            group by (b.AreaName),b.SysNo
                            ";
            return Context.Sql(sql).QueryMany<Hyt.Model.Manual.EntityStatisticMod>();
        }
        #endregion
        #region 精准营销
        /// <summary>
        /// 精准营销同统计报表
        /// </summary>
        /// <param name="proCateSysNos"></param>
        /// <param name="ShopSysNo"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public override List<Model.Manual.EntityStatisticMod> PrecisionMarketReport(string proCateSysNos, int? ShopSysNo, DateTime? startTime, DateTime? endTime)
        {
            string sql = @" select sum(SoOrderItem.Quantity) as Quantity,
                            (case CrCustomer.Name when '系统用户' then '非会员' else CrCustomer.Name end ) as EntityName  , SoOrder.CreateDate  as CreateTime,
                            SoOrderItem.ProductName,CrCustomer.SysNo
                            from SoOrder 
							inner join SoOrderItem on SoOrder.sysNo=SoOrderItem.OrderSysNo
							inner join Pdproduct on SoOrderItem.ProductSysNo=Pdproduct.SysNo
                            inner join WhWarehouse on WhWarehouse.SysNo=SoOrder.DefaultWarehouseSysNo
                            inner join CrCustomer on CrCustomer.SysNo = SoOrder.CustomerSysNo
							inner join PdCategoryAssociation on PdCategoryAssociation.ProductSysNo=SoOrderItem.ProductSysNo
                            where WhWarehouse.WarehouseType=20 and SoOrder.DefaultWarehouseSysNo=" + ShopSysNo + @"
                           ";
            if (startTime != null)
            {
                sql += " and CreateDate >='" + startTime.Value.ToString("yyyy-MM-dd") + " 00:00:00' ";
            }
            if (endTime != null)
            {
                sql += " and CreateDate <='" + endTime.Value.ToString("yyyy-MM-dd") + " 23:59:59' ";
            }
            if (!string.IsNullOrEmpty(proCateSysNos))
            {
                sql += " and CategorySysNo in (" + proCateSysNos + ") ";
            }
            sql += " group by CrCustomer.Name,SoOrder.CreateDate,CrCustomer.SysNo,SoOrderItem.ProductName ";

            return Context.Sql(sql).QueryMany<Hyt.Model.Manual.EntityStatisticMod>();
        }

        #endregion

        #region 返利记录统计报表
        /// <summary>
        /// 会员返利记录信息
        /// </summary>
        /// <param name="filter">返利记录信息</param>
        /// <returns>返利记录信息</returns>
        /// <remarks>2016-05-18 王耀发 创建</remarks>
        public override Pager<CBRptRebatesRecord> GetRebatesRecordList(ParaRptRebatesRecordFilter filter)
        {
            string sql = @"select a.SysNo, so.SysNo as OrderSysNo,so.OrderNo,so.CreateDate as OrderDate,
                            c.SysNo as PurchaserSysNo,
                            c.Account as PurchaserAccount,
                            c.Name as PurchaserName ,
                            b.SysNo as RebatesSysNo,
                            b.Account as RebatesAccount,
                            b.Name as RebatesName,
                            (select sum(Catle) from SoOrderItem where OrderSysNo = so.SysNo) as OrderCatle,a.Rebates as RebatesAmount,
                            convert(decimal(18,2),(select sum(Catle) * so.OperatFee/1000 from SoOrderItem where OrderSysNo = so.SysNo)) as OperatFee,
                            a.Rebates as ARebatesAmount,so.ProductAmount ,
                            so.FreightAmount,so.OrderAmount,case a.[Status] when 0 then '未返利' when 1 then '已返利' when 2 then '失败' end as [Status], '会员购物返利' as RebatesType, rd.LevelSysNo,
                            case a.Genre when 1 then '会员一级' when 2 then '会员二级' when 3 then '会员三级' end as Genre
                            from CrCustomerRebatesRecord a left join CrCustomer b on a.RecommendSysNo = b.SysNo
                            left join CrCustomer c on a.ComplySysNo = c.SysNo 
                            left join DsDealer rd on b.DealerSysNo = rd.SysNo 
                            inner join SoOrder so on a.OrderSysNo = so.SysNo
                            left join CrCustomer cc on so.CustomerSysNo = cc.SysNo	";


            string where = " where 1 = 1 ";

            //开始日期
            if (!string.IsNullOrEmpty(filter.BeginDate))
            {
                where += " and a.RebatesTime >= CONVERT(datetime,'" + filter.BeginDate + "')";
            }
            //结束日期
            if (!string.IsNullOrEmpty(filter.EndDate))
            {
                where += " and a.RebatesTime <= CONVERT(datetime,'" + filter.EndDate + "')";
            }

            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    where += " and rd.SysNo = " + filter.DealerSysNo;
                }
                else
                {
                    where += " and rd.CreatedBy = " + filter.DealerCreatedBy;
                }
            }
            if (filter.SelectedAgentSysNo != -1)
            {
                if (filter.SelectedDealerSysNo != -1)
                {
                    where += " and rd.SysNo = " + filter.SelectedDealerSysNo;
                }
                else
                {
                    where += " and rd.CreatedBy = " + filter.SelectedAgentSysNo;
                }
            }
            if (!string.IsNullOrEmpty(filter.Condition))
            {
                where += " and (charindex(b.Account,'" + filter.Condition + "')>0 or charindex(b.Name,'" + filter.Condition + "')>0 or convert(nvarchar(50),so.SysNo) ='" + filter.Condition + "')";
            }

            sql = "(" + sql + where + ") tb";

            var dataList = Context.Select<CBRptRebatesRecord>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var pager = new Pager<CBRptRebatesRecord>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.SysNo desc ").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }
        /// <summary>
        /// 查询导出会员返利列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public override List<CBOutputRptRebatesRecord> GetExportRebatesRecordList(ParaRptRebatesRecordFilter filter, List<int> sysNos)
        {
            string sql = @"select a.SysNo, so.SysNo as OrderSysNo,so.OrderNo,so.CreateDate as OrderDate,cc.SysNo as PurchaserSysNo,cc.Account as PurchaserAccount,b.SysNo as RebatesSysNo,b.Account as RebatesAccount,b.Name as RebatesName,
                            (select sum(Catle) from SoOrderItem where OrderSysNo = so.SysNo) as OrderCatle,a.Rebates as RebatesAmount,
                            convert(decimal(18,2),(select sum(Catle) * so.OperatFee/1000 from SoOrderItem where OrderSysNo = so.SysNo)) OperatFee,
                            a.Rebates as ARebatesAmount,so.ProductAmount ,
                            so.FreightAmount,so.OrderAmount,case a.[Status] when 0 then '未返利' when 1 then '已返利' when 2 then '失败' end as [Status], '会员购物返利' as RebatesType, rd.LevelSysNo,
                            case a.Genre when 1 then '会员一级' when 2 then '会员二级' when 3 then '会员三级' end as Genre
                            from CrCustomerRebatesRecord a left join CrCustomer b on a.RecommendSysNo = b.SysNo
                            left join CrCustomer c on a.ComplySysNo = c.SysNo 
                            left join DsDealer rd on b.DealerSysNo = rd.SysNo 
                            inner join SoOrder so on a.OrderSysNo = so.SysNo
                            left join CrCustomer cc on so.CustomerSysNo = cc.SysNo	";


            string where = " where 1 = 1 ";

            //开始日期
            if (!string.IsNullOrEmpty(filter.BeginDate))
            {
                where += " and a.RebatesTime >= CONVERT(datetime,'" + filter.BeginDate + "')";
            }
            //结束日期
            if (!string.IsNullOrEmpty(filter.EndDate))
            {
                where += " and a.RebatesTime <= CONVERT(datetime,'" + filter.EndDate + "')";
            }

            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    where += " and rd.SysNo = " + filter.DealerSysNo;
                }
                else
                {
                    where += " and rd.CreatedBy = " + filter.DealerCreatedBy;
                }
            }
            if (filter.SelectedAgentSysNo != -1)
            {
                if (filter.SelectedDealerSysNo != -1)
                {
                    where += " and rd.SysNo = " + filter.SelectedDealerSysNo;
                }
                else
                {
                    where += " and rd.CreatedBy = " + filter.SelectedAgentSysNo;
                }
            }
            if (!string.IsNullOrEmpty(filter.Condition))
            {
                where += " and (b.Account like '%" + filter.Condition + "%'  or b.Name like '%" + filter.Condition + "%' or c.Account like '%" + filter.Condition + "%'  or c.Name like '%" + filter.Condition + "%' or convert(nvarchar(50),so.SysNo) ='" + filter.Condition + "')";
            }

            sql = "select * from (" + sql + where + ") tb";

            if (sysNos.Count > 0)
            {
                sql += " where tb.SysNo in (" + string.Join(",", sysNos) + ") ";
            }

            sql += " order by tb.SysNo desc ";

            List<CBOutputRptRebatesRecord> outRebatesRecord = Context.Sql(sql).QueryMany<CBOutputRptRebatesRecord>();

            return outRebatesRecord;
        }
        
        /// <summary>
        /// 分销商返利记录信息
        /// </summary>
        /// <param name="filter">返利记录信息</param>
        /// <returns>返利记录信息</returns>
        /// <remarks>2016-05-18 王耀发 创建</remarks>
        public override Pager<CBRptRebatesRecord> GetDealerRebatesRecordList(ParaRptRebatesRecordFilter filter)
        {
            string sql = @"select it.SysNo, so.SysNo as OrderSysNo,so.OrderNo,so.CreateDate as OrderDate,c.SysNo as PurchaserSysNo,c.Account as PurchaserAccount,ds.SysNo as RebatesSysNo,ds.DealerName as RebatesAccount,ds.DealerName as RebatesName,
                            (select sum(Catle) from SoOrderItem where OrderSysNo = so.SysNo) as OrderCatle,it.Increased as RebatesAmount,
                            convert(decimal(18,2),(select sum(Catle) * so.OperatFee/1000 from SoOrderItem where OrderSysNo = so.SysNo)) OperatFee,
                            it.Increased as ARebatesAmount,so.ProductAmount,
                            so.FreightAmount,so.OrderAmount,case it.[Status] when 10 then '未返利' when 20 then '已返利' when -10 then '失败' end as [Status], '会员购物返利' as RebatesType, ds.LevelSysNo ,'分销商' as Genre
                            from DsPrePaymentItem it 
                            left join DsPrePayment t on t.SysNo =  it.PrePaymentSysNo
                            left join DsDealer ds on ds.SysNo = t.DealerSysNo	
                            inner join SoOrder so on it.SourceSysNo = so.SysNo
                            left join CrCustomer c on so.CustomerSysNo = c.SysNo ";


            string where = " where it.[Source] = 60 ";

            //开始日期
            if (!string.IsNullOrEmpty(filter.BeginDate))
            {
                where += " and it.CreatedDate >= CONVERT(datetime,'" + filter.BeginDate + "')";
            }
            //结束日期
            if (!string.IsNullOrEmpty(filter.EndDate))
            {
                where += " and it.CreatedDate <= CONVERT(datetime,'" + filter.EndDate + "')";
            }

            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    where += " and ds.SysNo = " + filter.DealerSysNo;
                }
                else
                {
                    where += " and ds.CreatedBy = " + filter.DealerCreatedBy;
                }
            }
            if (filter.SelectedAgentSysNo != -1)
            {
                if (filter.SelectedDealerSysNo != -1)
                {
                    where += " and ds.SysNo = " + filter.SelectedDealerSysNo;
                }
                else
                {
                    where += " and ds.CreatedBy = " + filter.SelectedAgentSysNo;
                }
            }

            if (!string.IsNullOrEmpty(filter.Condition))
            {
                where += " and (charindex(ds.DealerName,'" + filter.Condition + "')>0 or charindex(ds.DealerName,'" + filter.Condition + "')>0 or convert(nvarchar(50),it.SourceSysNo) ='" + filter.Condition + "')";
            }

            sql = "(" + sql + where + ") tb";

            var dataList = Context.Select<CBRptRebatesRecord>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var pager = new Pager<CBRptRebatesRecord>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.SysNo desc ").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }
        /// <summary>
        /// 查询导出分销商返利记录
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public override List<CBOutputRptRebatesRecord> GetExportDealerRebatesRecordList(ParaRptRebatesRecordFilter filter, List<int> sysNos)
        {
            string sql = @"select it.SysNo, so.SysNo as OrderSysNo,so.OrderNo,so.CreateDate as OrderDate,c.SysNo as PurchaserSysNo,c.Account as PurchaserAccount,ds.SysNo as RebatesSysNo,ds.DealerName as RebatesAccount,ds.DealerName as RebatesName,
                            (select sum(Catle) from SoOrderItem where OrderSysNo = so.SysNo) as OrderCatle,it.Increased as RebatesAmount,
                            convert(decimal(18,2),(select sum(Catle) * so.OperatFee/1000 from SoOrderItem where OrderSysNo = so.SysNo)) OperatFee,
                            it.Increased as ARebatesAmount,so.ProductAmount,
                            so.FreightAmount,so.OrderAmount,case it.[Status] when 10 then '未返利' when 20 then '已返利' when -10 then '失败' end as [Status], '会员购物返利' as RebatesType, ds.LevelSysNo ,'分销商' as Genre
                            from DsPrePaymentItem it 
                            left join DsPrePayment t on t.SysNo =  it.PrePaymentSysNo
                            left join DsDealer ds on ds.SysNo = t.DealerSysNo	
                            inner join SoOrder so on it.SourceSysNo = so.SysNo
                            left join CrCustomer c on so.CustomerSysNo = c.SysNo";


            string where = " where it.[Source] = 60 ";

            //开始日期
            if (!string.IsNullOrEmpty(filter.BeginDate))
            {
                where += " and it.CreatedDate >= CONVERT(datetime,'" + filter.BeginDate + "')";
            }
            //结束日期
            if (!string.IsNullOrEmpty(filter.EndDate))
            {
                where += " and it.CreatedDate <= CONVERT(datetime,'" + filter.EndDate + "')";
            }

            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    where += " and ds.SysNo = " + filter.DealerSysNo;
                }
                else
                {
                    where += " and ds.CreatedBy = " + filter.DealerCreatedBy;
                }
            }
            if (filter.SelectedAgentSysNo != -1)
            {
                if (filter.SelectedDealerSysNo != -1)
                {
                    where += " and ds.SysNo = " + filter.SelectedDealerSysNo;
                }
                else
                {
                    where += " and ds.CreatedBy = " + filter.SelectedAgentSysNo;
                }
            }

            if (!string.IsNullOrEmpty(filter.Condition))
            {
                where += " and (charindex(ds.DealerName,'" + filter.Condition + "')>0 or charindex(ds.DealerName,'" + filter.Condition + "')>0 or convert(nvarchar(50),it.SourceSysNo) ='" + filter.Condition + "')";
            }

            sql = "select * from (" + sql + where + ") tb";

            if (sysNos.Count > 0)
            {
                sql += " where tb.SysNo in (" + string.Join(",", sysNos) + ") ";
            }

            sql += " order by tb.SysNo desc ";

            List<CBOutputRptRebatesRecord> outRebatesRecord = Context.Sql(sql).QueryMany<CBOutputRptRebatesRecord>();

            return outRebatesRecord;
        }

        #endregion

        /// <summary>
        /// 同步销售单
        /// </summary>
        /// <returns>王耀发 2016-6-4 创建</returns>
        public override int ProCreateSaleDetail()
        {
            string Sql = "pro_CreateSaleDetail";
            int rowsAffected = Context.Sql(Sql).Execute();
            return rowsAffected;
        }


        /// <summary>
        /// 同步退换货单
        /// </summary>
        /// <returns>吴琨 2017-9-27 创建</returns>
        public override int SynchronousRma()
        {
            string Sql = "pro_RcReturnItem";
            int rowsAffected = Context.Sql(Sql).Execute();
            return rowsAffected;
        }

        public override IList<RptSalesRanking> GetLineOutSaleRanking(ParaRptSalesRankingFilter filter)
        {
            //            var sql = @"(select row_number() over (order by rpt.SalesQuantity desc) rowNumber,rpt.* from (
            //                        select a.商品分类 as productCategoryName,a.商品编号 as ProductSysNo,a.商品名称 as ProductName,
            //                        sum(a.销售数量) as SalesQuantity,sum(a.销售金额) as SalesAmount 
            //                        from rp_销售排行 a where {0}
            //                        group by a.商品分类,a.商品编号,a.商品名称
            //                    ) rpt ) tb";

            //            var paras = new ArrayList();
            //            var where = "1=1 ";
            //            int i = 0;
            //            if (filter.BeginDate != null)
            //            {
            //                where += " and a.统计日期>=@p0p" + i;
            //                paras.Add(filter.BeginDate);
            //                i++;
            //            }
            //            if (filter.EndDate != null)
            //            {
            //                where += " and a.统计日期<@p0p" + i;
            //                paras.Add(filter.EndDate);
            //                i++;
            //            }
            //            if (filter.ProductCategories != null)
            //            {
            //                var categories = string.Join(",", filter.ProductCategories);
            //                where +=
            //                    " and  exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col=a.商品分类编号)";
            //                paras.Add(categories);
            //                i++;
            //            }

            //            sql = string.Format(sql, where);
            //            if (filter.TakingCount > 0)
            //            {
            //                sql += " where rowNumber<=@p0p" + i;
            //                paras.Add(filter.TakingCount);
            //                i++;
            //            }

            //            var dataList = Context.Select<RptSalesRanking>("tb.*").From(sql).Parameters(paras).QueryMany();

            //            return dataList;
            var sql = @" select ROW_NUMBER() over(order by SUM(DsPosOrderItem.ProTotalValue) DESC)  as RowNumber,
                        SUM(DsPosOrderItem.ProNum) as SalesQuantity, (sum(DsPosOrderItem.ProTotalValue)) as SalesAmount, PdProduct.SysNo as ProductSysNo,PdProduct.Barcode,
                        PdProduct.ErpCode,PdProduct.EasName as ProductName,ProductCategoryName=(stuff
                                            ((SELECT   ''+ PdCategory.CategoryName
                                              FROM       PdCategoryAssociation inner join PdCategory on PdCategory.SysNo=PdCategoryAssociation.CategorySysNo
                                              WHERE   PdCategoryAssociation.ProductSysNo = PdProduct.SysNo and  PdCategoryAssociation.IsMaster=1  FOR xml path('')), 1, 0, ''))
                        ,ProductCategorySysNos=(stuff
                                            ((SELECT   ''+ PdCategory.SysNos
                                              FROM    PdCategoryAssociation inner join PdCategory on PdCategory.SysNo=PdCategoryAssociation.CategorySysNo
                                              WHERE   PdCategoryAssociation.ProductSysNo = PdProduct.SysNo and  PdCategoryAssociation.IsMaster=1 FOR xml path('')), 1, 0, ''))
                        from 
                         DsPosOrder inner join  DsPosOrderItem on DsPosOrder.SysNo=DsPosOrderItem.pSysNo  inner join PdProduct on DsPosOrderItem.ProSysNo=PdProduct.SysNo ";

            var groupBy = @" group by PdProduct.ErpCode,PdProduct.EasName, PdProduct.Barcode,PdProduct.SysNo ";
            var paras = new ArrayList();
            var where = "1=1    ";
            int i = 0;
            if (filter.BeginDate != null)
            {
                where += " and DsPosOrder.SaleTime>=@p0p" + i;
                paras.Add(filter.BeginDate);
                i++;
            }
            if (filter.EndDate != null)
            {
                where += " and DsPosOrder.SaleTime<@p0p" + i;
                paras.Add(filter.EndDate);
                i++;
            }
            if (filter.ProductCategories != null)
            {
                var categories = string.Join(",", filter.ProductCategories);
                var getproductSql = " select * from PdCategoryAssociation where CategorySysNo in (" + categories + ") ";
                List<PdCategoryAssociation> list = Context.Sql(getproductSql).QueryMany<PdCategoryAssociation>();
                string tempWhere = "";
                foreach (PdCategoryAssociation mod in list)
                {
                    if (tempWhere != "")
                    {
                        tempWhere += " , ";
                    }
                    tempWhere += mod.ProductSysNo;
                }
                if (string.IsNullOrEmpty(tempWhere))
                {
                    tempWhere = "-1";
                }
                where += " and  PdProduct.SysNo in (" + tempWhere + ") ";
                //where +=
                //    " and  exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col=a.商品分类编号)";
                //paras.Add(categories);
                i++;
            }

            if (where != "")
            {
                sql += " where " + where;
            }
            sql += groupBy + " order by SUM(DsPosOrderItem.ProTotalValue) Desc ";
            //sql = string.Format(sql, where);
            //if (filter.TakingCount > 0)
            //{
            //    sql += " where rowNumber<=@p0p" + i;
            //    paras.Add(filter.TakingCount);
            //    i++;
            //}

            var dataList = Context.Sql(sql, null).Parameters(paras).QueryMany<RptSalesRanking>();

            return dataList;
        }

        public override List<RptSalesRanking> GetLineOutSaleRankingByWarehouse(ParaWarehouseProductSalesFilter filter)
        {
            var sql = @" select ROW_NUMBER() over(order by SUM(DsPosOrderItem.ProTotalValue) DESC)  as RowNumber,
                        SUM(DsPosOrderItem.ProNum) as SalesQuantity, (sum(DsPosOrderItem.ProTotalValue)) as SalesAmount, PdProduct.SysNo as ProductSysNo,PdProduct.Barcode,
                        PdProduct.ErpCode,PdProduct.EasName as ProductName,ProductCategoryName=(stuff
                                            ((SELECT   ''+ PdCategory.CategoryName
                                              FROM       PdCategoryAssociation inner join PdCategory on PdCategory.SysNo=PdCategoryAssociation.CategorySysNo
                                              WHERE   PdCategoryAssociation.ProductSysNo = PdProduct.SysNo and  PdCategoryAssociation.IsMaster=1  FOR xml path('')), 1, 0, ''))
                        ,ProductCategorySysNos=(stuff
                                            ((SELECT   ''+ PdCategory.SysNos
                                              FROM    PdCategoryAssociation inner join PdCategory on PdCategory.SysNo=PdCategoryAssociation.CategorySysNo
                                              WHERE   PdCategoryAssociation.ProductSysNo = PdProduct.SysNo and  PdCategoryAssociation.IsMaster=1 FOR xml path('')), 1, 0, '')),
					        sum(DsPosReturnOrderItem.ReturnNum) as ReturnNum, sum(DsPosReturnOrderItem.ReturnTotalValue) as ReturnTotalValue,PdProductStock.StockQuantity
                        from 
                         DsPosOrder inner join  DsPosOrderItem on DsPosOrder.SysNo=DsPosOrderItem.pSysNo  inner join PdProduct on DsPosOrderItem.ProSysNo=PdProduct.SysNo ";
            sql += " left join DsPosReturnOrder on DsPosReturnOrder.OrderSysNo=DsPosOrder.SysNo left join  DsPosReturnOrderItem on DsPosReturnOrder.SysNo=DsPosReturnOrderItem.pSysNo and DsPosReturnOrderItem.ProSysNo=DsPosOrderItem.ProSysNo";
            sql += "  Inner join DsDealerWharehouse on DsDealerWharehouse.DealerSysNo=DsPosOrder.DsSysNo inner join PdProductStock on PdProductStock.WarehouseSysNo=DsDealerWharehouse.WarehouseSysNo and PdProductStock.PdProductSysNo=DsPosOrderItem.ProSysNo  ";
            var groupBy = @" group by PdProduct.ErpCode,PdProduct.EasName, PdProduct.Barcode,PdProduct.SysNo,PdProductStock.StockQuantity ";
            var paras = new ArrayList();
            var where = "1=1    ";
            int i = 0;
            if (filter.BeginDate != null)
            {
                where += " and DsPosOrder.SaleTime>=@p0p" + i;
                paras.Add(filter.BeginDate);
                i++;
            }
            if (filter.EndDate != null)
            {
                where += " and DsPosOrder.SaleTime<=@p0p" + i;
                filter.EndDate=filter.EndDate.Value.AddHours(23);
                filter.EndDate=filter.EndDate.Value.AddMinutes(59);
                filter.EndDate=filter.EndDate.Value.AddSeconds(59);
                paras.Add(filter.EndDate);
                i++;
            }
            if (!string.IsNullOrEmpty(filter.WhWarehouseIDS))
            {
                where += " and DsDealerWharehouse.WarehouseSysNo in ( " + filter.WhWarehouseIDS + " )";
            }
            if (filter.ProductCategories != null)
            {
                var categories = string.Join(",", filter.ProductCategories);
                var getproductSql = " select * from PdCategoryAssociation where CategorySysNo in (" + categories + ") ";
                List<PdCategoryAssociation> list = Context.Sql(getproductSql).QueryMany<PdCategoryAssociation>();
                string tempWhere = "";
                foreach (PdCategoryAssociation mod in list)
                {
                    if (tempWhere != "")
                    {
                        tempWhere += " , ";
                    }
                    tempWhere += mod.ProductSysNo;
                }
                if (string.IsNullOrEmpty(tempWhere))
                {
                    tempWhere = "-1";
                }
                where += " and  PdProduct.SysNo in (" + tempWhere + ") ";
                //where +=
                //    " and  exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col=a.商品分类编号)";
                //paras.Add(categories);
                i++;
            }

            if (where != "")
            {
                sql += " where " + where;
            }
            sql += groupBy + " order by SUM(DsPosOrderItem.ProTotalValue) Desc ";
            //sql = string.Format(sql, where);
            //if (filter.TakingCount > 0)
            //{
            //    sql += " where rowNumber<=@p0p" + i;
            //    paras.Add(filter.TakingCount);
            //    i++;
            //}

            var dataList = Context.Sql(sql, null).Parameters(paras).QueryMany<RptSalesRanking>();

            return dataList;
        }
    }
}
