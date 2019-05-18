using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Finance;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Finance
{
    /// <summary>
    /// 收款单(应收款)
    /// </summary>
    /// <remarks>2013-07-08 朱成果 创建</remarks>
    public class FnReceiptVoucherDaoImpl : IFnReceiptVoucherDao
    {

        /// <summary>
        /// 添加收款单
        /// </summary>
        /// <param name="entity">收款单实体</param>
        /// <returns>收款单编号</returns>
        /// <remarks>2013-07-08 朱成果 创建</remarks>
        public override int Insert(FnReceiptVoucher entity)
        {
            if (entity.ConfirmedDate == DateTime.MinValue)
            {
                entity.ConfirmedDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            var sysNo = Context.Insert("FnReceiptVoucher", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }

        /// <summary>
        /// 获取收款单
        /// </summary>
        /// <param name="source">收款单来源</param>
        /// <param name="sourceSysNo">收款单编号</param>
        /// <returns>收款单实体</returns>
        /// <remarks>2013-07-08 朱成果 创建</remarks>
        public override FnReceiptVoucher GetEntity(int source, int sourceSysNo)
        {
            return Context.Sql("select * from FnReceiptVoucher where Source=@Source and SourceSysNo=@SourceSysNo")
                        .Parameter("Source", source)
                        .Parameter("SourceSysNo", sourceSysNo)
                        .QuerySingle<FnReceiptVoucher>();
        }

        /// <summary>
        /// 获取收款单详情(不包括明细)
        /// </summary>
        /// <param name="sysNo">付款单编号</param>
        /// <returns>收款单详情</returns>
        /// <remarks>2013-7-22 余勇 创建 </remarks>
        public override CBFnReceiptVoucher GetEntity(int sysNo)
        {
            return Context.Sql("select * from FnReceiptVoucher where SysNo=@SysNo")
                       .Parameter("SysNo", sysNo)
                       .QuerySingle<CBFnReceiptVoucher>();
        }

        /// <summary>
        /// 更新收款单
        /// </summary>
        /// <param name="entity">收款单</param>
        /// <returns></returns>
        /// <remarks>2013-07-08 朱成果 创建</remarks>
        public override void Update(FnReceiptVoucher entity)
        {
            Context.Update("FnReceiptVoucher", entity)
                 .AutoMap(x => x.SysNo)
                 .Where("SysNo", entity.SysNo)
                 .Execute();
        }
        /// <summary>
        /// 删除收款单
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-13 王耀发 创建</remarks>
        public override void DeleteBySource(int SourceSysNo, int Source)
        {
            Context.Sql("delete FnReceiptVoucher where SourceSysNo=@SourceSysNo and Source = @Source ")
                   .Parameter("SourceSysNo", SourceSysNo)
                   .Parameter("Source", Source).Execute();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>查询收款单列表</returns>
        /// <remarks>2013-07-19 朱家宏 创建</remarks>
        /// <remarks>2013-07-24 黄志勇 修改</remarks>
        public override Pager<CBFnReceiptVoucher> GetAll(ParaVoucherFilter filter)
        {
            string where = " 1=1 ";
            var paras = new ArrayList();
            int i = 0;
            if (filter.IncomeType.HasValue)//--收入类型 
            {
                where += " and a.IncomeType=@p0p" + i.ToString();
                paras.Add(filter.IncomeType.Value);
                i++;
            }
            if (filter.Status.HasValue)//--状态
            {
                where += " and a.Status=@p0p" + i.ToString();
                paras.Add(filter.Status.Value);
                i++;
            }
            if (filter.BeginDate.HasValue)//--创建日期(起)
            {
                where += " and a.CreatedDate>=@p0p" + i.ToString();
                paras.Add(filter.BeginDate.Value);
                i++;
            }
            if (filter.EndDate.HasValue)//--创建日期(止) 
            {
                where += " and a.CreatedDate<@p0p" + i.ToString();
                paras.Add(filter.EndDate.Value.AddDays(1));
                i++;
            }
            if (filter.SourceSysNo.HasValue)//--单据来源编号 
            {
                where += " and a.SourceSysNo=@p0p" + i.ToString();
                paras.Add(filter.SourceSysNo.Value);
                i++;
            }
            if (filter.PaymentType.HasValue)//支付类型
            {
                where += " and exists (select 1 from FnReceiptVoucherItem tmp where tmp.ReceiptVoucherSysNo=a.sysno and tmp.PaymentTypeSysNo=@p0p" + i.ToString() + ")";
                paras.Add(filter.PaymentType.Value);
                i++;
            }
            var pager = new Pager<CBFnReceiptVoucher>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id
            };
            string queryfrom = "fnreceiptvoucher a left join SyUser b on a.confirmedby=b.sysno";  //查询Table   
            pager.TotalRows = Context.Select<int>("count(0)") //查询数量
                 .From(queryfrom)
                 .Where(where)
                 .Parameters(paras)
                 .QuerySingle();
            pager.Rows = Context
                .Select<CBFnReceiptVoucher>("a.*,b.username as confirmer")//返回数据列
                .From(queryfrom)
                .Where(where)
                .Parameters(paras)
                .OrderBy("a.SysNo desc")
                .Paging(pager.CurrentPage, filter.PageSize).QueryMany();
            return pager;

            #region 截断方式
            //            const string sql =
            //                   @"(select a.*,b.username as confirmer 
            //                            from fnreceiptvoucher a
            //                            left join SyUser b on a.confirmedby=b.sysno
            //                    where    
            //                            (:IncomeType is null or a.IncomeType=:IncomeType) and           --收入类型 
            //                            (:Status is null or a.Status=:Status) and                       --状态
            //                            (:BeginDate is null or a.CreatedDate>=:BeginDate) and           --创建日期(起)
            //                            (:EndDate is null or a.CreatedDate<:EndDate) and                --创建日期(止) 
            //                            (:SourceSysNo is null or a.SourceSysNo=:SourceSysNo) and        --单据来源编号 
            //                            (:PaymentType is null or exists (select 1 from FnReceiptVoucherItem tmp where tmp.ReceiptVoucherSysNo=a.sysno and tmp.PaymentTypeSysNo=:PaymentType))
            //                    ) tb";

            //            //查询日期上限+1
            //            filter.EndDate = filter.EndDate == null ? (DateTime?)null : filter.EndDate.Value.AddDays(1);

            //            var paras = new object[]
            //                {
            //                    filter.IncomeType,  filter.IncomeType,
            //                    filter.Status,      filter.Status,
            //                    filter.BeginDate,   filter.BeginDate,
            //                    filter.EndDate,     filter.EndDate,
            //                    filter.SourceSysNo, filter.SourceSysNo,
            //                    filter.PaymentType, filter.PaymentType
            //                };

            //            var dataList = Context.Select<CBFnReceiptVoucher>("tb.*").From(sql);
            //            var dataCount = Context.Select<int>("count(0)").From(sql);

            //            dataList.Parameters(paras);
            //            dataCount.Parameters(paras);

            //            var pager = new Pager<CBFnReceiptVoucher>
            //            {
            //                PageSize = filter.PageSize,
            //                CurrentPage = filter.Id
            //            };

            //            pager.TotalRows = dataCount.QuerySingle();
            //            pager.Rows = dataList.OrderBy("tb.SysNo desc").Paging(pager.CurrentPage, filter.PageSize).QueryMany();

            //            return pager;
            #endregion
        }

        /// <summary>
        /// 分页查询预收现金收款单
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="currentUserSysNo">当前登录用户系统编号</param>
        /// <returns>查询预收现金收款单列表</returns>
        /// <remarks>2013-10-14 沈强 修改</remarks>
        public override Pager<CBFnReceiptVoucher> GetFnReceipt(ParaWarehouseFilter filter, int currentUserSysNo)
        {
            string sql = @"(select a.*
                            from fnreceiptvoucher a
                    where    
                            a.IncomeType = {0} and
                            a.source = {1} and
                            {createby}                                                      --所在仓库的配送员 
                            (@0 is null or a.createdby = @0) and    --配送员系统编号
                            (@1 is null or a.Status=@1) and                       --状态
                            (@2 is null or a.CreatedDate>=@2) and           --创建日期(起)
                            (@3 is null or a.CreatedDate<@3) and                --创建日期(止) 
                            (@4 is null or a.SourceSysNo=@4) and        --单据来源编号 
                            exists
                            (
                               select 1 from soorder b where b.sysno = a.Sourcesysno and a.source={1} and b.PayTypeSysNo = {2} and b.OrderSource = {3}
                            )
                    ) tb";
            if (filter.WarehouseSysNo == null)
            {
                #region
                //                sql = @"(select a.*
                //                            from fnreceiptvoucher a
                //                    where    
                //                            a.IncomeType = {0} and
                //                            a.source = {1} and
                //                            a.createdby in(select b.usersysno from SYUSERWAREHOUSE b where b.warehousesysno in(select c.warehousesysno from SYUSERWAREHOUSE c where c.usersysno = {2})) and           --所在仓库的配送员 
                //                            (:DeliverySysNo is null or a.createdby = :DeliverySysNo) and    --配送员系统编号
                //                            (:Status is null or a.Status=:Status) and                       --状态
                //                            (:BeginDate is null or a.CreatedDate>=:BeginDate) and           --创建日期(起)
                //                            (:EndDate is null or a.CreatedDate<:EndDate) and                --创建日期(止) 
                //                            (:SourceSysNo is null or a.SourceSysNo=:SourceSysNo)            --单据来源编号 
                //                    ) tb";
                #endregion
                string tmp = "a.createdby in(select b.usersysno from SYUSERWAREHOUSE b where b.warehousesysno in(select c.warehousesysno from SYUSERWAREHOUSE c where c.usersysno = {4})) and";
                sql = sql.Replace("{createby}", tmp);
                sql = string.Format(sql, (int)FinanceStatus.收款单收入类型.预付, (int)FinanceStatus.收款来源类型.销售单,
                                    Model.SystemPredefined.PaymentType.现金预付, (int)OrderStatus.销售单来源.业务员下单,
                                    currentUserSysNo);

            }
            else
            {
                #region
                //                sql =
                //                    @"(select a.*
                //                            from fnreceiptvoucher a
                //                    where    
                //                            a.IncomeType = {0} and
                //                            a.source = {1} and
                //                            (:WarehouseSysno is null or a.createdby in(select b.usersysno from SYUSERWAREHOUSE b where b.warehousesysno = :WarehouseSysno)) and           --所在仓库的配送员 
                //                            (:DeliverySysNo is null or a.createdby = :DeliverySysNo) and    --配送员系统编号
                //                            (:Status is null or a.Status=:Status) and                       --状态
                //                            (:BeginDate is null or a.CreatedDate>=:BeginDate) and           --创建日期(起)
                //                            (:EndDate is null or a.CreatedDate<:EndDate) and                --创建日期(止) 
                //                            (:SourceSysNo is null or a.SourceSysNo=:SourceSysNo)            --单据来源编号 
                //                    ) tb";
                #endregion

                string tmp = "(@5 is null or a.createdby in(select b.usersysno from SYUSERWAREHOUSE b where b.warehousesysno = @5)) and";
                sql = sql.Replace("{createby}", tmp);
                sql = string.Format(sql, (int)FinanceStatus.收款单收入类型.预付, (int)FinanceStatus.收款来源类型.销售单,
                                    Model.SystemPredefined.PaymentType.现金预付, (int)OrderStatus.销售单来源.业务员下单);
            }

            //查询日期上限+1
            filter.EndDate = filter.EndDate == null ? (DateTime?)null : filter.EndDate.Value.AddDays(1);

            object[] paras = null;
            if (filter.WarehouseSysNo == null)
            {
                paras = new object[]
                    {
                        filter.DeliverySysNo,
                        filter.Status,
                        filter.BeginDate,
                        filter.EndDate,
                        filter.SourceSysNo
                    };
            }
            else
            {
                paras = new object[]
                    {
                        filter.DeliverySysNo,
                        filter.Status,
                        filter.BeginDate,
                        filter.EndDate,
                        filter.SourceSysNo,
                        filter.WarehouseSysNo
                    };
            }


            var dataList = Context.Select<CBFnReceiptVoucher>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBFnReceiptVoucher>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id
            };

            pager.TotalRows = dataCount.QuerySingle();
            pager.Rows = dataList.OrderBy("tb.LastUpdateDate desc").Paging(pager.CurrentPage, filter.PageSize).QueryMany();

            return pager;
        }

        /// <summary>
        /// 获取收款单
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>收款单实体</returns>
        /// <remarks>2013-07-30 黄伟 创建</remarks>
        public override FnReceiptVoucher GetReceiptVoucherByOrder(int orderSysNo)
        {
            return Context.Sql(@"select * from FnReceiptVoucher m where m.source=@source and m.sourcesysno=@sourcesysno")
                          .Parameter("source", (int)FinanceStatus.收款来源类型.销售单)
                          .Parameter("sourcesysno", orderSysNo)
                          .QuerySingle<FnReceiptVoucher>();
        }
    }
}
