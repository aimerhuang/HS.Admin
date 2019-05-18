using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Warehouse
{
    /// <summary>
    /// 出库数据层实现
    /// </summary>
    /// <remarks>
    /// 2013/7/12 何方 创建
    /// </remarks>
    public class OutStockDaoImpl : IOutStockDao
    {
        /// <summary>
        /// 根据过滤条件查询出库单
        /// </summary>
        /// <param name="condition">过滤条件</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-07-04 周唐炬 创建</remarks>
        public override Pager<CBWhStockOut> SearchFilter(StockOutSearchCondition condition, int pageSize)
        {
            string existSql = string.Empty;
            if (!string.IsNullOrWhiteSpace(condition.SysNoFilter))
            {
                existSql = string.Join(",", condition.SysNoFilter);
            }
            var pager = new Pager<CBWhStockOut>();
            const string sqlCount =
                @"select count(1)
                    from whstockout a
                    where 
                        (@0 is null or a.Status = @0)                       
                        and (@1 is null or a.WarehouseSysNo = @1)                       
                        and (@2 is null or a.DeliveryTypeSysNo=@2)
                        and (@3 is null or not exists(
                                    select 1 from splitstr(@3,',') z 
                                                where z.col=a.sysno
                                    ))";

            var paras = new object[]
            {
                condition.Status,
                condition.WarehouseSysNo,
                condition.DeliveryTypeSysNo,
                existSql,existSql
            };

            pager.TotalRows = Context.Sql(sqlCount).Parameters(paras).QuerySingle<int>();

            pager.Rows = Context.Select<CBWhStockOut>(@"a.*, '' as ReceiverName, a.createddate as SoCreateDate")
                                .From(@"whstockout a ")
                                .Where(@"(@0 is null or a.Status = @0)                       
                                        and (@1 is null or a.WarehouseSysNo = @1)                       
                                        and (@2 is null or a.DeliveryTypeSysNo=@2)
                                        and (
                                                @3 is null 
                                                or not exists(select 1 from splitstr(@3,',') z  where z.col=a.sysno)
                                            )")
                                                 .Parameters(paras)
                                                .OrderBy("a.SysNo desc").Paging(condition.CurrentPage, pageSize)
                                                .QueryMany();
            return pager;
        }

        /// <summary>
        /// 获取出库单的详细信息
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <returns>出库单详细信息实体</returns>
        /// <remarks>2013-06-25 周瑜 创建</remarks>
        public override CBWhStockOut GetStockOutInfo(int sysNo)
        {
            const string strSql = @"select a.* from whstockout a where a.sysno = @sysno ";
            CBWhStockOut stockInfo;
            using (IDbContext context = Context.UseSharedConnection(true))
            {
                stockInfo = context.Sql(strSql)
                       .Parameter("sysno", sysNo)
                       .QuerySingle<CBWhStockOut>();
                if (stockInfo != null)
                {
                    stockInfo.Items = context.Sql("select * from WhStockOutItem where StockOutSysNo = @StockOutSysNo and Status=@Status")
                                             .Parameter("StockOutSysNo", stockInfo.SysNo)
                                            .Parameter("Status", (int)Model.WorkflowStatus.WarehouseStatus.出库单明细状态.有效)
                                             .QueryMany<WhStockOutItem>();
                }
            }

            return stockInfo;
        }

        /// <summary>
        /// 根据输入条件查询出库单
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="userSysNos">当前用户系统编号</param>
        /// <param name="isHasAllWarehouse">是否拥有所有仓库</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        /// <remarks>2013-07-03 周唐炬 加入查询条件配送方式DeliveryTypeSysNo</remarks>
        /// <remarks>2013-11-13 沈强 重构代码</remarks>
        /// <remarks>2014-03-11 唐文均 重构代码</remarks>
        ///  //20170821新增商品erp查询和名称查询 罗勤尧
        public override Pager<CBWhStockOut> Search(StockOutSearchCondition condition, int pageIndex, int pageSize, int userSysNos, bool isHasAllWarehouse)
        {
            #region 周瑜之前的代码
            //            //根据产品编号来查询出库单
            //            #region 原始调试Sql
            //            /*
            //            const string strSql = @"select a.*, d.Name as ReceiverName from whstockout a
            //inner join soorder c on a.ordersysno = c.sysno
            //inner join soreceiveaddress d on c.RECEIVEADDRESSSYSNO = d.sysno
            //where a.Status = :Status
            //and ((:CreatedDate is null or a.CreatedDate >= to_date(:CreatedDate,'yyyy-mm-dd hh24:mi:ss'))
            //or (:EndDate is null or a.CreatedDate <= to_date(:EndDate,'yyyy-mm-dd hh24:mi:ss')))
            //and (:StockOutSysNo is null or a.SysNo = :StockOutSysNo)
            //and (:WarehouseSysNo is null or a.WarehouseSysNo = :WarehouseSysNo)
            //and (:SoSysNo is null or c.SysNo = :SoSysNo)
            //and (:InvoiceSysNo is null or c.InvoiceSysNo = :InvoiceSysNo)
            //and (:AreaSysNo is null or a.RECEIVEADDRESSSYSNO = :AreaSysNo)
            //order by a.SysNo";
            //             */
            //            #endregion

            //            //将仓库编号转换为字符串

            //            //var returnList = new Pager<CBWhStockOut>();
            //            var pager = new Pager<CBWhStockOut>();
            //            using (IDbContext context = Context.UseSharedConnection(true))
            //            {
            //                string where = @"
            //(:StartDate is null or a.CreatedDate >= :StartDate ) 
            //and (:EndDate is null or a.CreatedDate < :EndDate )
            //and (:StockOutSysNo is null or a.SysNo = :StockOutSysNo)
            //and (:WarehouseSysNo is null or a.WarehouseSysNo = :WarehouseSysNo)
            //and (:SoSysNo is null or c.SysNo = :SoSysNo)
            //and (:AreaSysNo is null or a.RECEIVEADDRESSSYSNO = :AreaSysNo)
            //and (:DeliveryTypeSysNo is null or a.DeliveryTypeSysNo=:DeliveryTypeSysNo)
            //and ((:InvoiceSysNo is null) or (:InvoiceSysNo=0 and a.InvoiceSysNo  = 0) or (:InvoiceSysNo>0 and a.InvoiceSysNo > 0))
            //and (:Status is null or a.Status  = :Status)
            //and (:TransactionSysNo is null or a.TransactionSysNo = :TransactionSysNo)
            //and (:deliveryTypeSysNos is null or not exists (select 1 from table(splitstr(:deliveryTypeSysNos,',')) tmp where tmp.column_value = a.deliverytypesysno))
            //{warehouse}";
            //                //and exists (select 1 from table(splitstr(:warehouseSysNos,',')) tmp where tmp.column_value = a.WarehouseSysNo)
            //                //select * from whwarehouse

            //                var deliveryTypeSysNoList = new List<int>
            //                {
            //                    DeliveryType.门店自提,
            //                    DeliveryType.自提
            //                };
            //                var deliveryTypeSysNos = string.Join(",", deliveryTypeSysNoList);

            //                object[] parms = null;

            //                if (condition.EndDate != null)
            //                {
            //                    DateTime enddate = DateTime.Parse(condition.EndDate.ToString());
            //                    condition.EndDate = enddate.AddDays(1);
            //                }

            //                if (isHasAllWarehouse)
            //                {
            //                    where = where.Replace("{warehouse}", "");
            //                    parms = new object[]
            //                    {
            //                        condition.StartDate,
            //                        condition.StartDate,
            //                        condition.EndDate,
            //                        condition.EndDate,
            //                        condition.StockOutSysNo,
            //                        condition.StockOutSysNo,
            //                        condition.WarehouseSysNo,
            //                        condition.WarehouseSysNo,
            //                        condition.SoSysNo,
            //                        condition.SoSysNo,
            //                        condition.AreaSysNo,
            //                        condition.AreaSysNo,
            //                        condition.DeliveryTypeSysNo,
            //                        condition.DeliveryTypeSysNo,
            //                        condition.InvoiceSysNo,
            //                        condition.InvoiceSysNo,
            //                        condition.InvoiceSysNo,
            //                        condition.Status,
            //                        condition.Status,
            //                        condition.TransactionSysNo,
            //                        condition.TransactionSysNo,
            //                        deliveryTypeSysNos,
            //                        deliveryTypeSysNos
            //                    };
            //                }
            //                else
            //                {
            //                    where = where.Replace("{warehouse}", "and exists (select 1 from SyUserWarehouse b  where b.WarehouseSysNo = a.WarehouseSysNo and b.UserSysNo = :userSysNos)");
            //                    parms = new object[]
            //                    {
            //                        condition.StartDate,
            //                        condition.StartDate,
            //                        condition.EndDate,
            //                        condition.EndDate,
            //                        condition.StockOutSysNo,
            //                        condition.StockOutSysNo,
            //                        condition.WarehouseSysNo,
            //                        condition.WarehouseSysNo,
            //                        condition.SoSysNo,
            //                        condition.SoSysNo,
            //                        condition.AreaSysNo,
            //                        condition.AreaSysNo,
            //                        condition.DeliveryTypeSysNo,
            //                        condition.DeliveryTypeSysNo,
            //                        condition.InvoiceSysNo,
            //                        condition.InvoiceSysNo,
            //                        condition.InvoiceSysNo,
            //                        condition.Status,
            //                        condition.Status,
            //                        condition.TransactionSysNo,
            //                        condition.TransactionSysNo,
            //                        deliveryTypeSysNos,
            //                        deliveryTypeSysNos,
            //                        userSysNos
            //                    };
            //                }

            //                if (condition.ProductSysNo != null)
            //                {
            //                    var masterSysNoList = context.Sql("select StockOutSysNo from WhStockOutItem where ProductSysNo = :ProductSysNo")
            //                        .Parameter("ProductSysNo", condition.ProductSysNo)
            //                        .QueryMany<int>();
            //                    string sysno = masterSysNoList.Aggregate(string.Empty, (current, i) => current + (i + ","));
            //                    if (sysno.Substring(sysno.Length - 1, 1) == ",")
            //                    {
            //                        sysno = sysno.Remove(sysno.Length - 1, 1);
            //                    }
            //                    where += " and a.SysNo in (" + sysno + ")";
            //                }

            //                pager.TotalRows = context.Sql(@"
            //select count(1) from whstockout a 
            //inner join soorder c on a.ordersysno = c.sysno
            //inner join soreceiveaddress d on a.RECEIVEADDRESSSYSNO = d.sysno where " + where)
            //                                         .Parameters(parms)
            //                                         .QuerySingle<int>();

            //                pager.Rows = context.Select<CBWhStockOut>("a.*, d.Name as ReceiverName,c.CreateDate as SoCreateDate")
            //                                                      .From(@"
            //whstockout a 
            //inner join soorder c on a.ordersysno = c.sysno
            //inner join soreceiveaddress d on a.RECEIVEADDRESSSYSNO = d.sysno")
            //                                                      .AndWhere(where)
            //                                                      .Parameters(parms)
            //                                                      .OrderBy("a.SysNo desc")
            //                                                      .Paging(pageIndex, pageSize)
            //                                                      .QueryMany();
            //                foreach (CBWhStockOut master in pager.Rows)
            //                {
            //                    master.Items = context.Sql("select * from WhStockOutItem where StockOutSysNo = :StockOutSysNo")
            //                                          .Parameter("StockOutSysNo", master.SysNo)
            //                                          .QueryMany<WhStockOutItem>();

            //                }
            //            }
            //            return pager;
            #endregion

            #region 修改前代码 2014-03-11
            /*
            #region 查询sql

            string sqlSelect = @"select * 
                                    from(
                                            select a.*                                      
                                            ,'' as ReceiverName
                                            ,a.CreatedDate as SoCreateDate
                                            ,row_number() over (order by a.SysNo desc) FLUENTDATA_ROWNUMBER 
                                            from whstockout a 
                                            where 
                                                    (:StartDate is null or a.CreatedDate >= :StartDate ) 
                                                and (:EndDate is null or a.CreatedDate < :EndDate )
                                                and (:StockOutSysNo is null or a.SysNo = :StockOutSysNo)
                                                and (:WarehouseSysNo is null or a.WarehouseSysNo = :WarehouseSysNo)
                                                and (:SoSysNo is null or a.ordersysno = :SoSysNo)
                                                and (:AreaSysNo is null or a.RECEIVEADDRESSSYSNO = :AreaSysNo)
                                                and (:DeliveryTypeSysNo is null or a.DeliveryTypeSysNo=:DeliveryTypeSysNo)
                                                and ((:InvoiceSysNo is null) or (:InvoiceSysNo = 0 and a.InvoiceSysNo  = 0) or (:InvoiceSysNo > 0 and a.InvoiceSysNo > 0))
                                                and (:Status is null or a.Status  = :Status)
                                                and (:TransactionSysNo is null or a.TransactionSysNo = :TransactionSysNo)
                                                and a.deliverytypesysno != {2} and  a.deliverytypesysno != {3}
                                                and (:isHasAllWarehouse is null or exists(select 1 from SyUserWarehouse b  where b.WarehouseSysNo = a.WarehouseSysNo and b.UserSysNo = :userSysNos))
                                                and (:ProductSysNo is null or exists(select 1 from WhStockOutItem w where a.sysno = w.StockOutSysNo and w.ProductSysNo = :ProductSysNo))
                                                and (:ExpressNo is null or exists(select 1 from LgDeliveryItem lg where lg.notetype={4} and lg.notesysno=a.sysno and lg.expressno = :ExpressNo))
                                                and (:ThirdPartyOrder is null or a.ordersysno = (select so.sysno from soorder so where so.transactionsysno = (select ds.ordertransactionsysno from dsorder ds where ds.mallorderid = :ThirdPartyOrder)))
                                                and (:ReceiveName is null or exists(select 1 from soorder so inner join soreceiveaddress sor on so.receiveaddresssysno = sor.sysno where a.ordersysno=so.sysno and sor.name = :ReceiveName))
                                                and (:DsDealerMallSysNo is null or exists(select 1 from soorder so where so.ordersource = {5} and so.sysno = a.ordersysno and so.ordersourcesysno = :DsDealerMallSysNo))
                                                and (:CustomerAccount is null or exists(select 1 from soorder so inner join crcustomer cus on cus.sysno = so.customersysno where a.ordersysno = so.sysno and cus.account = :CustomerAccount))
                                    )
                                    where fluentdata_RowNumber between {0} and {1}
                                    order by fluentdata_RowNumber";

            string countSelect = @"select count(1)
                                from whstockout a 
                                where 
                                        (:StartDate is null or a.CreatedDate >= :StartDate ) 
                                    and (:EndDate is null or a.CreatedDate < :EndDate )
                                    and (:StockOutSysNo is null or a.SysNo = :StockOutSysNo)
                                    and (:WarehouseSysNo is null or a.WarehouseSysNo = :WarehouseSysNo)
                                    and (:SoSysNo is null or a.ordersysno = :SoSysNo)
                                    and (:AreaSysNo is null or a.RECEIVEADDRESSSYSNO = :AreaSysNo)
                                    and (:DeliveryTypeSysNo is null or a.DeliveryTypeSysNo=:DeliveryTypeSysNo)
                                    and ((:InvoiceSysNo is null) or (:InvoiceSysNo=0 and a.InvoiceSysNo  = 0) or (:InvoiceSysNo>0 and a.InvoiceSysNo > 0))
                                    and (:Status is null or a.Status  = :Status)
                                    and (:TransactionSysNo is null or a.TransactionSysNo = :TransactionSysNo)
                                    and a.deliverytypesysno != {0} and  a.deliverytypesysno != {1}
                                    and (:isHasAllWarehouse is null or exists (select 1 from SyUserWarehouse b  where b.WarehouseSysNo = a.WarehouseSysNo and b.UserSysNo = :userSysNos))
                                    and (:ProductSysNo is null or exists(select 1 from WhStockOutItem w where a.sysno = w.StockOutSysNo and w.ProductSysNo = :ProductSysNo))
                                    and (:ExpressNo is null or exists(select 1 from LgDeliveryItem lg where lg.notetype={2} and lg.notesysno=a.sysno and lg.expressno = :ExpressNo))
                                    and (:ThirdPartyOrder is null or a.ordersysno = (select so.sysno from soorder so where so.transactionsysno = (select ds.ordertransactionsysno from dsorder ds where ds.mallorderid = :ThirdPartyOrder)))
                                    and (:ReceiveName is null or exists(select 1 from soorder so inner join soreceiveaddress sor on so.receiveaddresssysno = sor.sysno where a.ordersysno=so.sysno and sor.name = :ReceiveName))
                                    and (:DsDealerMallSysNo is null or exists(select 1 from soorder so where so.ordersource = {3} and so.sysno = a.ordersysno and so.ordersourcesysno = :DsDealerMallSysNo))
                                    and (:CustomerAccount is null or exists(select 1 from soorder so inner join crcustomer cus on cus.sysno = so.customersysno where a.ordersysno = so.sysno and cus.account = :CustomerAccount))";
            #endregion

            #region 设置sql默认参数

            int beginNum = pageSize * (pageIndex - 1) + 1;
            int endNum = beginNum + pageSize - 1;

            sqlSelect = string.Format(sqlSelect, beginNum, endNum, DeliveryType.门店自提, DeliveryType.自提,
                                      LogisticsStatus.配送单据类型.出库单.GetHashCode(), OrderStatus.销售单来源.分销商升舱.GetHashCode());
            countSelect = string.Format(countSelect, DeliveryType.门店自提, DeliveryType.自提,
                                        LogisticsStatus.配送单据类型.出库单.GetHashCode(), OrderStatus.销售单来源.分销商升舱.GetHashCode());
            #endregion

            var pager = new Pager<CBWhStockOut>();
            using (IDbContext context = Context.UseSharedConnection(true))
            {
                #region 设置查询条件

                //查询所有仓库，还是只查询有权限的部分仓库
                string hasAllWarehouse = isHasAllWarehouse ? null : "part";

                if (condition.EndDate != null)
                {
                    DateTime enddate = DateTime.Parse(condition.EndDate.ToString());
                    condition.EndDate = enddate.AddDays(1);
                }

                var parms = new object[]
                    {
                        condition.StartDate,
                        condition.StartDate,
                        condition.EndDate,
                        condition.EndDate,
                        condition.StockOutSysNo,
                        condition.StockOutSysNo,
                        condition.WarehouseSysNo,
                        condition.WarehouseSysNo,
                        condition.SoSysNo,
                        condition.SoSysNo,
                        condition.AreaSysNo,
                        condition.AreaSysNo,
                        condition.DeliveryTypeSysNo,
                        condition.DeliveryTypeSysNo,
                        condition.InvoiceSysNo,
                        condition.InvoiceSysNo,
                        condition.InvoiceSysNo,
                        condition.Status,
                        condition.Status,
                        condition.TransactionSysNo,
                        condition.TransactionSysNo,
                        hasAllWarehouse,
                        userSysNos,
                        condition.ProductSysNo,
                        condition.ProductSysNo,
                        condition.ExpressNo,
                        condition.ExpressNo,
                        condition.ThirdPartyOrder,
                        condition.ThirdPartyOrder,
                        condition.ReceiveName,
                        condition.ReceiveName,
                        condition.DsDealerMallSysNo,
                        condition.DsDealerMallSysNo,
                        condition.CustomerAccount,
                        condition.CustomerAccount
                    };

                #endregion

                pager.TotalRows = context.Sql(countSelect).Parameters(parms).QuerySingle<int>();

                pager.Rows = context.Sql(sqlSelect).Parameters(parms).QueryMany<CBWhStockOut>();

            }*/
            #endregion

            string sqlSelect = @"select tb.* 
                                    from(
                                            select a.*                                      
                                            ,'' as ReceiverName
                                            ,a.CreatedDate as SoCreateDate
                                            ,row_number() over (order by a.SysNo desc) FLUENTDATA_ROWNUMBER 
                                            from whstockout a 
                                            where {0}
                                    ) tb
                                    where tb.fluentdata_RowNumber between {1} and {2}
                                    order by tb.fluentdata_RowNumber";

            string countSelect = @"select count(1)
                                from whstockout a 
                                where {0}";

            string sqlSelectCount = @"select tb.* 
                                    from(
                                            select a.*                                      
                                            ,'' as ReceiverName
                                            ,a.CreatedDate as SoCreateDate
                                            ,row_number() over (order by a.SysNo desc) FLUENTDATA_ROWNUMBER 
                                            from whstockout a 
                                            where {0}
                                    ) tb
                                    order by tb.fluentdata_RowNumber";

            #region 构建条件

            var parms = new ArrayList();
            var where = "1=1"; // and a.WarehouseSysNo!=59
            int i = 0;
            if (condition.StartDate != null)
            {
                where += " and a.CreatedDate>=@p0p" + i;
                parms.Add(condition.StartDate);
                i++;
            }
            if (condition.EndDate != null)
            {
                where += " and a.CreatedDate<@p0p" + i;               
                parms.Add(condition.EndDate);
                i++;
            }

            if (condition.StartStockOutDate != null)
            {
                where += " and a.StockOutDate>=@p0p" + i;
                parms.Add(condition.StartStockOutDate);
                i++;
            }
            if (condition.EndStockOutDate != null)
            {
                where += " and a.StockOutDate<@p0p" + i;
                parms.Add(condition.EndStockOutDate);
                i++;
            }

            if (condition.StockOutSysNo != null)
            {
                where += " and a.SysNo=@p0p" + i;
                parms.Add(condition.StockOutSysNo);
                i++;
            }
            if (condition.WarehouseSysNo != null)
            {
                where += " and a.WarehouseSysNo=@p0p" + i;
                parms.Add(condition.WarehouseSysNo);
                i++;
            }
            if (condition.SoSysNo != null)
            {
                where += " and a.ordersysno=@p0p" + i;
                parms.Add(condition.SoSysNo);
                i++;
            }
            if (condition.AreaSysNo != null)
            {
                where += " and a.RECEIVEADDRESSSYSNO=@p0p" + i;
                parms.Add(condition.AreaSysNo);
                i++;
            }
            if (condition.DeliveryTypeSysNo != null)
            {
                where += " and a.DeliveryTypeSysNo=@p0p" + i;
                parms.Add(condition.DeliveryTypeSysNo);
                i++;
            }
            if (condition.InvoiceSysNo.HasValue && condition.InvoiceSysNo.Value == 1)
            {
                where += " and a.InvoiceSysNo > 0";
            }
            else if (condition.InvoiceSysNo.HasValue && condition.InvoiceSysNo.Value == 0)
            {
                where += " and (a.InvoiceSysNo = 0 or a.InvoiceSysNo is null)";
            }
            if (condition.Status != null)
            {
                where += " and a.Status=@p0p" + i;
                parms.Add(condition.Status);
                i++;
            }
            if (condition.TransactionSysNo != null)
            {
                where += " and a.TransactionSysNo=@p0p" + i;
                parms.Add(condition.TransactionSysNo);
                i++;
            }

            if (condition.AwaitShipStatus != null && condition.AwaitShipStatus > 0)
            {
                where += " and a.Status in (20,30,40)";              
            }
            //where += " and a.deliverytypesysno != {0} and  a.deliverytypesysno != {1}";

            //查询所有仓库，还是只查询有权限的部分仓库
            if (!isHasAllWarehouse)
            {
                where += " and exists (select 1 from SyUserWarehouse b  where b.WarehouseSysNo = a.WarehouseSysNo and b.UserSysNo = @p0p" + i + ")";
                parms.Add(userSysNos);
                i++;
            }
            if (condition.ProductSysNo != null)
            {
                where += " and exists(select 1 from WhStockOutItem w where a.sysno = w.StockOutSysNo and w.ProductSysNo = @p0p" + i + ")";
                parms.Add(condition.ProductSysNo);
                i++;
            }
            //20170821新增商品erp查询和名称查询 罗勤尧
            if (condition.ProductErpCode != null)
            {
                where += " and exists(select w.ProductSysNo from WhStockOutItem w where a.sysno = w.StockOutSysNo and w.ProductSysNo in(select p.SysNo from PdProduct p where p.ErpCode=@p0p" + i + "))";
                parms.Add(condition.ProductErpCode);
                i++;
            }
            if (condition.ProductName != null)
            {
                where += " and exists(select 1 from WhStockOutItem w where a.sysno = w.StockOutSysNo and w.ProductName like  '%" + condition.ProductName + "%')";
                //parms.Add(condition.ProductName);
                //i++;
            }
            if (condition.ExpressNo != null)
            {
                where += " and exists(select 1 from LgDeliveryItem lg where lg.notetype={2} and lg.notesysno=a.sysno and lg.expressno = @p0p" + i + ")";
                parms.Add(condition.ExpressNo);
                i++;
            }
            if (condition.ThirdPartyOrder != null)
            {
                where += " and a.ordersysno = (select so.sysno from soorder so where so.transactionsysno = (select ds.ordertransactionsysno from dsorder ds where ds.mallorderid = @p0p" + i + "))";
                parms.Add(condition.ThirdPartyOrder);
                i++;
            }
            if (condition.ReceiveName != null)
            {
                where += " and exists(select 1 from soorder so inner join soreceiveaddress sor on so.receiveaddresssysno = sor.sysno where a.ordersysno=so.sysno and sor.name = @p0p" + i + ")";
                parms.Add(condition.ReceiveName);
                i++;
            }
            if (condition.DsDealerMallSysNo != null)
            {
                where += " and exists(select 1 from soorder so where so.ordersource = {3} and so.sysno = a.ordersysno and so.ordersourcesysno = @p0p" + i + ")";
                parms.Add(condition.DsDealerMallSysNo);
                i++;
            }
            if (condition.CustomerAccount != null)
            {
                where += " and exists(select 1 from soorder so inner join crcustomer cus on cus.sysno = so.customersysno where a.ordersysno = so.sysno and cus.account = @p0p" + i + ")";
                parms.Add(condition.CustomerAccount);
                i++;
            }

            #endregion

            #region 设置sql参数

            int beginNum = pageSize * (pageIndex - 1) + 1;
            int endNum = beginNum + pageSize - 1;

            //where = string.Format(where, DeliveryType.门店自提, DeliveryType.自提, LogisticsStatus.配送单据类型.出库单.GetHashCode(), OrderStatus.销售单来源.分销商升舱.GetHashCode());
            where = string.Format(where, "", "", LogisticsStatus.配送单据类型.出库单.GetHashCode(), OrderStatus.销售单来源.分销商升舱.GetHashCode());

            sqlSelect = string.Format(sqlSelect, where, beginNum, endNum);

            countSelect = string.Format(countSelect, where);

            sqlSelectCount = string.Format(sqlSelectCount, where);

            #endregion

            var pager = new Pager<CBWhStockOut>();
            using (IDbContext context = Context.UseSharedConnection(true))
            {
                pager.TotalRows = context.Sql(countSelect).Parameters(parms).QuerySingle<int>();

                pager.Rows = context.Sql(sqlSelect).Parameters(parms).QueryMany<CBWhStockOut>();

                StockOutSearchConditionList.RList = context.Sql(sqlSelectCount).Parameters(parms).QueryMany<CBWhStockOut>();
            }
            return pager;
        }

        /// <summary>
        /// 根据输入条件查询出库单
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        /// <remarks>2013-07-03 周唐炬 加入查询条件配送方式DeliveryTypeSysNo</remarks>
        public override Pager<CBWhStockOut> Search(StockOutSearchCondition condition, int pageIndex, int pageSize)
        {
            //根据产品编号来查询出库单
            #region 原始调试Sql
            /*
            const string strSql = @"select a.*, d.Name as ReceiverName from whstockout a
inner join soorder c on a.ordersysno = c.sysno
inner join crcustomer d on c.customersysno = d.sysno
where a.Status = :Status
and ((:CreatedDate is null or a.CreatedDate >= to_date(:CreatedDate,'yyyy-mm-dd hh24:mi:ss'))
or (:EndDate is null or a.CreatedDate <= to_date(:EndDate,'yyyy-mm-dd hh24:mi:ss')))
and (:StockOutSysNo is null or a.SysNo = :StockOutSysNo)
and (:WarehouseSysNo is null or a.WarehouseSysNo = :WarehouseSysNo)
and (:SoSysNo is null or c.SysNo = :SoSysNo)
and (:InvoiceSysNo is null or c.InvoiceSysNo = :InvoiceSysNo)
and (:AreaSysNo is null or a.RECEIVEADDRESSSYSNO = :AreaSysNo)
order by a.SysNo";
             */
            #endregion

            //var returnList = new Pager<CBWhStockOut>();
            var pager = new Pager<CBWhStockOut>();
            using (IDbContext context = Context.UseSharedConnection(true))
            {
                string where = @"
(@0 is null or a.CreatedDate >= @0 ) 
and (@1 is null or a.CreatedDate <= @1 )
and (@2 is null or a.SysNo = @2)
and (@3 is null or a.WarehouseSysNo = @3)
and (@4 is null or c.SysNo = @4)
and (@5 is null or a.RECEIVEADDRESSSYSNO = @5)
and (@6 is null or a.DeliveryTypeSysNo=@6)
and ((@7 is null) or (@7=0 and a.InvoiceSysNo  = 0) or (@7>0 and a.InvoiceSysNo > 0))
and (@8 is null or a.Status  = @8)
and (@9 is null or a.TransactionSysNo = @9)
and (@10 is null or not exists (select 1 from splitstr(@10,',') tmp where tmp.col = a.deliverytypesysno))";

                var deliveryTypeSysNoList = new List<int>
                {
                    DeliveryType.门店自提,
                    DeliveryType.自提
                };
                var deliveryTypeSysNos = string.Join(",", deliveryTypeSysNoList);
                var parms = new object[]
                    {
                        condition.StartDate,
                        condition.EndDate,
                        condition.StockOutSysNo,
                        condition.WarehouseSysNo,
                        condition.SoSysNo,
                        condition.AreaSysNo,
                        condition.DeliveryTypeSysNo,
                        condition.InvoiceSysNo,
                        condition.Status,
                        condition.TransactionSysNo,
                        deliveryTypeSysNos
                    };

                if (condition.ProductSysNo != null)
                {
                    var masterSysNoList = context.Sql("select StockOutSysNo from WhStockOutItem where ProductSysNo = @ProductSysNo")
                        .Parameter("ProductSysNo", condition.ProductSysNo)
                        .QueryMany<int>();
                    string sysno = masterSysNoList.Aggregate(string.Empty, (current, i) => current + (i + ","));
                    if (sysno.Substring(sysno.Length - 1, 1) == ",")
                    {
                        sysno = sysno.Remove(sysno.Length - 1, 1);
                    }
                    where += " and a.SysNo in (" + sysno + ")";
                }

                pager.TotalRows = context.Sql(@"
select count(1) from whstockout a 
inner join soorder c on a.ordersysno = c.sysno
inner join crcustomer d on c.customersysno = d.sysno where " + where)
                                         .Parameters(parms)
                                         .QuerySingle<int>();

                pager.Rows = context.Select<CBWhStockOut>("a.*, d.Name as ReceiverName,c.CreateDate as SoCreateDate")
                                                      .From(@"
whstockout a 
inner join soorder c on a.ordersysno = c.sysno
inner join crcustomer d on c.customersysno = d.sysno")
                                                      .AndWhere(where)
                                                      .Parameters(parms)
                                                      .OrderBy("a.SysNo desc")
                                                      .Paging(pageIndex, pageSize)
                                                      .QueryMany();
                foreach (CBWhStockOut master in pager.Rows)
                {
                    master.Items = context.Sql("select * from WhStockOutItem where StockOutSysNo = @StockOutSysNo")
                                          .Parameter("StockOutSysNo", master.SysNo)
                                          .QueryMany<WhStockOutItem>();

                }
            }
            return pager;
        }

        /// <summary>
        /// 根据输入条件查询出库单
        /// </summary>
        /// <param name="status">出库单状态</param>
        /// <param name="isInvoice">是否开票</param>
        /// <param name="deliverySysNo">配送方式系统编号</param>
        /// <param name="sysNo">出库单系统编号</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="userSysNos">当前用户系统编号</param>
        /// <param name="isHasAllWarehouse">是否拥有所有仓库</param>
        /// <returns>发票实体列表</returns>
        /// <remarks>2013-07-10 周瑜 创建</remarks>
        public override Pager<CBWhStockOut> QuickSearch(int? status, int? isInvoice, int? deliverySysNo, string sysNo, int pageIndex, int pageSize, int? warehouseSysNo, int userSysNos, bool isHasAllWarehouse)
        {
            var pager = new Pager<CBWhStockOut>();
            string where;
            object[] parms = null;
            var deliveryTypeSysNoList = new List<int>
                {
                    DeliveryType.门店自提,
                    DeliveryType.自提
                };
            var deliveryTypeSysNos = string.Join(",", deliveryTypeSysNoList);
            if (!string.IsNullOrEmpty(sysNo))
            {
                where = @"(@sysno is null or a.SysNo = @sysno)
and (@deliveryTypeSysNos is null or not exists (select 1 from splitstr(@deliveryTypeSysNos,',') tmp where tmp.col = a.deliverytypesysno))
and (@warehouseSysNo is null or a.warehouseSysNo = @warehouseSysNo)
{warehouse}";
                if (isHasAllWarehouse)
                {
                    where = where.Replace("{warehouse}", "");
                    parms = new object[]
                    {
                        sysNo,
                        sysNo,
                        deliveryTypeSysNos,
                        deliveryTypeSysNos,
                        warehouseSysNo,
                        warehouseSysNo
                    };
                }
                else
                {
                    where = where.Replace("{warehouse}", "and exists (select 1 from SyUserWarehouse b  where b.WarehouseSysNo = a.WarehouseSysNo and b.UserSysNo = @userSysNos)");
                    parms = new object[]
                    {
                        sysNo,
                        sysNo,
                        deliveryTypeSysNos,
                        deliveryTypeSysNos,
                        warehouseSysNo,
                        warehouseSysNo,
                        userSysNos
                    };
                }

            }
            else
            {
                where = @"(@0 is null or a.Status = @0) 
and ((@1 is null) or (@1=0 and a.InvoiceSysNo  = 0) or (@1>0 and a.InvoiceSysNo > 0))
and (@2 is null or a.DeliveryTypeSysNo = @2)
and (@3 is null or not exists (select 1 from splitstr(@3,',') tmp where tmp.col = a.deliverytypesysno))
and (@4 is null or a.warehouseSysNo = @4)
{warehouse}";
                if (isHasAllWarehouse)
                {
                    where = where.Replace("{warehouse}", "");
                    parms = new object[]
                    {
                        status,
                        isInvoice,
                        deliverySysNo,
                        deliveryTypeSysNos,
                        warehouseSysNo
                    };
                }
                else
                {
                    where = where.Replace("{warehouse}", "and exists (select 1 from SyUserWarehouse b  where b.WarehouseSysNo = a.WarehouseSysNo and b.UserSysNo = @5)");
                    parms = new object[]
                    {
                        status,
                        isInvoice,
                        deliverySysNo,
                        deliveryTypeSysNos,
                        warehouseSysNo,
                        userSysNos
                    };
                }

            }

            using (IDbContext context = Context.UseSharedConnection(true))
            {
                pager.TotalRows = context.Sql(@"select count(1) from whstockout a 
inner join soorder c on a.ordersysno = c.sysno
inner join crcustomer d on c.customersysno = d.sysno where " + where)
                                         .Parameters(parms)
                                         .QuerySingle<int>();

                pager.Rows = context.Select<CBWhStockOut>("a.*, d.Name as ReceiverName,c.CreateDate as SoCreateDate")
                                    .From(@"whstockout a 
inner join soorder c on a.ordersysno = c.sysno
inner join crcustomer d on c.customersysno = d.sysno")
                                    .Where(where)
                                    .Parameters(parms)
                                    .OrderBy("a.SysNo desc")
                                    .Paging(pageIndex, pageSize)
                                    .QueryMany();
            }
            return pager;
        }

        /// <summary>
        /// 修改出库单状态
        /// </summary>
        /// <param name="model">用于修改出库单的实体</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public override void Update(CBWhStockOut model)
        {
            Context.Update("WhStockOut", model)
                   .AutoMap(x => x.SysNo, x => x.Items, x => x.ScanedItems, x => x.SoCreateDate, x => x.ReceiverName, x => x.AuditorDate, x => x.CustomerSysNo)
                   .Where("SysNo", model.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 修改出库单
        /// </summary>
        /// <param name="model">用于修改出库单的实体</param>
        /// <returns>返回受影响的行数</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public override int Update(WhStockOut model)
        {
            int rowsAffected = Context.Update("WhStockOut", model)
                   .AutoMap(x => x.SysNo, x => x.Items, x => x.SoOrder)
                   .Where("SysNo", model.SysNo)
                //Where("Stamp", model.Stamp)
                   .Execute();
            return rowsAffected;
        }
        /// <summary>
        /// 修改出库单备注
        /// </summary>
        /// <param name="model">用于修改出库单的实体</param>
        /// <returns>返回受影响的行数</returns>
       /// <remarks>罗勤尧 创建</remarks>
        public override int UpdateRemarks(int SysNo, string Remarks)
        {
            return Context.Sql(
                    "update WhStockOut set Remarks =@Remarks where SysNo = @SysNo")
                    .Parameter("Remarks", Remarks)
                       .Parameter("SysNo", SysNo)

                       .Execute();
        }
        /// <summary>
        /// 修改出库单（用于事务处理）
        /// </summary>
        /// <param name="model">用于修改出库单的实体</param>
        /// <param name="status">状态</param>
        /// <returns>返回受影响的行数</returns>
        /// <remarks>2014-08-01 余勇 创建</remarks>
        public override int UpdateStockOutByStatus(WhStockOut model, int status)
        {
            int rowsAffected = Context.Update("WhStockOut", model)
                   .AutoMap(x => x.SysNo, x => x.Items, x => x.SoOrder)
                   .Where("SysNo", model.SysNo)
                   //.Where("Status", status)
                   .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 修改出库单明细中的商品数量,用于分批出库
        /// </summary>
        /// <param name="productQuantity">The product quantity.</param>
        /// <returns>
        /// 影响到的数量
        /// </returns>
        /// <remarks>
        /// 2013-07-04 周瑜 创建
        /// 2013-08-14 何方重构
        /// </remarks>
        //public override int UpdateProductQuantity(Dictionary<int, int> productQuantity)
        //{
        //    int rowsAffected = 0;
        //    using (IDbContext context = Context.UseSharedConnection(true))
        //    {
        //        rowsAffected +=
        //            productQuantity.Sum(
        //                item =>
        //                context.Sql(@"Update WhStockOutItem set ProductQuantity = :ProductQuantity where SysNo = :SysNO")
        //                       .Parameter("ProductQuantity", item.Value)
        //                       .Parameter("SysNo", item.Key)
        //                       .Execute());
        //    }
        //    return rowsAffected;
        //}

        /// <summary>
        /// 修改出库单明细中
        /// </summary>
        /// <param name="whStockOutItems">需要修改的出库单明细</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-14 何方 创建
        /// </remarks>
        public override void UpdateItems(IList<WhStockOutItem> whStockOutItems)
        {
            foreach (var item in whStockOutItems)
            {
                UpdateOutItem(item);
            }
        }

        /// <summary>
        /// 根据主键获取出库单
        /// </summary>
        /// <param name="sysNo">主键</param>
        /// <returns>出库单实体</returns>
        /// <remarks>2013-06-14 周瑜 创建</remarks>
        public override WhStockOut GetModel(int sysNo)
        {
            WhStockOut model;
            using (IDbContext context = Context.UseSharedConnection(true))
            {
                model =
                    context.Sql("select * from WhStockOut where sysno= @sysno")
                    .Parameter("sysno", sysNo)
                    .QuerySingle<WhStockOut>();

                if (model == null)
                {
                    throw new Exception("不存在出库单系统编号为 " + sysNo + " 的出库单");
                }

                model.Items = context.Sql("select m.*,pd.ErpCode as ProductErpCode  from WhStockOutItem as m left join PdProduct as pd on pd.SysNo=m.ProductSysNo  where m.StockOutSysNo = @StockOutSysNo")
                                     .Parameter("StockOutSysNo", model.SysNo)
                                     .QueryMany<WhStockOutItem>();
            }
            return model;
        }
        /// <summary>
        /// 获取出库详情
        /// </summary>
        /// <param name="stockOutSysno">出库单系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-10-24 杨浩 创建</remarks>
        public override WhStockOut GetSimpleInfo(int stockOutSysno)
        {       
            var model =
                   Context.Sql("select SysNo,WarehouseSysNo,DeliveryTypeSysNo from WhStockOut where sysno= @sysno")
                   .Parameter("sysno", stockOutSysno)
                   .QuerySingle<WhStockOut>();
            return model;
        }
        /// <summary>
        /// 根据事务编号获取出库单
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>出库单实体</returns>
        /// <remarks>2013-07-29 周瑜 创建</remarks>
        public override IList<WhStockOut> GetModelByTransactionSysNo(string transactionSysNo)
        {
            IList<WhStockOut> model = Context.Sql("select * from WhStockOut where transactionSysNo= @transactionSysNo")
                                 .Parameter("transactionSysNo", transactionSysNo)
                                 .QueryMany<WhStockOut>();

            if (model == null)
            {
                throw new Exception("不存在事务编号为 " + transactionSysNo + " 的出库单");
            }

            foreach (var whStockOut in model)
            {
                whStockOut.Items = Context.Sql("select * from WhStockOutItem where StockOutSysNo = @StockOutSysNo")
                                 .Parameter("StockOutSysNo", whStockOut.SysNo)
                                 .QueryMany<WhStockOutItem>();
            }
            return model;
        }

        /// <summary>
        /// 新增出库单
        /// </summary>
        /// <param name="model">用于新增出库单的实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public override int Insert(WhStockOut model)
        {
            int i;
            //在业务层使用事务
            using (IDbContext context = Context.UseTransaction(false))
            {
                //新增出库单主表
                i = context.Insert("WhStockOut", model)
                           .AutoMap(x => x.SysNo, x => x.Items, x => x.SoOrder)
                           .ExecuteReturnLastId<int>("SysNo");

                //新增出库单明细表           
                foreach (WhStockOutItem item in model.Items)
                {
                    item.StockOutSysNo = i;
                    context.Insert("WhStockOutItem", item)
                           .AutoMap(x => x.SysNo, x => x.IsScaned, x => x.ScanedQuantity, x => x.ProductErpCode, x => x.WarehouseErpCode)
                           .Execute();
                }
                context.Commit();
            }

            return i;
        }

        /// <summary>
        /// 逻辑删除出库单中的商品
        /// </summary>
        /// <param name="items">将要删除的商品列表</param>
        /// <returns></returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public override void RemoveItem(IList<WhStockOutItem> items)
        {
            foreach (var item in items)
            {
                item.Status = WarehouseStatus.出库单明细状态.无效.GetHashCode();
                Context.Sql(
                    "update WhStockOutItem set Status =@Status where SysNo = @SysNo")
                    .Parameter("Status", item.Status)
                       .Parameter("SysNo", item.SysNo)

                       .Execute();
            }

        }

        #region 销售单创建出库单用到的一些方法
        /// <summary>
        /// 在出库单主表上添加一条记录
        /// </summary>
        /// <param name="model">出库实体</param>
        /// <returns>出库单编号</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public override int InsertMain(WhStockOut model)
        {
            if (model.PickUpDate == DateTime.MinValue)
            {
                model.PickUpDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (model.SignTime == DateTime.MinValue)
            {
                model.SignTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (model.Stamp == DateTime.MinValue)
            {
                model.Stamp = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (model.StockOutDate == DateTime.MinValue)
            {
                model.StockOutDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            return Context.Insert("WhStockOut", model)
                           .AutoMap(x => x.SysNo, x => x.Items, x => x.SoOrder)
                           .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 在出库单明细表上添加一条记录
        /// </summary>
        /// <param name="item">出库明细</param>
        /// <returns>出库明细编号</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public override int InsertItem(WhStockOutItem item)
        {
            return Context.Insert("WhStockOutItem", item)
                          .AutoMap(x => x.SysNo, x => x.IsScaned, x => x.ScanedQuantity, x => x.ProductErpCode, x => x.WarehouseErpCode)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 根据订单编号获取出库单列表
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="onlyComplate">只读完成的定点</param>
        /// <returns>出库单列表</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        /// <remarks>2013-06-25 邵  斌 扩展，更具状态读取</remarks>
        public override IList<WhStockOut> GetWhStockOutListByOrderID(int orderId, bool onlyComplate = false)
        {
            var list = Context.Select<WhStockOut>("p.*")
                .From("WhStockOut p")
                .Where("p.ORDERSYSNO=" + orderId + (onlyComplate ? " and status=" + (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.已签收 : ""))
                .OrderBy("p.CREATEDDATE desc")
                .QueryMany();
            if (list != null)
            {
                foreach (WhStockOut item in list)
                {
                    item.Items = Context.Select<WhStockOutItem>("p.*")
                        .From("WhStockOutItem p")
                        .Where("p.stockoutsysno=" + item.SysNo)
                        .QueryMany();
                }
            }
            return list;
        }

        /// <summary>
        /// 根据订单号和产品编号获取相关的出库明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="productid">产品编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-19 朱成果 创建</remarks>
        public override IList<WhStockOutItem> GetWhStockOutItemList(int orderId, int productid)
        {
            return
                  Context.Sql("select * from WhStockOutItem where OrderSysNo=@OrderSysNo and ProductSysNo=@ProductSysNo")
                 .Parameter("OrderSysNo", orderId)
                 .Parameter("ProductSysNo", productid)
                 .QueryMany<WhStockOutItem>();

        }

        /// <summary>
        /// 获取出库单主表数据
        /// </summary>
        /// <param name="sysNo">出库单编号</param>
        /// <returns>出库单主表数据</returns>
        /// <remarks>2013-08-19 朱成果 创建</remarks>
        public override WhStockOut GetEntity(int sysNo)
        {
            return Context.Sql("select * from WhStockOut where sysno=@sysno")
                  .Parameter("sysno", sysNo)
                  .QuerySingle<WhStockOut>();
        }

        #endregion

        #region 退换货更新出库单明细
        /// <summary>
        /// 获取出库单明细
        /// </summary>
        /// <param name="sysNo">出库单明细编号</param>
        /// <returns>出库单明细数据</returns>
        /// <remarks>2013-07-15 朱成果 创建</remarks>
        public override WhStockOutItem GetStockOutItem(int sysNo)
        {
            return Context.Sql("select * from WhStockOutItem where SysNo=@SysNo")
                     .Parameter("SysNo", sysNo)
                     .QuerySingle<WhStockOutItem>();
        }

        /// <summary>
        /// 获取出库单明细集合
        /// </summary>
        /// <param name="sysNos">出库单明细编号</param>
        /// <returns>出库单明细数据</returns>
        /// <remarks>2013-07-15 朱成果 创建</remarks>
        public override IList<WhStockOutItem> GetStockOutItem(int[] sysNos)
        {
            string sysNosString = string.Join(",", sysNos);

            #region select sql
            string sql = @"select * from WhStockOutItem a 
                            where exists(
                                            select 1 from splitstr(@0,',') b where b.col = a.sysno
                                        )";
            #endregion

            return Context.Sql(sql, sysNosString).QueryMany<WhStockOutItem>();
        }

        /// <summary>
        /// 更新出库单明细
        /// </summary>
        /// <param name="item">出库单明细实体</param>
        /// <returns></returns>
        /// <remarks>2013-07-15 朱成果 创建</remarks>
        public override void UpdateOutItem(WhStockOutItem item)
        {
            Context.Update("WhStockOutItem", item)
                        .AutoMap(x => x.SysNo, x => x.IsScaned, x => x.ScanedQuantity,x=>x.ProductErpCode,x=>x.WarehouseErpCode)
                        .Where("SysNo", item.SysNo)
                        .Execute();
        }

        #endregion

        /// <summary>
        /// 根据系统编号集合获取出库单列表
        /// </summary>
        /// <param name="sysNos">出库单系统编号数组</param>
        /// <returns>出库单列表</returns>
        /// <remarks>2013-07-04 沈强 创建</remarks>
        /// <remarks>2013-07-06 何方 重构</remarks>
        public override IList<WhStockOut> GetWhStockOutListBySysNos(int[] sysNos)
        {
            //保存系统编号字符串
            string sysNosString = string.Join(",", sysNos);

            #region select sql
            string sql = @"select * from WhStockOut a 
                            where exists(
                                            select 1 from splitstr(@0,',') b where b.col = a.sysno
                                        )";
            #endregion

            return Context.Sql(sql, sysNosString).QueryMany<WhStockOut>();
        }

        /// <summary>
        /// 根据系统编号集合获取出库单及收货人列表
        /// </summary>
        /// <param name="sysNos">出库单系统编号集合</param>
        /// <returns>出库单及收货人列表</returns>
        /// <remarks>2013-07-09 黄伟 创建</remarks>
        public override IList<CBWhStockOut> GetWhStockOutList(int[] sysNos)
        {
            List<CBWhStockOut> model;

            using (var context = Context.UseSharedConnection(true))
            {
                model = Context.Sql(@"select m.*,a.name as ReceiverName,d.DeliveryTypeName from WhStockOut m 
inner join SoReceiveAddress a on m.receiveaddresssysno=a.sysno
inner join lgdeliverytype d on m.DeliveryTypeSysNo=d.sysno
                            where exists(
                                            select 1 from splitstr(@0,',') b where b.col = m.sysno
                                        )", string.Join(",", sysNos))
                                   .QueryMany<CBWhStockOut>();
                model.ForEach(p => p.Items = GetWhStockOutItemList(p.SysNo));
            }

            return model;

        }

        /// <summary>
        /// 根据出库单系统编号获取出库单明细列表
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <returns>出库单明细列表</returns>
        /// <remarks>2013-07-09 黄伟 创建</remarks>
        public override IList<WhStockOutItem> GetWhStockOutItemList(int sysNo)
        {
            return Context.Sql(@"select m.*,pd.ErpCode as ProductErpCode from WhStockOutItem as m left join PdProduct as pd on pd.SysNo=m.ProductSysNo
                            where m.stockoutsysno=@sysno and m.status!=0")
                          .Parameter("sysno", sysNo)
                          .QueryMany<WhStockOutItem>();

        }

        /// <summary>
        /// 根据销售单明细编号获取有效的出库单明细列表
        /// </summary>
        /// <param name="orderItemSysNo">销售单明细编号</param>
        /// <returns>出库单明细列表</returns>
        /// <remarks>2013-11-22 吴文强 创建</remarks>
        public override IList<WhStockOutItem> GetStockOutItems(int[] orderItemSysNo)
        {
            if (orderItemSysNo.Count() == 0 || orderItemSysNo == null)
            {
                return new List<WhStockOutItem>();
            }
            var sql = @"
                        select soi.* from WhStockOutItem soi
                        left join WhStockOut so on soi.stockoutsysno = so.sysno
                        where so.status != @StockOutStatus 
                        and soi.status = @StockOutItemStatus
                        and orderitemsysno in(@OrderItemSysNo)
                        order by soi.createddate
                    ";
            return Context.Sql(sql)
                          .Parameter("StockOutStatus", WarehouseStatus.出库单状态.作废)
                          .Parameter("StockOutItemStatus", WarehouseStatus.出库单明细状态.有效)
                          .Parameter("OrderItemSysNo", orderItemSysNo)
                          .QueryMany<WhStockOutItem>();
        }

        /// <summary>
        /// 根据出库单系统编号获取相应订单
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号</param>
        /// <returns>订单实体</returns>
        /// <remarks>2013-07-11 黄伟 创建</remarks>
        public override SoOrder GetSoOrder(int stockOutSysNo)
        {
            return Context.Sql(@"select * from soorder s
where s.sysno=(select w.ordersysno from WhStockOut w where w.sysno=@sysno)")
                          .Parameter("sysno", stockOutSysNo)
                          .QuerySingle<SoOrder>();
        }

        /// <summary>
        /// 更新出库单
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号</param>
        /// <param name="status">更新状态</param>
        /// <returns></returns>
        /// <remarks>2013-8-21 周瑜 创建</remarks>
        public override void UpdateStatus(int stockOutSysNo, WarehouseStatus.出库单状态 status)
        {
            var affected =
            Context.Sql(
                "update WhStockOut set Status =@Status  where  SysNo = @StockOutSysNo")
                   .Parameter("Status", (int)status)
                   .Parameter("StockOutSysNo", stockOutSysNo)
                   .Execute();
            if (affected != 1)
            {
                throw new Exception("更新的行数为" + affected);
            }
        }

        /// <summary>
        /// 判断出库单是否为配送单中的第一单
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <returns>true:是第一单 false:非第一单 </returns>
        /// <remarks>2013-07-31 周瑜创建</remarks>
        public override bool GetWhStockOutWithDeliveryCount(int sysNo)
        {
            //notetype=10 指配送单的类型为出库单
            //iscod = 1 指为货到付款
            //status!=-10 指状态不为作废
            var count = Context.Sql(@"select count(1) from 
lgdeliveryitem a inner join soorder b on a.transactionsysno = b.transactionsysno
where 
a.notetype=10 and a.status!=-10 and a.iscod = 1
and b.transactionsysno = (select transactionsysno from whstockout where sysno = @sysno)")
                               .Parameter("sysno", sysNo)
                               .QuerySingle<int>();
            return count > 1;
        }

        /// <summary>
        /// 根据客户系统编号与出库单状态获取退换货详细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="status">出库单状态</param>
        /// <returns>退换货详细集合</returns>
        /// <remarks>2013-09-12 沈强 创建</remarks>
        public override IList<Model.B2CApp.ReturnDetail> GetReturnDetailByCustomerSysNoAndStatus(int customerSysNo, int status)
        {
            string sql = @"select 
            a.sysno as StockOutSysNo
            ,b.sysno as OrderSysNo
            ,b.createdate
            ,b.onlinestatus
            ,b.orderamount 
            ,(case 
                    when a.iscod = 1 then 0 
                    when a.iscod = 0 then 1 
                end) as IsOnlinePay
            from whstockout a inner join soorder b
            on a.ordersysno = b.sysno
            where b.customersysno = @0 and a.status = @1";

            return Context.Sql(sql).Parameters(customerSysNo, status).QueryMany<Model.B2CApp.ReturnDetail>();
        }

        /// <summary>
        /// 根据客户系统编号与出库单状态获取退换货详细
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号</param>
        /// <returns>退换货详细</returns>
        /// <remarks>2013-09-12 沈强 创建</remarks>
        public override Model.B2CApp.ReturnDetail GetReturnDetailByStockOutSysNo(int stockOutSysNo)
        {
            string sql = @"select 
            a.sysno as StockOutSysNo
            ,b.sysno as OrderSysNo
            ,b.createdate
            ,b.onlinestatus
            ,b.orderamount 
            ,(case 
                    when a.iscod = 1 then 0 
                    when a.iscod = 0 then 1 
                end) as IsOnlinePay
            from whstockout a inner join soorder b
            on a.ordersysno = b.sysno
            where a.sysno = @0";

            return Context.Sql(sql).Parameters(stockOutSysNo).QuerySingle<Model.B2CApp.ReturnDetail>();
        }

        /// <summary>
        /// 获取出库单明细
        /// </summary>
        /// <param name="sysNo">出库单明细编号</param>
        /// <returns>出库单明细</returns>
        /// <remarks>2013-12-04 yangheyu 创建</remarks>
        public override WhStockOutItem GetWhStockOutItem(int sysNo)
        {
            return Context.Sql("select * from WhStockOutItem where sysno =@0", sysNo).QuerySingle<WhStockOutItem>();
        }
    }
}