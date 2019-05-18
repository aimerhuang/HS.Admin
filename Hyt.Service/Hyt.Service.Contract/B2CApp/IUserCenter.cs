using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Contract.Base;

namespace Hyt.Service.Contract.B2CApp
{
    /// <summary>
    /// 个人中心管理接口相关
    /// </summary>
    /// <remarks> 2013-7-1 杨浩 创建</remarks>
    [ServiceKnownType(typeof(Result))]
    [ServiceContract]
    public interface IUserCenter : IBaseServiceContract
    {
        #region 地址管理

        /// <summary>
        /// 新增用户收货地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns>返回新地址的系统号</returns>
        /// <remarks>2013-8-1 杨浩 创建</remarks>
        [WebInvoke(UriTemplate = "/Address/Add", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result<int> AddAddress(CrReceiveAddress address);

        /// <summary>
        /// 更新收货地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        /// <remarks>2013-8-1 杨浩 创建</remarks>
        [WebInvoke(UriTemplate = "/Address/Update", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result UpdateAddress(CrReceiveAddress address);

        /// <summary>
        /// 删除收货地址
        /// </summary>
        /// <param name="sysNo">地址编号</param>
        /// <returns></returns>
        /// <remarks>2013-8-1 杨浩 创建</remarks>
        [WebInvoke(UriTemplate = "/Address/Delete", Method = "DELETE", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result DeleteAddress(int sysNo);

        /// <summary>
        /// 设置默认收货地址
        /// </summary>
        /// <param name="sysNo">地址编号</param>
        /// <returns></returns>
        /// <remarks>2013-8-1 杨浩 创建</remarks>
        [WebInvoke(UriTemplate = "/Address/SetDefault", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result SetDefaultAddress(int sysNo);

        #endregion

        #region Home

        /// <summary>
        /// 上传用户头像
        /// </summary>
        /// <param name="imgBase64"></param>
        /// <returns></returns>
        /// <remarks>杨浩 添加</remarks>
        [WebInvoke(UriTemplate = "/Home/UploadAvatar", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result UploadAvatar(string imgBase64);

        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-8-29 杨浩 添加</remarks>
        [WebGet(UriTemplate = "/Home/GetAvatar",ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result<string> GetAvatar();

        /// <summary>
        /// 获取用户中心首页的所有提示数
        /// </summary>
        /// <returns></returns>
        /// <remarks>杨浩 添加</remarks>
        [WebGet(UriTemplate = "/Home/AllTips", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [CustomOperationBehavior]
        Result<Tips> GetAllTips();

        #endregion

        #region Product

        /// <summary>
        /// 晒单或评价商品列表
        /// </summary>
        /// <param name="filter">filter</param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/Product/ShowOrComment?filter={filter}&pageIndex={pageIndex}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [CustomOperationBehavior]
        ResultPager<IList<ShowOrComment>> GetProductShowOrComment(int filter, int pageIndex);

        /// <summary>
        /// 获取用户关注商品
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/Product/Attention?pageIndex={pageIndex}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        ResultPager<IList<AttentionProduct>> GetAttentionProduct(int pageIndex);

        /// <summary>
        /// 删除用户关注商品
        /// </summary>
        /// <param name="sysNo">关注系统编号</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/Product/Attention/Delete", Method = "DELETE", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result DeleteAttentionProduct(int sysNo);

        /// <summary>
        /// 添加商品关注(收藏)
        /// </summary>
        /// <param name="productSysNo">商品系统号</param>
        /// <returns></returns>
        [CustomOperationBehavior]
        [WebInvoke(UriTemplate = "/Product/Attention/Add", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result AddAttention(int productSysNo);

        #endregion 

        #region Other

        /// <summary>
        /// 获取用户消息
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        ResultPager<IList<CrSiteMessage>> GetMessages(int pageIndex);

        #endregion

        #region 用户订单

        /// <summary>
        /// 用户订单列表
        /// </summary>
        /// <param name="filter">状态：5（All）、配送中（10）、待支付（20）待评价(30)、退换货[已结算](40)</param>
        /// <param name="month">1:一个月内订单,-1:一个月前订单</param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        /// <remarks>杨浩 添加</remarks>
        [WebGet(UriTemplate = "/Order/List/?filter={filter}&month={month}&pageIndex={pageIndex}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        ResultPager<IList<Order>> GetAllOrders(AppEnum.OrderFilter filter, int month, int pageIndex);

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="sysNo">订单系统编号</param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/Order/Details/{sysNo}/", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result<OrderDetail> GetOrderDetail(string sysNo);

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="sysNo">订单系统编号</param>
        /// <returns></returns>
        [WebInvoke( Method="POST", UriTemplate = "/Order/Cancel/{sysNo}/", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result CancelOrder(string sysNo);

        /// <summary>
        /// 获取订单去支付信息
        /// </summary>
        /// <param name="sysNo">订单系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-16 杨浩 创建</remarks>
        [WebGet(UriTemplate = "/Order/PayResult/{sysNo}/", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result<OrderResult> GetOrderPayResult(string sysNo);

        #endregion

        #region 退换货

        /// <summary>
        /// 获取可退换列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-8-13 杨浩 创建</remarks>
        [CustomOperationBehavior]
        [WebGet(UriTemplate = "/Returns/List", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        ResultPager<IList<ReturnDetail>> GetReturnsList();

        /// <summary>
        /// 获取可选仓库
        /// </summary>
        /// <param name="stockOutSysNo">出库单编号</param>
        /// <param name="pickupTypeSysNo">取件方式编号</param>
        /// <returns></returns>
        /// <remarks>2013-8-13 杨浩 创建</remarks>
        [CustomOperationBehavior]
        [WebGet(UriTemplate = "/Returns/Warehouse/{stockOutSysNo}?pickupTypeSysNo={pickupTypeSysNo}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<Warehouse>> GetWarehouse(string stockOutSysNo, int pickupTypeSysNo);

        /// <summary>
        /// 新建换货单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-8-15 杨浩 创建</remarks>
        [CustomOperationBehavior]
        [WebInvoke(UriTemplate = "/Returns/AddExchanges/", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result AddExchanges(ExchangeOrders exchange);

        /// <summary>
        /// 新建退货单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-8-15 杨浩 创建</remarks>
        [CustomOperationBehavior]
        [WebInvoke(UriTemplate = "/Returns/RejectedOrders/", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result AddRejectedOrders(RejectedOrders rejectedOrders);

        /// <summary>
        /// 获取取件方式
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-9-12 杨浩 创建</remarks>
        [CustomOperationBehavior]
        [WebGet(UriTemplate = "/Returns/PickupType/", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<RePickupType>> GetPickupType();
        
        /// <summary>
        /// 根据订单号获取可退换信息
        /// </summary>
        /// <param name="stockOutSysNo">出库单编号</param>
        /// <returns></returns>
        /// <remarks>2013-8-15 杨浩 创建</remarks>
        [WebGet(UriTemplate = "/Returns/Info/{stockOutSysNo}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result<ReturnDetail> GetReturnInfo(string stockOutSysNo);

        /// <summary>
        /// 查看退换货历史
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-9-13 杨浩 创建</remarks>
        [WebGet(UriTemplate = "/Returns/History/", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result<IList<ReturnHistory>> GetReturnHistory();

        /// <summary>
        /// 查看退换货进度
        /// </summary>
        /// <param name="sysNo">退换货编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-13 杨浩 创建</remarks>
        [WebGet(UriTemplate = "/Returns/Schedule/{sysNo}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result<ReturnHistorySub> GetReturnSchedule(string sysNo);

        #endregion

        #region 订单查询

        /// <summary>
        /// 获取订单日志
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-12 杨浩 创建</remarks>
        [CustomOperationBehavior]
        [WebGet(UriTemplate = "/Order/Log/{orderSysNo}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<TransactionLog> GetTransactionLog(string orderSysNo);
        

        #endregion

        #region 优惠券

        /// <summary>
        /// 获取用户优惠券
        /// </summary>
        /// <param name="status">status: 20 已审核(当前可用)，30 已使用</param>
        /// <returns></returns>
        /// <remarks> 2013-9-16 杨浩 创建</remarks>
        [CustomOperationBehavior]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<Coupon>> GetCoupons(PromotionStatus.优惠券状态 status);

        #endregion
    }

    #region Model

    public class Tips
    {
        /// <summary>
        /// 待付款数量
        /// </summary>
        public int ObligationNum { get; set; }

        /// <summary>
        /// 商品关注数
        /// </summary>
        public int AttentionNum { get; set; }

        /// <summary>
        /// 待评价数
        /// </summary>
        public int NoEvaluationNum{ get; set; }

        /// <summary>
        /// 消息数量
        /// </summary>
        public int MessageNum { get; set; }

        /// <summary>
        /// 优惠券数
        /// </summary>
        public int CouponNum { get; set; }

        /// <summary>
        /// 配送中数
        /// </summary>
        public int DeliveriesNum { get; set; }
    }

    #endregion
}
