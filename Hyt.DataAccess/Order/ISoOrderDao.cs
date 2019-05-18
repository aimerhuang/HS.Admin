using System;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Manual;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.Order;
using Hyt.Model.DouShabaoModel;

namespace Hyt.DataAccess.Order
{
    /// <summary>
    /// 订单主表信息数据访问接口
    /// </summary>
    /// <remarks>2013-06-13 杨浩 创建</remarks>
    public abstract class ISoOrderDao : DaoBase<ISoOrderDao>
    {

        /// <summary>
        /// 获取精简订单详情（在线支付改变订单状态使用）
        /// </summary>
        /// <param name="sysno">订单编号</param>
        /// <returns></returns>
        /// <remarks>2017-09-27 杨浩 创建</remarks>
        public abstract SoOrder GetSimplifyOrderInfo(int sysno);
        /// <summary>
        /// 更新支付和在线支付状态
        /// </summary>
        /// <param name="sysno">订单号</param>
        /// <param name="payStatus">支付状态</param>
        /// <param name="onlineStatus">在线支付状态</param>
        /// <param name="status">订单状态</param>
        /// <param name="customsPayStatus">支付推海关状态</param>
        /// <param name="tradeCheckStatus">支付推国检状态</param>
        /// <param name="payTypeSysNo">支付类型系统编号</param>
        /// <remarks>2017-09-27 杨浩 创建</remarks>
        public abstract int UpdatePayStatusAndOnlineStatusAndStatus(int sysno, int payStatus, string onlineStatus, int status, int customsPayStatus, int tradeCheckStatus, int payTypeSysNo);
        /// <summary>
        /// 根据订单编号获取配送单
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-09-09 杨浩 创建</remarks>
        public abstract List<LgDeliveryItem> GetDeliveryItem(int sysNo);
        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="orderStatusText">订单状态文本</param>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        /// <remarks>2016-5-13 杨浩 创建</remarks>
        public abstract int UpdateOrderStatus(int orderSysNo, string orderStatusText, int orderStatus);
         /// <summary>
        /// 更新订单配送方式
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <param name="DeliveryTypeSysNo"></param>
        /// <returns></returns>
        public abstract int UpdateOrderDeliveryType(int orderSysNo, int DeliveryTypeSysNo);
        /// <summary>
        /// 更新订单配送备注
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <param name="DeliveryTypeSysNo"></param>
        /// <returns></returns>
        public abstract int UpdateOrderDeliveryRemarks(int orderSysNo, string DeliveryRemarks);
        /// <summary>
        /// 获取超时为确认收货的订单编号列表
        /// </summary>
        /// <param name="timeOutDay">超时天数</param>
        /// <returns></returns>
        /// <remarks>2016-5-13 杨浩 创建</remarks>
        public abstract IList<SoOrder> GetConfirmReceiptOrderSysNoList(int timeOutDay);

        /// <summary>
        /// 更新订单API执行状态
        /// </summary>
        /// <param name="status">状态值</param>
        /// <param name="type">API类型</param>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2015-12-28 杨浩 创建</remarks>
        public abstract int UpdateOrderApiStatus(int status, int apiType, int orderSysNo);
        /// <summary>
        /// 订单分页查询
        /// </summary>
        /// <param name="pager">分页数据</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        /// <remarks>2013-06-13 朱家宏 创建</remarks>
        public abstract void GetSoOrders(ref Pager<CBSoOrder> pager, ParaOrderFilter filter);

        /// <summary>
        /// 订单分页查询
        /// </summary>
        /// <param name="pager">分页数据</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        /// <remarks>2017-09-18 罗勤要 创建</remarks>
        public abstract void GetSoOrdersNew(ref Pager<CBSoOrder> pager, ParaOrderFilter filter);

        /// <summary>
        /// 订单分页查询
        /// </summary>
        /// <param name="pager">分页数据</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        /// <remarks>2017-09-18 罗勤要 创建</remarks>
        public abstract void GetSoOrdersB2C(ref Pager<CBSoOrder> pager, ParaOrderFilter filter);

         /// <summary>
        /// 获取订单时间搓
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        ///<remarks>2013-11-7 朱成果 创建</remarks>
        public abstract DateTime GetOrderStamp(int SysNo);
        /// <summary>
        /// 根据订单编号获取订单详情
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-9 杨浩 创建</remarks>
        public abstract SoOrder GetOrderByOrderNo(string orderNo);

        /// <summary>
        /// 根据订单编号获取订单详情利嘉模板
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-9 杨浩 创建</remarks>
        public abstract LiJiaOrderModel GetLiJiaOrderByOrderNo(int orderNo);
        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="SysNo">订单编号</param>
        /// <returns></returns>
        ///<remarks>2013-06-13 朱成果 创建</remarks>
        public abstract SoOrder GetEntity(int SysNo);
        /// <summary>
        /// 获取豆沙包签名需要的参数
        /// </summary>
        /// <param name="SysNo">订单编号</param>
        /// <returns></returns>
        public abstract Signparameter GetSignparameter(int SysNo);
        /// <summary>
        /// 获取配送方式，身份证，总价，(运费)，下单时间
        /// </summary>
        /// <param name="SysNo">订单编号</param>
        /// <returns>2017-07-07 罗熙 创建</returns>
        public abstract DouShabaoOrderParameter Getotherparameter(int SysNo);
        /// <summary>
        /// 获取商品列表所需要的参数
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns>2017-07-10 罗熙 创建</returns>
        public abstract ProductList GetProductlist(int SysNo);
        /// <summary>
        /// 获取订单详情列表商品图片
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns>订单商品图片</returns>
        ///<remarks>2016-08-04 罗远康 创建</remarks>
        public abstract IList<PdProductImage> GetOrderProductImgUrl(int SysNo);
        /// <summary>
        /// 仅仅返回订单的时间戳和金额相关的信息
        /// </summary>
        /// <param name="SysNo">订单编号</param>
        /// <returns></returns>
        ///<remarks>2013-06-13 朱成果 创建</remarks>
        public abstract SoOrderAmount GetSoOrderAmount(int SysNo);

        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-29 朱成果  创建</remarks>
        public abstract int GetOrderStatus(int orderId);

        /// <summary>
        /// 更新订单主表
        /// </summary>
        /// <param name="soOrder">订单实体</param>
        /// <returns>t:成功 f:失败</returns>
        /// <remarks>2013-06-14 朱家宏 创建</remarks>
        public abstract bool Update(SoOrder soOrder);

        /// <summary>
        /// 更新订单支付方式
        /// </summary>
        /// <param name="soID">订单编号</param>
        /// <param name="payType">支付方式</param>
        /// <returns>void</returns>
        /// <remarks>2013-12-11 黄波 创建</remarks>
        public abstract void UpdateOrderPayType(int soID,int payType);

        /// <summary>
        /// 更新销售单状态值
        /// </summary>
        /// <param name="orderId">销售单编号</param>
        /// <param name="status">状态值</param>
        /// <returns></returns>
        /// <remarks>2013-06-25 黄志勇  创建</remarks>
        public abstract void UpdateOrderStatus(int orderId,int status);

        /// <summary>
        /// 更新销售单支付时间
        /// </summary>
        /// <param name="orderId">销售单编号</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        /// <remarks>2017-09-15 罗勤尧  创建</remarks>
        public abstract void UpdateOrderPayDte(int orderId, DateTime PayDate);

        /// <summary>
        ///同步销售单支付时间
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-09-15 罗勤尧  创建</remarks>
        public abstract void UpdateAllOrderPayDte();

        /// <summary>
        ///同步指定销售单支付时间
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-09-15 罗勤尧  创建</remarks>
        public abstract void UpdateOrderPayDteById(int orderId);

        /// <summary>
        /// 更新销售单发送状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="SendStatus"></param>
        /// <remarks>2015-09-17 王耀发  创建</remarks>
        public abstract void UpdateOrderSendStatus(int orderId, int SendStatus);

        /// <summary>
        /// 插入销售单主表
        /// </summary>
        /// <param name="entity">销售单主表实体</param>
        /// <returns>销售单主表实体（带编号）</returns>
        /// <remarks>2013-06-27 黄志勇  创建</remarks>
        public abstract SoOrder InsertEntity(SoOrder entity);

        /// <summary>
        /// 插入订单数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <remarks>2016-06-09 王耀发  创建</remarks>
        public abstract SoOrder Inser(SoOrder entity);

        /// <summary>
        /// 根据订单号更新订单前台显示状态
        /// </summary>
        /// <param name="orderID">订单号</param>
        ///  <param name="onlineStatus">前台显示状态</param>
        /// <returns></returns>
        /// <remarks>2013-07-04 朱成果  创建</remarks> 
        public abstract void UpdateOnlineStatusByOrderID(int orderID, string onlineStatus);

        /// <summary>
        /// 根据事物编号更新订单前台显示状态
        /// </summary>
        /// <param name="transactionSysNo">事物编号</param>
        ///  <param name="onlineStatus">前台显示状态</param>
        /// <remarks>2013-07-04 朱成果  创建</remarks> 
        public abstract void UpdateOnlineStatusByTransactionSysNo(string transactionSysNo,string onlineStatus);

        /// <summary>
        /// 出库单分页查询
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>可分页的出库单表</returns>
        /// <remarks>2013-07-04 朱成果  创建</remarks> 
        public abstract Pager<CBOutStockOrder> GetOutStockOrders(ParaOutStockOrderFilter filter);

        /// <summary>
        /// 检查订单是否满足完结条件
        /// </summary>
        /// <param name="SysNo">订单号</param>
        /// <returns></returns>
        /// <remarks>2013-07-04 朱成果  创建</remarks> 
        public abstract bool CheckedOrderFinish(int SysNo);

        /// <summary>
        /// 检查是否满足订单作废的条件
        /// </summary>
        /// <param name="SysNo">订单号</param>
        /// <returns></returns>
        /// <remarks>2013-08-29  朱成果  创建</remarks> 
        public abstract bool CheckedOrderCancel(int SysNo);

        /// <summary>
        /// 更新付款状态
        /// </summary>
        /// <param name="SysNo">订单号</param>
        /// <param name="PayStatus">付款状态</param>
        /// <returns>void</returns>
        /// <remarks>2013-08-29  朱成果  创建</remarks> 
        public abstract void UpdatePayStatus(int SysNo, int PayStatus);

        /// <summary>
        /// 更新发票编号
        /// </summary>
        /// <param name="SysNo">订单编号</param>
        /// <param name="invoiceNo">发票编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-30 朱成果  创建</remarks> 
        public abstract void UpdateInvoiceNo(int SysNo, int invoiceNo);

        /// <summary>
        /// 获取待支付订单数
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>待支付订单数</returns>
        /// <remarks>2013-08-27 周瑜 创建</remarks>
        public abstract int GetUnPaidOrderCount(int customerSysNo);

        /// <summary>
        /// 获取未评论商品数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>未评论商品数量</returns>
        /// <remarks>2013-08-27 周瑜 创建</remarks>
        public abstract int GetUnCommentsCount(int customerSysNo);

        /// <summary>
        /// 通过事物编号来获取订单
        /// </summary>
        /// <param name="value">事务编号</param>
        /// <returns>订单</returns>
        /// <remarks> 2013-09-06 朱家宏 创建</remarks>
        public abstract SoOrder GetByTransactionSysNo(string value);

        /// <summary>
        /// 通过出库单号来获取订单
        /// </summary>
        /// <param name="outStockSysNo">出库单号</param>
        /// <returns>订单</returns>
        /// <remarks> 2013-09-13 郑荣华 创建</remarks>
        public abstract SoOrder GetByOutStockSysNo(int outStockSysNo);

        /// <summary>
        /// 插入订单优惠券关系记录
        /// </summary>
        /// <param name="soCoupon">订单优惠券</param>
        /// <returns></returns>
        /// <remarks>2013-07-30 朱成果  创建</remarks> 
        public abstract SoCoupon InsertSoCoupon(SoCoupon soCoupon);

        /// <summary>
        /// 删除订单优惠券关系记录
        /// </summary>
        /// <param name="couponSysNo">优惠券SysNo.</param>
        /// <param name="orderSysNo">订单SysNo.</param>
        /// <returns></returns>
        /// <remarks>2013-07-30 朱成果  创建</remarks> 
        public abstract void DeleteSoCoupon(int couponSysNo, int orderSysNo);

        /// <summary>
        /// 删除订单所有优惠券
        /// </summary>
        /// <param name="orderSysNo">订单SysNo.</param>
        /// <returns></returns>
        /// <remarks>2013-07-30 朱成果  创建</remarks> 
        public abstract void DeleteSoCoupon(int orderSysNo);

        /// <summary>
        /// 根据订单号获取订单优惠券
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns>订单优惠券集合</returns>
        /// <remarks>2013-09-13 吴文强 创建</remarks>
        public abstract List<SpCoupon> GetCouponByOrderSysNo(int orderSysNo);

        /// <summary>
        /// 根据订单事物编号返回订单号
        /// </summary>
        /// <param name="transactionSysNo">订单事物编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-11 朱成果 创建</remarks>
        public abstract int GetOrderIDByTransactionSysNo(string transactionSysNo);

        /// <summary>
        /// 根据订单编号返回事物编号
        /// </summary>
        /// <param name="orderid">订单号</param>
        /// <returns></returns>
        /// <remarks>2013-09-11 朱成果 创建</remarks>
        public abstract string GetOrderTransactionSysNoByID(int orderid);

        /// <summary>
        /// 销售单统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="filter">分销商权限信息</param>
        /// <param name="infomation">信息对象</param>
        /// <returns>有效的销售单总数</returns>
        /// <remarks>2013-09-26 邵斌 创建</remarks>
        public abstract CBDefaultPageCountInfo GetOrderTotalInformation(System.DateTime startTime, System.DateTime endTime, ParaIsDealerFilter filter, ref CBDefaultPageCountInfo infomation);

        /// <summary>
        /// 销售单统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="infomation">信息对象</param>
        /// <returns>有效的销售单总数</returns>
        /// <remarks>2013-09-26 邵斌 创建</remarks>
        public abstract CBDefaultPageCountInfo GetTodayOrderTotalInformation(System.DateTime startTime,
                                                                             System.DateTime endTime, 
                                                                             ParaIsDealerFilter filter,
                                                                             ref CBDefaultPageCountInfo infomation);

        /// <summary>
        /// 获取预付的订单
        /// </summary>
        /// <returns>返回预付的订单列表</returns>
        /// <remarks>2013-11-8 苟治国 创建</remarks>
        public abstract List<SoOrder> GetClearList();

        /// <summary>
        /// 查询导出订单列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-1-19 王耀发 创建</remarks>
        public abstract List<CBOutputSoOrders> GetExportOrderList(List<int> sysNos);

        /// <summary>
        /// 查询导出订单列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-1-19 王耀发 创建</remarks>
        public abstract List<CBOutputSoOrders> GetExportOrderListByDoSearch(ParaOrderFilter filter);

        /// <summary>
        /// 查询导出订单列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-1-19 王耀发 创建</remarks>
        public abstract List<CBOutputSoOrders> GetExportOrderListByDoComplexSearch(ParaOrderFilter filter);

        /// <summary>
        /// 设置门店订单搜索分页
        /// </summary>
        /// <param name="pager">分页</param>
        /// <returns></returns>
        /// <remarks>2015-10-14 杨云奕 添加</remarks>
        public abstract SoOrderMods GetSoOrderMods(int soOrderSysNo);

        /// <summary>
        /// 获取订单地址信息
        /// </summary>
        /// <param name="ReceiveAddressSysNo">订单地址编号</param>
        /// <returns></returns>
        /// <remarks>2015-10-16 杨云奕 添加</remarks>
        public abstract SoReceiveAddressMod GetOrderReceiveAddress2(int ReceiveAddressSysNo);

        /// <summary>
        /// 更新订单广州机场商检状态
        /// </summary>
        /// <param name="soID">订单编号</param>
        /// <param name="GZJCStatus">状态</param>
        /// <returns></returns>
        /// <remarks>2016-4-6 王耀发 创建</remarks>
        public abstract void UpdateOrderGZJCStatus(int soID, int GZJCStatus);

        /// <summary>
        /// 更新订单南沙商检状态
        /// </summary>
        /// <param name="soID">订单编号</param>
        /// <param name="NsStatus">状态</param>
        /// <returns></returns>
        /// <remarks>2016-4-6 王耀发 创建</remarks>
        public abstract void UpdateOrderNsStatus(int soID, int NsStatus);

        /// <summary>
        /// 所有时间段的订单信息
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public abstract List<CBSoOrder> GetAllOrderByDateTime(DateTime startTime, DateTime endTime);

        public abstract List<EntityStatisticMod> GetEntityStatisticDataList(int? defaultWareSysNo, DateTime? startTime, DateTime? endTime, string type);

        /// <summary>
        /// 通过订单编号集合获取订单明细信息
        /// </summary>
        /// <param name="SysNos">编号集合</param>
        /// <returns></returns>
        /// <remarks>2016-06-02 杨云奕 添加</remarks>
        public abstract List<CBSoOrderItem> GetOrderItemsByOrderId(int[] SysNos);

        /// <summary>
        /// 获取订单的最原始订单编号，换货订单返回原始订单号，非换货订单就返回当前订单编号
        /// </summary>
        /// <param name="currectorderid">当前订单</param>
        /// <returns></returns>
        /// <remarks>2015-04-29 杨浩 创建</remarks>
        public abstract int GetOriginalOrderID(int currectorderid);
        /// <summary>
        /// 根据订单系统编号列表和状态获取订单列表
        /// </summary>
        /// <param name="sysNos">订单系统编号集（多个逗号分隔）</param>
        /// <param name="status">订单状态</param>
        /// <returns></returns>
        /// <remarks>2016-6-26 杨浩 创建</remarks>
        public abstract List<SoOrder> GetOrderListBySysNosAndStatus(string sysNos, int status);
        /// <summary>
        /// 订单商品毛重总和
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-7-25 杨浩 创建</remarks>
        public abstract decimal TotalOrderProductWeight(int orderSysNo);

        #region 查询导出订单数据（用于信营）
        /// <summary>
        /// 查询导出订单列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-8-4 罗远康 复制添加导出图片</remarks>
        public abstract void GetXXExportOrderList(ref Pager<CBXXOutputSoOrders> pager, List<int> sysNos);

        /// <summary>
        /// 查询导出订单列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-8-4 罗远康 复制添加导出图片</remarks>
        public abstract void GetXXExportOrderListByDoSearch(ref Pager<CBXXOutputSoOrders> pager, ParaOrderFilter filter);

        /// <summary>
        /// 查询导出订单列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-8-4 罗远康 复制添加导出图片</remarks>
        public abstract void GetXXExportOrderListByDoComplexSearch(ref Pager<CBXXOutputSoOrders> pager, ParaOrderFilter filter);
        #endregion

        #region 又一城订单添加推送
        /// <summary>
        /// 根据订单号查询是否提推送过又一城详情
        /// </summary>
        /// <param name="OrderSysNo"></param>
        /// <returns></returns>
        public abstract SoAddOrderToU1City GetU1CityEntity(int OrderSysNo);
        
        /// <summary>
        /// 添加一条记录状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract int InserU1City(SoAddOrderToU1City entity);

        /// <summary>
        /// 添加推送又一城返回参数记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract SoOrderToU1CityInfomation InserToU1CityInfomation(SoOrderToU1CityInfomation entity);

        /// <summary>
        /// 查询是否存在记录
        /// </summary>
        /// <param name="ProductSysNo"></param>
        /// <param name="OrderSysNo"></param>
        /// <returns></returns>
        public abstract int GetToU1CityInfomation(string TransactionPdSku, int OrderSysNo);
          /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="OrderSysNo"></param>
        /// <returns></returns>
        public abstract SoReturnOrderToU1City GetReturnOrderToU1City(int OrderSysNo);


        #endregion

         /// <summary>
        /// 获取物流单号及物流编码
        /// </summary>
        /// <param name="ordersysno"></param>
        /// <returns></returns>
        /// <remarks>2015-10-23 陈海裕 创建</remarks>
        public abstract Hyt.Model.Common.LgExpressModel GetDeliveryCodeData(int ordersysno);

        public abstract List<SoOrder> GetAllOrderBySysNos(string SysNos);

        /// <summary>
        /// 更新订单表订单积分
        /// </summary>
        /// <param name="OrderSysNo"></param>
        /// <param name="point"></param>
        public abstract void UpdateOrderPoint(int OrderSysNo, int point);

        /// <summary>
        /// 获取订单中销售的仓库商品情况
        /// </summary>
        /// <param name="warehouseSysNo"></param>
        /// <param name="productSysNo"></param>
        /// <returns></returns>
        public abstract List<CDSoOrderItem> GetSoOrderItemByWarehouseProduct(int warehouseSysNo, int productSysNo);

        /// <summary>
        /// 是否全部发货
        /// </summary>
        /// <param name="orderSysno">订单系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-08-24 杨浩 创建</remarks>
        public abstract bool IsAllShip(int orderSysno);


        /// <summary>
        /// 获取销售订单详情
        /// </summary>
        /// <param name="SysNo">订单编号</param>
        /// <returns></returns>
        ///<remarks>2017-08-25 吴琨 创建</remarks>
        public abstract WhStockOut GetEntityTo(int SysNo);

        /// <summary>
        /// 查询销售单表
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-08-25 吴琨 创建</returns>
        public abstract SoOrder GetModel(int sysNo);



        /// <summary>
        /// 根据订单编号修改支付方式
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract bool UpPayTypeSysNo(int id, int PayTypeSysNo);
        
    }
}
