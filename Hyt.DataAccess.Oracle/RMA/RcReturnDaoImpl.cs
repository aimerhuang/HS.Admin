using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.RMA;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.RMA
{

    /// <summary>
    /// 退换货
    /// </summary>
    /// <remarks>2013-07-11 朱成果 创建</remarks>
    public class RcReturnDaoImpl : IRcReturnDao
    {
        /// <summary>
        /// 插入退换货数据
        /// </summary>
        /// <param name="entity">退换货实体</param>
        /// <returns>返回新的编号</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public override int Insert(Model.RcReturn entity)
        {
            var sysNo = Context.Insert("RcReturn", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }

        /// <summary>
        /// 更新退换货数据
        /// </summary>
        /// <param name="entity">退换货实体</param>
        /// <returns></returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public override void Update(Model.RcReturn entity)
        {
            Context.Update("RcReturn", entity)
                .AutoMap(o => o.SysNo)
                .Where("SysNo", entity.SysNo).Execute();
        }

        /// <summary>
        /// 获取当前订单待处理的退货单数量（不包括，作废和已完成的退换货单)
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns>退货单数量</returns>
        /// <remarks>2013-07-16 朱成果 创建</remarks>
        public override int GetDealWithCount(int orderID)
        {
            return Context.Sql("select count(1) from RcReturn where OrderSysNo=@OrderSysNo and Status<>" + ((int)Hyt.Model.WorkflowStatus.RmaStatus.退换货状态.作废).ToString() + " and Status<>" + ((int)Hyt.Model.WorkflowStatus.RmaStatus.退换货状态.已完成).ToString())
                 .Parameter("OrderSysNo", orderID).QuerySingle<int>();
        }

        /// <summary>
        /// 获取当前订单待处理的退货单（不包括，作废和已完成的退换货单)
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns>获取当前订单待处理的退货单（不包括，作废和已完成的退换货单)</returns>
        /// <remarks>2013-07-16 朱成果 创建</remarks>
        public override RcReturn GetDealWithRMA(int orderID)
        {
            return Context.Sql("select * from RcReturn where OrderSysNo=@OrderSysNo and Status<>" + ((int)Hyt.Model.WorkflowStatus.RmaStatus.退换货状态.作废).ToString() + " and Status<>" + ((int)Hyt.Model.WorkflowStatus.RmaStatus.退换货状态.已完成).ToString())
                .Parameter("OrderSysNo", orderID).QuerySingle<RcReturn>();
        }

        /// <summary>
        /// 获取当前订单待审核的退货单
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns>获取当前订单待审核的退货单</returns>
        /// <remarks>2014-04-24 余勇 创建</remarks>
        public override RcReturn GetPendWithReturn(int orderID)
        {
            return Context.Sql("select * from RcReturn where OrderSysNo=@OrderSysNo and RmaType=" + (int)RmaStatus.RMA类型.售后退货 + " and Status=" + ((int)Hyt.Model.WorkflowStatus.RmaStatus.退换货状态.待审核).ToString())
                .Parameter("OrderSysNo", orderID).QuerySingle<RcReturn>();
        }

        /// <summary>
        /// 获取退换货单实体
        /// </summary>
        /// <param name="sysNo">退换货单编号</param>
        /// <returns>退换货单实体</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public override CBRcReturn GetEntity(int sysNo)
        {
            return Context.Sql("select * from RcReturn where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo).QuerySingle<CBRcReturn>();
        }

        /// <summary>
        /// 转门店
        /// </summary>
        /// <param name="sysNo">退款单编号</param>
        /// <param name="handleDepartment">处理部门</param> 
        /// <returns>影响行数</returns> 
        /// <remarks>2013-06-19 余勇 创建</remarks>
        public override int UpdateRcReturnToShop(int sysNo, int handleDepartment)
        {
            return Context.Update("RcReturn")
                                .Column("HandleDepartment", handleDepartment)
                                .Column("WarehouseSysNo", 0)
                                .Where("SysNo", sysNo)
                                .Execute();
        }

        /// <summary>
        /// 退换货单分页查询
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>退换货表</returns>
        /// <remarks>2013-07-11 朱家宏 创建</remarks>
        public override Pager<CBRcReturn> GetAll(ParaRmaFilter filter)
        {
            const string sql =
                 @"(select  a.sysno,a.ordersysno,a.refundtotalamount,a.status,a.createdate,a.rmatype,a.source,a.handledepartment,a.CustomerSysNo,
                            c.account as customerAccount,
                            e.warehousename,e.backwarehousename
                    from    rcreturn a
                            left join soOrder b on b.sysno=a.ordersysno
                            left join crCustomer c on c.sysno=a.customersysno
                            left join WhwareHouse e on e.SYSNO = a.warehousesysno
                    where   
                            (@0 is null or c.account=@0) and                                                                            --会员号
                            (@1 is null or a.sysno=@1) and                                                                                                  --RmaId
                            (@2 is null or a.OrderSysNo=@2) and                                                                                   --OrderSysNo
                            (@3 is null or exists (select 1 from splitstr(@3,',') tmp where tmp.col = b.ordersource)) and     --销售单来源
                            (@4 is null or @4 = '' or exists (select 1 from splitstr(@4,',') tmp where tmp.col = a.status)) and            --退换货状态
                            (@5 is null or exists (select 1 from splitstr(@5,',') tmp where tmp.col = a.source)) and              --申请单来源
                            (@6 is null or a.RmaType=@6) and                                                                                            --RmaType
                            (@7 is null or a.createDate>=@7) and                                                                                    --创建日期(起)
                            (@8 is null or a.createDate<@8) and                                                                                         --创建日期(止) 
                            (@9 is null or c.SysNo=@9) and                                                                                     --会员编号
                            (@10='true' or (a.warehousesysno is null or a.warehousesysno=0)) and      
                            (@11 is null or exists (select 1 from splitstr(@11,',') tmp where tmp.col = a.HandleDepartment)) and              --申请单处理部门
                            (@12 is null or exists (select 1 from splitstr(@12,',') tmp where tmp.col = e.sysNo))
                    ) tb";

            var orderSources = filter.OrderSources != null ? string.Join(",", filter.OrderSources) : null;
            var rmaStatuses = filter.RmaStatuses != null ? string.Join(",", filter.RmaStatuses) : null;
            var rmaSources = filter.RmaSources != null ? string.Join(",", filter.RmaSources) : null;
            var handleDepartments = filter.HandleDepartments != null ? string.Join(",", filter.HandleDepartments) : null;
            var storeSysNos = filter.StoreSysNoList != null ? string.Join(",", filter.StoreSysNoList) : null;

            //查询日期上限+1
            filter.EndDate = filter.EndDate == null ? (DateTime?)null : filter.EndDate.Value.AddDays(1);

            var paras = new object[]
                {
                    filter.CustomerAccount,    // filter.CustomerAccount,
                    filter.RmaId,             ///  filter.RmaId,
                    filter.OrderSysNo,        //  filter.OrderSysNo,
                    orderSources,              // orderSources,
                    rmaStatuses,             //   rmaStatuses,
                    rmaSources,              //   rmaSources,
                    filter.RmaType,            // filter.RmaType,
                    filter.BeginDate,         //  filter.BeginDate,
                    filter.EndDate,           //  filter.EndDate,
                    filter.CustomerSysNo,      // filter.CustomerSysNo,
                    filter.HasWarehouse.ToString().ToLower(),
                    handleDepartments,        //  handleDepartments,
                    storeSysNos//,                storeSysNos 
                };

            var dataList = Context.Select<CBRcReturn>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBRcReturn>
                {
                    PageSize = filter.PageSize,
                    CurrentPage = filter.Id
                };

            pager.TotalRows = dataCount.QuerySingle();
            pager.Rows = dataList.OrderBy("tb.createdate desc").Paging(pager.CurrentPage, filter.PageSize).QueryMany();

            return pager;

        }

        /// <summary>
        /// 通过订单编号获得退换货列表
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>退换货列表</returns>
        /// <remarks>
        /// 2013-12-06 余勇 创建
        /// </remarks>
        public override List<CBRcReturn> GetRmaReturnListByOrderSysNo(int orderSysNo)
        {
            return Context.Sql(@"select  a.sysno,a.ordersysno,a.refundtotalamount,a.status,a.createdate,a.rmatype,a.source,a.handledepartment,a.CustomerSysNo,
                            c.account as customerAccount,
                            e.warehousename
                    from    rcreturn a
                            left join soOrder b on b.sysno=a.ordersysno
                            left join crCustomer c on c.sysno=a.customersysno
                            left join WhwareHouse e on e.SYSNO = a.warehousesysno
                            where b.sysno=@OrderSysNo
                            ")
                            .Parameter("OrderSysNo", orderSysNo)
                            .QueryMany<CBRcReturn>();
        }

        /// <summary>
        /// 根据出库单明细系统编号获取退换货申请单
        /// </summary>
        /// <param name="stockOutItemSysNo">出库单明细系统编号</param>
        /// <param name="sourceType">退换货申请单来源</param>
        /// <returns>退换货列表</returns>
        /// <remarks>
        /// 2013/8/21 何方 创建
        /// </remarks>
        public override IList<RcReturn> Get(int stockOutItemSysNo, Model.WorkflowStatus.RmaStatus.退换货申请单来源? sourceType)
        {
            int? source = null;
            if (sourceType != null)
                source = (int)sourceType;

            return Context.Sql(@"select * from RcReturn 
                    where (@source is null or source=@source)
                    and SysNo in
                    (select distinct ReturnSysNo from rcreturnitem
                    where StockOutItemSysNo=@StockOutItemSysNo)")
                          .Parameter("source", source)
                          .Parameter("StockOutItemSysNo", stockOutItemSysNo)
                          .QueryMany<RcReturn>();
        }

        #region 退货金额计算使用

        /// <summary>
        /// 获取有效销售订单明细
        /// 订单明细数量不包含退货(退换货类型为退货并审核通过)数量
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>有效销售订单明细</returns>
        /// <remarks>2013-09-13 吴文强 创建</remarks>
        public override IList<SoOrderItem> GetValidSalesOrderItem(int orderSysNo)
        {
            const string sql =
                @"
                select oi.sysno
                        ,oi.OrderSysNo
                        ,oi.TransactionSysNo
                        ,oi.ProductSysNo
                        ,oi.ProductName
                        ,oi.quantity - isnull((select sum(ri.rmaquantity) 
                        from WhStockOutItem soi
                        left join RcReturnItem  ri on ri.stockoutitemsysno = soi.sysno
                        left join RcReturn r on r.sysno = ri.returnsysno
                        where soi.orderitemsysno = oi.sysno
                        and r.rmatype = 20 and r.status > 10),0) as quantity    
                        ,oi.OriginalPrice
                        ,oi.SalesUnitPrice
                        ,oi.SalesAmount
                        ,oi.DiscountAmount
                        ,oi.ChangeAmount
                        ,oi.RealStockOutQuantity
                        ,oi.ProductSalesType
                        ,oi.ProductSalesTypeSysNo
                        ,oi.GroupCode
                        ,oi.GroupName
                        ,oi.UsedPromotions
                from SoOrderItem oi
                where oi.OrderSysNo = @orderSysNo
                ";

            return Context.Sql(sql)
                          .Parameter("orderSysNo", orderSysNo)
                          .QueryMany<SoOrderItem>();
        }

        /// <summary>
        /// 获取退货已退回的惠源币
        /// 退换货状态：=待退款 or 已完成
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns>退货已退回的惠源币</returns>
        /// <remarks>2013-10-29 吴文强 创建</remarks>
        public override decimal GetReturnCoin(int orderSysNo)
        {
            const string sql = @"
                            select sum(RefundCoin) 
                            from RcReturn 
                            where (Status = @Status or Status = @Status1) --状态:待退款或已完成
                                and OrderSysNo = @OrderSysNo";

            return Context.Sql(sql)
                          .Parameter("Status", (int)RmaStatus.退换货状态.待退款)
                          .Parameter("Status1", (int)RmaStatus.退换货状态.已完成)
                          .Parameter("OrderSysNo", orderSysNo)
                          .QuerySingle<decimal>();
        }
        #endregion

        #region 后台首页统计信息

        /// <summary>
        /// 退换货单统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="filter">分销商权限信息</param>
        /// <param name="infomation">信息对象</param>
        /// <returns>统计数据</returns>
        /// <remarks>2013-09-26 邵斌 创建</remarks>
        public override CBDefaultPageCountInfo GetRMATotalInformation(System.DateTime startTime, System.DateTime endTime, ParaIsDealerFilter filter, ref CBDefaultPageCountInfo infomation)
        {

            //统计退还单总数和退换货总金额
            //var where = "where a.status<>@status and a.createdate between @startTime and @endTime";
            var where = "where r.status<>@status ";
            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    where += " and dea.SysNo = " + filter.DealerSysNo;
                }
                else
                {
                    where += " and dea.CreatedBy = " + filter.DealerCreatedBy;
                }
            }

            var sql = @"select count(r.sysno) as RMACount,sum(r.OrginAmount) as RMAAmount  from RcReturn r left join SoOrder a on r.OrderSysNo = a.SysNo 
                       left join DsDealer dea on a.DealerSysNo = dea.SysNo " + where;
            var info = Context.Sql(sql)
                .Parameter("status", (int)OrderStatus.销售单状态.作废)
                //.Parameter("startTime", startTime)
                //.Parameter("endTime", endTime)
                .QuerySingle<CBDefaultPageCountInfo>();

            infomation.RMACount = info.RMACount;
            infomation.RMAAmount = info.RMAAmount;

            return infomation;

        }

        /// <summary>
        /// 退换货单统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="filter">分销商权限信息</param>
        /// <param name="infomation">信息对象</param>
        /// <returns>统计数据</returns>
        /// <remarks>2013-09-26 邵斌 创建</remarks>
        public override CBDefaultPageCountInfo GetTodayRMATotalInformation(System.DateTime startTime, System.DateTime endTime, ParaIsDealerFilter filter, ref CBDefaultPageCountInfo infomation)
        {
            //待审核退换货单
            //var where = "where a.status<>@status and a.createdate between @startTime and @endTime";
            var where = "where r.status=@status";
            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    where += " and dea.SysNo = " + filter.DealerSysNo;
                }
                else
                {
                    where += " and dea.CreatedBy = " + filter.DealerCreatedBy;
                }
            }

            var sql = @"select count(r.sysno) as RequiredAuidtRMAOrderCount  from RcReturn r left join SoOrder a on r.OrderSysNo = a.SysNo 
                       left join DsDealer dea on a.DealerSysNo = dea.SysNo " + where;
            infomation.RequiredAuidtRMAOrderCount = Context.Sql(sql)
                .Parameter("status", (int)OrderStatus.销售单状态.待审核)
                //.Parameter("startTime", startTime)
                //.Parameter("endTime", endTime)
                .QuerySingle<int>();
            return infomation;

        }

        #endregion

        /// <summary>
        /// 获取还货订单编号
        /// </summary>
        /// <param name="rmaid">退换货编号</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-06-17 朱成果 创建
        /// </remarks>
        public override  int GetRMAOrderSysNo(int rmaid)
        {
           return 
                 Context.Sql("select SysNo from SoOrder where OrderSource=@OrderSource and OrderSourceSysNo=@OrderSourceSysNo")
                .Parameter("OrderSource", (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.RMA下单)
                .Parameter("OrderSourceSysNo", rmaid)
                .QuerySingle<int>();
        }

        /// <summary>
        /// 退换货返利扣除列表
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-6 杨浩 创建</remarks>
        public override List<CBReurnDeductRebates> GetReurnDeductRebates(int orderSysNo)
        {
            return
                Context.Sql("select crr.SysNo as SysNo,ri.DeductRebates as DeductRebates,r.Status as ReturnStatus,r.SysNo as ReturnSysNo  RcReturn as r  inner join RcReturnItem  as ri on r.SysNo=ri.ReturnSysNo  where r.OrderSysNo=@orderSysNo ")
               .Parameter("orderSysNo", orderSysNo)
               .QueryMany<CBReurnDeductRebates>();
        }
    }
}
