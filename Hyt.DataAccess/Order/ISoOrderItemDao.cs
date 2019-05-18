using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.SellBusiness;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.DataAccess.Base;
namespace Hyt.DataAccess.Order
{
    /// <summary>
    /// 订单明细
    /// </summary>
    /// <remarks>2013-06-21 朱家宏 创建</remarks>
    public abstract class ISoOrderItemDao : DaoBase<ISoOrderItemDao>
    {
        /// <summary>
        /// 根据orderId获取订单明细
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>订单明细集合</returns>
        /// <remarks>2013-06-21 朱家宏 创建</remarks>
        public abstract IList<SoOrderItem> GetOrderItemsByOrderSysNo(int orderSysNo);

        /// <summary>
        /// 根据用户系统号获取订单明细
        /// </summary>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>订单明细集合</returns>
        /// <remarks>2013-08-16 杨晗 创建</remarks>
        public abstract IList<SoOrderItem> GetOrderItemsByCustomerSysNo(int customerSysNo);

        /// <summary>
        /// 根据orderId获取订单明细
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>订单明细(包括商品ERP code）集合</returns>
        /// <remarks>2013-07-26 朱成果 创建</remarks>
        public abstract IList<CBSoOrderItem> GetOrderItemsWithErpCodeByOrderSysNo(int orderSysNo);
     
        /// <summary>
        /// 插入销售单明细
        /// </summary>
        /// <param name="item">销售单明细</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public abstract int Insert(SoOrderItem item);

        /// <summary>
        /// 获取订单商品明细
        /// </summary>
        /// <param name="sysNo">明细编号</param>
        /// <returns>订单商品明细</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public abstract SoOrderItem GetEntity(int sysNo);

         /// <summary>
         /// 删除订单商品明细
         /// </summary>
        /// <param name="sysNo">明细编号</param>
        /// <returns></returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public abstract void Delete(int sysNo);
        /// <summary>
        /// 删除订单明细
        /// </summary>
        /// <param name="OrderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-07-13 王耀发 创建</remarks>
        public abstract void DeleteByOrderSysNo(int OrderSysNo);
        /// <summary>
        /// 更新订单商品明细数量
        /// </summary>
        /// <param name="sysNo">明细编号</param>
        /// <param name="quantity">数量</param>
        /// <param name="changeAmount">调价金额</param>
        /// <returns></returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        /// <remarks>2013-09-17 黄志勇 修改</remarks>
        public abstract void UpdateOrderItemQuantity(int sysNo, int quantity, decimal changeAmount);

        /// <summary>
        /// 更新销售单出库数量
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <param name="quantity">出库数量</param>
        /// <returns> </returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public abstract void UpdateOutStockQuantity(int sysNo, int quantity);

         /// <summary>
        /// 同步订单总价(返回订单应收款金额)
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns> 返回订单应收款金额</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public abstract  decimal SynchronousOrderAmount(int orderId);

        /// <summary>
        /// 获取晒单评价中当前客户已完成的销售单明细列表
        /// </summary>
        /// <param name="pageIndex">起始页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="customerSysNo">用户系统号</param>
        /// <param name="count">抛出总数</param>
        /// <returns>销售单明细列表</returns>
        /// <remarks>2013-08-15 杨晗 创建</remarks>
        public abstract IList<CBFeCommentList> GetFeCommentList(int pageIndex, int pageSize, int customerSysNo, out int count);

        /// <summary>
        /// 获取晒单评价中当前客户已完成并且没晒单的销售单明细列表
        /// </summary>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>销售单明细列表</returns>
        /// <remarks>2013-08-15 杨晗 创建</remarks>
        public abstract IList<CBFeCommentList> GetFeCommentList(int customerSysNo);

        /// <summary>
        /// 获取升仓赠品列表
        /// </summary>
        /// <param name="orderid">订单编号</param>
        /// <returns>升仓赠品列表</returns>
        /// <remarks>2014-07-03 朱成果 创建</remarks>
        public abstract IList<CBSoOrderItem> GetMallGiftItems(int orderid);

        /// <summary>
        /// 获得推送订单需要的参数信息
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns>2015-09-02 王耀发 创建</returns>
        public abstract IList<SendSoOrderModel> GetSendSoOrderModelByOrderSysNo(int orderSysNo);

        /// <summary>
        /// 获得推送订单需要的参数信息
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns>2015-09-02 王耀发 创建</returns>
        public abstract SendSoOrderTitleModel GetSendSoOrderTitleModelByOrderSysNo(int orderSysNo);

        /// <summary>
        /// 插进推送订单返回值
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>2015-09-02 王耀发 创建</returns>
        public abstract int InsertSendOrderReturn(SendOrderReturn entity);

        /// <summary>
        /// 根据orderId获取订单明细
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>订单明细集合</returns>
        /// <remarks>2013-06-21 王耀发 创建</remarks>
        public abstract IList<CBSoOrderItem> GetCBSoOrderItemsByOrderSysNo(int orderSysNo);

        /// <summary>
        /// 获取订单明细
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>2016-1-7 王耀发 创建</remarks>
        public abstract Pager<CBSoOrderItem> GetOrderItemsRecordList(ParaOrderItemRecordFilter filter);

        public abstract List<CBSoOrderItem> GetCBOrderItemListBySysNos(string SysNos);
    }

}
