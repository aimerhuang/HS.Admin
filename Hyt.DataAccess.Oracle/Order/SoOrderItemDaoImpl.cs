using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.SellBusiness;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.DataAccess.Order;

namespace Hyt.DataAccess.Oracle.Order
{
    /// <summary>
    /// 订单明细
    /// </summary>
    /// <remarks>2013-06-21 朱家宏 创建</remarks>
    public class SoOrderItemDaoImpl : ISoOrderItemDao
    {
        /// <summary>
        /// 根据orderId获取订单明细
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>订单明细集合</returns>
        /// <remarks>2013-06-21 朱家宏 创建</remarks>
        public override IList<SoOrderItem> GetOrderItemsByOrderSysNo(int orderSysNo)
        {
            var items =
                Context.Select<SoOrderItem>("*")
                       .From("SoOrderItem")
                       .Where("OrderSysNo=@OrderSysNo")
                       .Parameter("OrderSysNo", orderSysNo)
                       .QueryMany();
            return items;
        }

        /// <summary>
        /// 根据用户系统号获取订单明细
        /// </summary>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>订单明细集合</returns>
        /// <remarks>2013-08-16 杨晗 创建</remarks>
        public override IList<SoOrderItem> GetOrderItemsByCustomerSysNo(int customerSysNo)
        {
            const string sql =
                @"select si.* from soorderitem si left outer join soorder so on si.ordersysno=so.sysno where so.customersysno=@customersysno";
            var items = Context.Sql(sql)
                               .Parameter("customerSysNo", customerSysNo)
                               .QueryMany<SoOrderItem>();
            return items;
        }

        /// <summary>
        /// 根据orderId获取订单明细
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>订单明细(包括商品ERP code）集合</returns>
        /// <remarks>2013-07-26 朱成果 创建</remarks>
        public override IList<CBSoOrderItem> GetOrderItemsWithErpCodeByOrderSysNo(int orderSysNo)
        {
//            var items = Context.Sql(@"select distinct SoOrderItem.*,ErpCode,
//           (select sum(StockQuantity) from PdProductStock where WarehouseSysNo = (select DefaultWarehouseSysNo from SoOrder where SysNo = @OrderSysNo) and PdProductSysNo = SoOrderItem.Productsysno) as StockQuantity
//           from SoOrderItem left outer join PdProduct on SoOrderItem.Productsysno=SoOrderItem.Sysno
//           where OrderSysNo=@OrderSysNo")
            var items = Context.Sql(@"select SoOrderItem.*,PdProduct.ErpCode,0 as StockQuantity
           from SoOrderItem left outer join PdProduct on SoOrderItem.Productsysno=PdProduct.Sysno
           where OrderSysNo=@OrderSysNo")
                                        .Parameter("OrderSysNo", orderSysNo)
                                        .QueryMany<CBSoOrderItem>();
           return items;
        }
        /// <summary>
        /// 获取订单明细
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>2016-1-7 王耀发 创建</remarks>
        public override Pager<CBSoOrderItem> GetOrderItemsRecordList(ParaOrderItemRecordFilter filter)
        {
            string sqlWhere = "1=1";

            string sql = @"(select a.*,b.ErpCode, b.EasName,b.LastUpdateDate,c.RebateRtio,c.OperatFee
                    from SoOrderItem a left join PdProduct b on a.Productsysno = b.SysNo 
                    left join SoOrder c on a.OrderSysNo = c.SysNo
                    where (@0 = 0 or a.OrderSysNo = @1) 
                      and " + sqlWhere + " ) tb";

            var dataList = Context.Select<CBSoOrderItem>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var paras = new object[]
                {
                    filter.OrderSysNo,
                    filter.OrderSysNo
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBSoOrderItem>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy(" tb.LastUpdateDate desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }

        /// <summary>
        /// 插入销售单明细
        /// </summary>
        /// <param name="item">销售单明细</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public override int Insert(SoOrderItem item)
        {
            item.SysNo = Context.Insert("SoOrderItem", item)
                          .AutoMap(o => o.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
            return item.SysNo;
        }

        /// <summary>
        /// 更新订单商品明细数量
        /// </summary>
        /// <param name="sysNo">明细编号</param>
        /// <param name="quantity">数量</param>
        /// <param name="changeAmount">调价金额</param>
        /// <returns></returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        /// <remarks>2013-09-17 黄志勇 修改</remarks>
        public override void UpdateOrderItemQuantity(int sysNo, int quantity, decimal changeAmount)
        {
            Context.Sql("update SoOrderItem set Quantity=@Quantity,ChangeAmount=@changeAmount where SysNo=@SysNo")
                   .Parameter("Quantity", quantity)
                   .Parameter("ChangeAmount", changeAmount)
                   .Parameter("SysNo", sysNo).Execute();
            Context.Sql("update SoOrderItem set SalesAmount=SalesUnitPrice*Quantity where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo).Execute();

        }

        /// <summary>
        /// 获取订单明细
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns>订单明细列表</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public override SoOrderItem GetEntity(int sysNo)
        {
            return Context.Sql("select * from SoOrderItem where SysNo=@SysNo")
                          .Parameter("SysNo", sysNo).QuerySingle<SoOrderItem>();
        }

        /// <summary>
        /// 删除订单明细
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="sysNo">订单商品明细编号</param>
        /// <returns>操作是否成功 </returns>
        /// <remarks>2013-06-24 朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Delete("SoOrderItem").Where("SysNo", sysNo).Execute();
        }

        /// <summary>
        /// 删除订单明细
        /// </summary>
        /// <param name="OrderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-07-13 王耀发 创建</remarks>
        public override void DeleteByOrderSysNo(int OrderSysNo)
        {
            Context.Sql("delete SoOrderItem where OrderSysNo=@OrderSysNo")
                   .Parameter("OrderSysNo", OrderSysNo)
                   .Execute();
        }

        /// <summary>
        /// 更新销售单出库数量
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <param name="quantity">出库数量</param>
        /// <returns> </returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public override void UpdateOutStockQuantity(int sysNo, int quantity)
        {
            Context.Sql("update SoOrderItem set RealStockOutQuantity=@RealStockOutQuantity where SysNo=@SysNo")
                   .Parameter("RealStockOutQuantity", quantity)
                   .Parameter("SysNo", sysNo)
                   .Execute();
        }

        /// <summary>
        /// 同步订单总价
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns> 返回订单应收款金额</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        /// <remarks>2013-09-18 黄志勇 修改</remarks>
        public override decimal SynchronousOrderAmount(int orderId)
        {
            //同步商品销售价合计,商品折扣金额,商品调价金额合计
            Context.Sql(@"
                        update soorder  set
                        ProductAmount=
                        (
                        select  sum(SalesAmount)
                        from soorderitem where ordersysno=@id
                        ),
                        ProductDiscountAmount=
                        (
                        select  sum(DiscountAmount)
                        from soorderitem where ordersysno=@id
                        ),
                         ProductChangeAmount=
                        (
                        select  sum(ChangeAmount)
                        from soorderitem where ordersysno=@id
                        )
                        where sysno=@id")
                   .Parameter("id", orderId)        
                   .Execute();
            //同步 销售单总金额,总金额中现金支付部分
            Context.Sql(@"
                        update soorder
                        set 
                        OrderAmount=isnull(ProductAmount,0)+isnull(ProductChangeAmount,0)+isnull(FreightChangeAmount,0)+isnull(FreightAmount,0)+isnull(TaxFee,0)-isnull(FreightDiscountAmount,0)-isnull(OrderDiscountAmount,0)-isnull(CouponAmount,0)-isnull(ProductDiscountAmount,0)                        
                        where sysNo=@id
                        ")
                 .Parameter("id", orderId)
                 .Execute();
            //订单现金支付金额=订单总金额-会员币支付金额-余额支付金额
            Context.Sql(@"
                        update soorder
                        set 
                        CashPay=isnull(OrderAmount,0)-isnull(CoinPay,0)-isnull(BalancePay,0)                        
                        where sysNo=@id
                        ")
                .Parameter("id", orderId)
                .Execute();

            return
                Context.Sql("select CashPay from SoOrder where SysNo=@SysNo")
                       .Parameter("SysNo", orderId)
                       .QuerySingle<decimal>();//返回应收金额
        }

        /// <summary>
        /// 获取晒单评价中当前客户已完成的销售单明细列表
        /// </summary>
        /// <param name="pageIndex">起始页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="customerSysNo">用户系统号</param>
        /// <param name="count">抛出总数</param>
        /// <returns>销售单明细列表</returns>
        /// <remarks>2013-08-15 杨晗 创建</remarks>
        public override IList<CBFeCommentList> GetFeCommentList(int pageIndex, int pageSize, int customerSysNo,
                                                                out int count)
        {
            var returnValue = new List<CBFeCommentList>();
            int pageStart = (pageIndex - 1) * pageSize + 1;
            int pageEnd = pageIndex * pageSize;

            #region sql语句

            const string sql = @"select * from (select row_number() over(order by sysno,ProductSysNo) rn,a.*
from (select distinct so.sysno,so.createdate,i.ProductSysNo,i.ProductName from SoOrder so left outer join SoOrderItem i on so.sysno=i.OrderSysNo
where so.status=@status and so.CustomerSysNo=@CustomerSysNo) a) where rn between @pageStart and @pageEnd
";
            const string sqlCount =
                @"select count(*) from (select distinct so.sysno,so.createdate,i.ProductSysNo,i.ProductName
from SoOrder so left outer join SoOrderItem i on so.sysno=i.OrderSysNo where so.status=@status and so.CustomerSysNo=@CustomerSysNo)
";

            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                count = context.Sql(sqlCount)
                               .Parameter("status", (int)OrderStatus.销售单状态.已完成)
                               .Parameter("CustomerSysNo", customerSysNo)
                               .QuerySingle<int>();
                returnValue = context.Sql(sql)
                                     .Parameter("status", (int)OrderStatus.销售单状态.已完成)
                                     .Parameter("CustomerSysNo", customerSysNo)
                                     .Parameter("pageStart", pageStart)
                                     .Parameter("pageEnd", pageEnd)
                                     .QueryMany<CBFeCommentList>();
            }
            return returnValue;
        }

        /// <summary>
        /// 获取晒单评价中当前客户已完成并且没晒单的销售单明细列表
        /// </summary>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>销售单明细列表</returns>
        /// <remarks>2013-08-15 杨晗 创建</remarks>
        public override IList<CBFeCommentList> GetFeCommentList(int customerSysNo)
        {
            const string sql = @"select * from (select row_number() over(order by sysno,ProductSysNo) rn,a.*
from (select distinct so.sysno,so.createdate,i.ProductSysNo,i.ProductName from SoOrder so left outer join SoOrderItem i on so.sysno=i.OrderSysNo
where so.status=@status and so.CustomerSysNo=@CustomerSysNo
 and i.productsysno not in (select productsysno from feproductcomment where customersysno=@CustomerSysNo and isshare=@isshare) 
) a) where rn between 1 and 5";
            return Context.Sql(sql)
                          .Parameter("status", (int)OrderStatus.销售单状态.已完成)
                          .Parameter("CustomerSysNo", customerSysNo)
                // .Parameter("CustomerSysNo", customerSysNo)
                          .Parameter("isshare", (int)ForeStatus.是否晒单.是)
                          .QueryMany<CBFeCommentList>();
        }

        /// <summary>
        /// 获取升仓赠品列表
        /// </summary>
        /// <param name="orderid">订单编号</param>
        /// <returns>升仓赠品列表</returns>
        /// <remarks>2014-07-03 朱成果 创建</remarks>
        public override IList<CBSoOrderItem> GetMallGiftItems(int orderid)
        {
            var items = Context.Sql(
            @"
            select distinct SoOrderItem.*,ErpCode
            from SoOrderItem left outer join PdProduct on SoOrderItem.Productsysno=PdProduct.Sysno
            where OrderSysNo=@OrderSysNo and SalesAmount=0 and GroupName='淘宝赠品'")
           .Parameter("OrderSysNo", orderid).QueryMany<CBSoOrderItem>();
            return items;
        }
        /// <summary>
        /// 获得推送订单需要的参数信息
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns>2015-09-02 王耀发 创建</returns>
        public override IList<SendSoOrderModel> GetSendSoOrderModelByOrderSysNo(int orderSysNo)
        {
            var items =
                Context.Select<SendSoOrderModel>("'' as OverseaCarrier,'' as OverseaTrackingNo,'1' as WarehouseId,'' as CustomerReference,'' as MerchantName,'' as MerchantOrderNo,'' as ConsigneeFirstName," +
                                                 "'' as ConsigneeLastName,'' as Remark,b.ErpCode as SKU,b.ErpCode as UPC,b.ProductName as CommodityName,'' as Category,'' as Brand,'' as Color," + 
                                                 "'' as Size,'' as Material,'' as CommoditySourceURL,'' as CommodityImageUrlList, a.OriginalPrice as UnitPrice," +
                                                 "a.OriginalPrice as DeclaredValue,b.ValueUnit as ValueUnit, b.NetWeight as [Weight], b.SalesMeasurementUnit as WeightUnit," +
                                                 "isnull(b.Volume,'') as Volume,isnull(b.VolumeUnit,'') as VolumeUnit,a.Quantity as Quantity,b.SysNo as CustomerReferenceSub")
                       .From("SoOrderItem a left join PdProduct b on a.ProductSysNo = b.SysNo")
                       .Where("a.OrderSysNo=@OrderSysNo")
                       .Parameter("OrderSysNo", orderSysNo)
                       .QueryMany();
            return items;
        }
        /// <summary>
        /// 获得推送订单需要的参数信息
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns>2015-09-02 王耀发 创建</returns>
        public override SendSoOrderTitleModel GetSendSoOrderTitleModelByOrderSysNo(int orderSysNo)
        {
            return Context.Sql("select '' as OverseaCarrier,'' as OverseaTrackingNo,'1' as WarehouseId,'' as CustomerReference,'' as MerchantName,'' as MerchantOrderNo,'' as ConsigneeFirstName," +
                               "'' as ConsigneeLastName,'' as Remark from SoOrder a where a.SysNo=@OrderSysNo")
                   .Parameter("OrderSysNo", orderSysNo)
              .QuerySingle<SendSoOrderTitleModel>();
        }
        /// <summary>
        /// 插进推送订单返回值
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>2015-09-02 王耀发 创建</returns>
        public override int InsertSendOrderReturn(SendOrderReturn entity)
        {
            entity.SysNo = Context.Insert("SendOrderReturn", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }
        /// <summary>
        /// 根据orderId获取订单明细
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>订单明细集合</returns>
        /// <remarks>2013-06-21 王耀发 创建</remarks>
        public override IList<CBSoOrderItem> GetCBSoOrderItemsByOrderSysNo(int orderSysNo)
        {
            var items =
                Context.Select<CBSoOrderItem>("*")
                       .From("SoOrderItem left join PdProduct on SoOrderItem.Productsysno=PdProduct.Sysno")
                       .Where("SoOrderItem.OrderSysNo=@OrderSysNo")
                       .Parameter("OrderSysNo", orderSysNo)
                       .QueryMany();
            return items;
        }

        public override List<CBSoOrderItem> GetCBOrderItemListBySysNos(string SysNos)
        {
            string sql = @" select SoOrderItem.*,PdProduct.EasName,PdProduct.ErpCode ,PdProduct.BarCode
                            ,PdProduct.GrosWeight,PdProduct.NetWeight
                            from SoOrderItem inner join SoOrder on SoOrderItem.OrderSysNo=SoOrder.SysNo 
                            inner join PdProduct on PdProduct.SysNo = SoOrderItem.ProductSysNo ";
            sql += " where SoOrderItem.OrderSysNo in (" + SysNos + ") ";
            return Context.Sql(sql).QueryMany<CBSoOrderItem>();
        }
    }
}
