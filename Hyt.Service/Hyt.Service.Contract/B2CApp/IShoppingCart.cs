using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Service.Contract.Base;

namespace Hyt.Service.Contract.B2CApp
{
    /// <summary>
    /// 购物车接口相关
    /// </summary>
    /// <remarks>杨浩 2013-7-1 创建</remarks>
    [ServiceKnownType(typeof(Result))]
    [ServiceContract]
    public interface IShoppingCart : IBaseServiceContract
    {
        /// <summary>
        /// 获取购物车数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>购物车数量</returns>
        /// <remarks>2013-10-10 杨浩 创建</remarks>
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        Result<int> GetShoppingQuantity(int customerSysNo);

        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-08-30 杨浩 创建</remarks>
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result<ShoppingCartApp> GetShoppingCart(int customerSysNo);

        /// <summary>
        /// 添加商品到购物车
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="quantity">数量</param>
        /// <returns></returns>
        /// <remarks>2013-08-30 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result AddToCart(int customerSysNo, int productSysNo, int quantity);

        /// <summary>
        /// 添加组合或团购商品到购物车
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupSysNo">组系统编号(组合,团购主表系统编号)</param>
        /// <param name="quantity">组数量</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-01 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        [OperationContractAttribute(Name = "AddGroupToCart")]
        Result AddToCart(int customerSysNo, int groupSysNo, int quantity, int promotionSysNo);
       
        /// <summary>
        /// 添加促销赠品至购物车
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <remarks>2013-09-3 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result<ShoppingCartApp> AddGift(int customerSysNo, int productSysNo, int promotionSysNo);

        /// <summary>
        /// 选择购物车明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <remarks>2013-09-3 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result<ShoppingCartApp> CheckedItem(int customerSysNo, int[] sysNo);

        /// <summary>
        /// 取消选择购物车明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <remarks>2013-09-3 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result<ShoppingCartApp> UncheckedItem(int customerSysNo, int[] sysNo);

        /// <summary>
        /// 更新购物车商品数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号集合</param>
        /// <param name="quantity">商品数量</param>
        [CustomOperationBehavior]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<ShoppingCartApp> UpdateQuantity(int customerSysNo, int[] sysNo, int quantity);

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车编号</param>
        /// <returns>返回总价信息</returns>
        /// <remarks> 2013-7-8 杨浩 创建</remarks>
        [CustomOperationBehavior]
        [WebInvoke(Method = "DELETE", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<ShoppingCartApp> Remove(int customerSysNo, int[] sysNo);

        /// <summary>
        /// 删除促销赠品
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <remarks>2013-08-13 杨浩 创建</remarks>
        [CustomOperationBehavior]
        [WebInvoke(Method = "DELETE", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<ShoppingCartApp> RemoveGift(int customerSysNo, int productSysNo, int promotionSysNo);

        /// <summary>
        /// 获取用户收货地址
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <returns></returns>
        /// <remarks> 2013-7-12 杨浩 创建</remarks>
        [CustomOperationBehavior]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<ReceiveAddress>> GetShippingAddress(int customerSysNo);

        /// <summary>
        /// 获取用户优惠券
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <returns></returns>
        /// <remarks> 2013-7-12 杨浩 创建</remarks>
        [CustomOperationBehavior]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<Coupon>> GetCoupons(int customerSysNo);

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <returns></returns>
        /// <param name="receiveAddressSysNo">收货地址编号</param>
        /// <remarks> 2013-7-12 杨浩 创建</remarks>
        [CustomOperationBehavior]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<PaymentType>> GetPaymentType(int receiveAddressSysNo);

        /// <summary>
        /// 获取商品清单
        /// </summary>
        /// <returns></returns>
        /// <param name="customerSysNo">用户编号</param>
        /// <remarks> 2013-9-5 杨浩 创建</remarks>
        [CustomOperationBehavior]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<SimplProductItem>> GetProducts(int customerSysNo);

        /// <summary>
        /// 获取可选自提门店
        /// </summary>
        /// <param name="areaSysNo">地区编码</param>
        /// <returns></returns>
        /// <remarks> 2013-9-5 杨浩 创建</remarks>
        [CustomOperationBehavior]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<NWarehouse>> GetShop(int areaSysNo);

        /// <summary>
        /// 获取配送时间
        /// </summary>
        /// <returns></returns>
        /// <remarks> 2013-9-5 杨浩 创建</remarks>
        [CustomOperationBehavior(false)]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<string>> GetDeliveryTime();

        /// <summary>
        /// 配送方式
        /// </summary>
        /// <param name="receiveAddressSysNo">收货地址编号</param>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result<IList<DeliveryType>> GetDeliveryType(int receiveAddressSysNo);

        /// <summary>
        /// 选择地址和配送方式后计算金额
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="areaSysNo">区域编号</param>
        /// <param name="deliveryTypeSysNo">配送方式编号</param>
        /// <param name="couponCode">优惠劵码</param>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result<ShoppingAmount> CalculateAmount(int customerSysNo, int areaSysNo, int deliveryTypeSysNo, string couponCode);

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="order">新建订单模型</param>
        /// <returns></returns>
        /// <remarks>2013-8-15 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior]
        Result<OrderResult> CreateOrder(CreateOrder order);
    }
}
