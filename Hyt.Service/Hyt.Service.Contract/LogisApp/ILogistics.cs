using System.Collections.Generic;
using Hyt.Model;
using System.ServiceModel;
using System.ServiceModel.Web;
using Hyt.Model.LogisApp;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Contract.Base;
using System;

namespace Hyt.Service.Contract.LogisApp
{

    /// <summary>
    /// 表示与“”相关的应用层服务契约。
    /// </summary>
    //[ServiceContract(SessionMode = SessionMode.Required)]
    [ServiceContract]
    public interface ILogistics : IBaseServiceContract
    {

        #region 配送员 (周唐炬)

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account">用户名</param>
        /// <param name="password">密码.</param>
        /// <returns>返回结果</returns>
        /// <remarks>
        /// 2013-6-13 何方 创建
        /// </remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        //[Description("物流APP登录")]
        Result Login(string account, string password);

        /// <summary>
        /// </summary>
        /// <param name="oldPassword">旧密码.</param>
        /// <param name="newPassword">新密码.</param>
        /// <returns>返回结果</returns>
        /// <remarks>
        /// 2013-6-13 何方 创建
        /// </remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result ModifyPassword(string oldPassword, string newPassword);

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <remarks>2013-06-19 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result LogOut();

        /// <summary>
        /// 更新GPS位置数据
        /// </summary>
        /// <param name="latitude">纬度</param>
        /// <param name="longitude">经度</param>
        /// <param name="gpsDate">定位时间</param>
        /// <param name="locationType">定位类型</param>
        /// <param name="radius">误差</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-06-24 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result UpdateGpsForJson(decimal latitude, decimal longitude, string gpsDate, int locationType, float radius);

        /// <summary>
        /// 批量更新GPS位置数据
        /// </summary>
        /// <param name="list">批量GPS数据</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-06-24 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result BatchUpdateGpsForJson(List<APPCBLgDeliveryUserLocation> list);

        /// <summary>
        /// APP上传图片保存
        /// </summary>
        /// <param name="noteType">单据类型{出库单/取件单</param>
        /// <param name="noteSysNo">单据系统编号</param>
        /// <param name="imgBase64">图片Base64编码</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-06-24 周唐炬 创建</remarks>     
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result UploadSign(int noteType, int noteSysNo, string imgBase64);
        #endregion

        #region 会员信息 (周唐炬)
        /// <summary>
        /// 获取某地地区的子地区数据
        /// </summary>
        /// <param name="areaSysNo">地区父级系统号</param>
        /// <returns>省市区数据</returns>
        /// <remarks> 2013-07-10 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<CBBsArea>> SelectArea(int areaSysNo);

        /// <summary>
        /// 添加会员
        /// </summary>
        /// <param name="customerInfo">会员信息</param>
        /// <returns>返回结果</returns>
        /// <remarks> 2013-07-10 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result<int> CreateCustomer(AppCBCrCustomer customerInfo);

        /// <summary>
        /// 查询会员(模糊查询)
        /// </summary>
        /// <param name="keyword">姓名、手机号</param>
        /// <returns>会员列表</returns>
        /// <remarks>2013-07-10 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<AppCBCrCustomer>> SearchCustomer(string keyword);

        /// <summary>
        /// 添加会员收货地址
        /// </summary>
        /// <param name="receiveAddress">会员收货地址</param>
        /// <returns>返回结果</returns>
        /// <remarks>2014-02-28 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result ReceiveAddressCreate(AppCrReceiveAddress receiveAddress);

        /// <summary>
        /// 获取用户的收货地址
        /// </summary>
        /// <returns>用户的收货地址列表</returns>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <remarks> 2013-07-10 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<AppCrReceiveAddress>> GetCustomerAddressList(int customerSysNo);

        #endregion

        #region 商品 (周唐炬)

        /// <summary>
        /// 通过客户系统编号获取借货单商品列表
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>取借货单商品列表</returns>
        /// <remarks>2013-07-11 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<CBPdProductJson>> GetProductLendItmeList(int customerSysNo);
        /// <summary>
        /// 返回所有的商品分类
        /// </summary>
        /// <returns>所有的商品分类</returns>
        /// <remarks>2013-07-30 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<AppPdCategory>> GetProductCategoryList();

        /// <summary>
        /// 获取所有的分类下商品列表
        /// </summary>
        /// <param name="categorySysNo">商品分类系统编号</param>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="keyword">查询关键字(ERP商品编号,商品名称)</param>
        /// <param name="currentPageIndex">当前索引</param>
        /// <param name="pageSize">每页显示数</param>
        /// <returns>所有的商品分类</returns>
        /// <remarks>2013-07-30 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<Pager<AppProduct>> GetProductList(int? categorySysNo, int? customerSysNo, string keyword, int currentPageIndex, int pageSize);

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <returns>返回商品详情</returns>
        ///  <remarks>2013-07-30 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<AppProduct> GetProductDetails(int productSysNo, int customerSysNo);


        /// <summary>
        /// 获取产品最低价格（业务员销售限价）
        /// </summary>
        /// <param name="productSysnos">产品id字符串  逗号分隔 </param>
        /// <returns>
        /// [
        ///     {SysNo:111,Price:xxx},
        /// ]
        /// </returns>
        ///  <remarks>2014-09-16 杨文兵 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<String> SelectProductLowestPrice(string productSysnos);

        /// <summary>
        /// 获取产品价格
        /// </summary>
        /// <param name="productSysnos">产品id字符串  逗号分隔 </param>
        /// <returns>
        /// [
        ///     {SysNo:111,PriceList:[{Name:'初级',Price:xxx},{Name:'中级',Price:xxx},{Name:'高级',Price:xxx},{Name:'最低价',Price:xxx}]},
        ///     {SysNo:112,PriceList:[{Name:'初级',Price:xxx},{Name:'中级',Price:xxx},{Name:'高级',Price:xxx},{Name:'最低价',Price:xxx}]},
        /// ]
        /// </returns>
        ///  <remarks>2014-09-16 杨文兵 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<String> SelectProductPrice(string productSysnos);

        #endregion

        #region 购物车 (周唐炬)

        /// <summary>
        /// 添加一个或多个商品至购物车
        /// </summary>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="products">商品系统编号集合</param>
        /// <param name="shopCartSource">购物车商品来源</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>当前当前添加商品的购物车信息对象(包括促销信息)</returns>
        /// <remarks>2013-07-30 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> AddProductsToShopCart(int? customerSysNo, IList<int> products, Model.WorkflowStatus.CustomerStatus.购物车商品来源 shopCartSource, bool isReturn);

        /// <summary>
        /// 将组促销转换为购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupSysNo">组系统编号(组合,团购主表系统编号)</param>
        /// <param name="quantity">组数量</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="shopCartSource">购物车商品来源</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> ConvertGroupToShopCartItems(int customerSysNo, int groupSysNo, int quantity,
                                                           int promotionSysNo, Model.WorkflowStatus.CustomerStatus.购物车商品来源 shopCartSource, bool isReturn);

        /// <summary>
        /// 添加促销赠品至购物车
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> AddGiftToShopCart(int customerSysNo, int productSysNo, int promotionSysNo, Model.WorkflowStatus.CustomerStatus.购物车商品来源 source, bool isReturn);

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> RemoveShopCartItems(int customerSysNo, int[] sysNo, bool isReturn);

        /// <summary>
        /// 删除购物车组商品
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> RemoveShopCartGroup(int customerSysNo, string groupCode, string promotionSysNo, bool isReturn);

        /// <summary>
        /// 删除促销赠品
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> RemoveGift(int customerSysNo, int productSysNo, int promotionSysNo, bool isReturn);

        /// <summary>
        /// 删除购物车所有明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <remarks>2013-09-24 沈强 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result RemoveAll(int customerSysNo);

        /// <summary>
        /// 删除购物车选中的明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-24 沈强 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> RemoveCheckedItem(int customerSysNo, bool isReturn);

        /// <summary>
        /// 更新购物车明细商品数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号集合</param>
        /// <param name="quantity">商品数量</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> UpdateItemsQuantity(int customerSysNo, int[] sysNo, int quantity, bool isReturn);

        /// <summary>
        /// 更新购物车组组商品数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> UpdateGroupQuantity(int customerSysNo, string groupCode, string promotionSysNo,
                                                          int quantity, bool isReturn);

        /// <summary>
        /// 选择购物车所有明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        /// [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> CheckedAll(int customerSysNo, bool isReturn);

        /// <summary>
        /// 取消选择购物车所有明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> UncheckedAll(int customerSysNo, bool isReturn);

        /// <summary>
        /// 选择购物车明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> CheckedItem(int customerSysNo, int[] itemSysNo, bool isReturn);

        /// <summary>
        /// 取消选择购物车明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> UncheckedItem(int customerSysNo, int[] itemSysNo, bool isReturn);

        /// <summary>
        /// 选择购物车组明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> CheckedGroupItem(int customerSysNo, string groupCode, string promotionSysNo, bool isReturn);

        /// <summary>
        /// 取消选择购物车组明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-16 沈强 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> UncheckedGroupItem(int customerSysNo, string groupCode, string promotionSysNo, bool isReturn);

        /// <summary>
        /// 获取当前用户所有的购物车信息
        /// </summary>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <returns>当前用户购物车信息(包括促销信息)</returns>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部</param>
        /// <remarks>2013-07-30 周唐炬 创建</remarks>
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> GetShopCart(int customerSysNo, bool isChecked);

        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号(null:购物车为服务器选中的对象)</param>
        /// <param name="areaSysNo">区域系统编号</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="promotionCode">促销代码</param>
        /// <param name="couponCode">优惠券代码</param>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-09-16 沈强 修改</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> GetShoppingCart(int customerSysNo, int[] sysNo, int? areaSysNo,
                                                      int? deliveryTypeSysNo, string promotionCode, string couponCode,
                                                      bool isChecked);

        /// <summary>
        /// 获取当前购物车有效可使用的优惠券
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <returns>返回有效可使用的优惠券集合</returns>
        /// <remarks>2013-09-28 沈强 修改</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<List<AppSpCoupon>> GetCurrentCartValidCoupons(int customerSysNo);

        /// <summary>
        /// App提交订单
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="receiveAddress">收货地址对象</param>
        /// <param name="order">App订单信息</param>
        /// <returns></returns>
        /// <remarks>2013-09-29 沈强 修改</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<AppSoOrder> CreatedOrder(string cacheKey, AppSoReceiveAddress receiveAddress, AppShopCartOrder order);


         /// <summary>
         /// APP 下单（带促销）到待结算
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="receiveAddress">收货地址对象</param>
         /// <param name="order">App订单信息</param>
        /// <returns></returns>
        /// <remarks>2014-09-29 朱成果 修改</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<AppSoOrder> CreatedOrderAndDelivery(string cacheKey, AppSoReceiveAddress receiveAddress, AppShopCartOrder order);

        /// <summary>
        /// App提交订单cache
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="receiveAddress">收货地址对象</param>
        /// <param name="order">App订单信息</param>
        /// <returns>返回订单</returns>
        /// <remarks>2014-03-03 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<AppSoOrder> CreateOrderCache(string cacheKey, AppSoReceiveAddress receiveAddress, AppShopCartOrder order);

        /// <summary>
        /// App订单作废
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-11 沈强 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result CancelSoOrder(int orderSysNo);
        #endregion

        #region 订单 (周唐炬)

        /// <summary>
        /// 创建业务员调价APP订单 到结算步骤
        /// </summary>
        /// <param name="order">APP订单</param>
        /// <returns>
        /// 返回结果
        /// </returns>
        /// <remarks>
        /// 2014-09-16 杨文兵 创建
        /// 注意：需要在服务器端做验证，验证调价之后价格是否小于最低价格，小于则不允许。
        /// 注意：服务器端需要做验证，业务员是否做了调价操作，对订单做不同的标识
        /// </remarks>
        [OperationContract] 
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result<AppSoOrder> CreateSoOrderToSettlement(AppOrder2 order);

        /// <summary>
        /// 创建业务员调价APP订单 到订单审核步骤 将订单加入任务池
        /// </summary>
        /// <param name="order">APP订单</param>
        /// <returns>返回结果</returns>
        ///  <remarks>2014-09-16 杨文兵 创建
        ///  注意：需要在服务器端做验证，验证调价之后价格是否小于最低价格，小于则不允许。
        ///  注意：服务器端需要做验证，业务员是否做了调价操作，对订单做不同的标识
        ///  </remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result<AppSoOrder> CreateSoOrderToAudit(AppOrder2 order);

        /// <summary>
        /// 创建APP订单
        /// </summary>
        /// <param name="order">APP订单</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-07-30 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result CreateSoOrder(AppOrder order);

        /// <summary>
        /// 获取该区域支持的配送方式 
        /// </summary>
        /// <param name="areaNo">地区系统编号</param>
        /// <param name="address">地区全称</param>
        /// <returns>该区域支持的配送方式 </returns>
        /// <remarks>2013-08-01 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<AppLgDeliveryType>> GetDeliveryTypeList(int areaNo, string address);

        /// <summary>
        /// 根据配送方式获取支付方式
        /// </summary>
        /// <param name="deliverySysNo">配送方式</param>
        /// <returns></returns>
        /// <remarks>2013-08-01 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<AppBsPaymentType>> GetPayTypeList(int deliverySysNo);

        /// <summary>
        /// 获取发票类型列表
        /// </summary>
        /// <returns>发票类型列表</returns>
        ///<remarks>2013-08-01 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        Result<IList<AppFnInvoiceType>> GetInvoiceTypeList();
        #endregion

        #region 配送单 黄伟

        /// <summary>
        /// 获取当天指定的配送单列表
        /// </summary>
        /// <param name="sysNos">要过滤的配送单系统编号SysNo</param>
        /// <returns>当天指定的配送单列表</returns>
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        Result<List<CBWCFLgDelivery>> GetDeliveryList(List<int> sysNos);

        /// <summary>
        /// 获取该配送员所有配送在途的配送单列表
        /// </summary>
        /// <returns>配送单列表</returns>
        /// <remarks>2013-12-30 黄伟 创建</remarks>
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        Result<List<CBWCFLgDelivery>> GetDeliveryListAll();

        /// <summary>
        /// 根据配送单编号获取配送单明细
        /// </summary>
        /// <param name="sysNo">配送单系统编号</param>
        /// <returns>配送单明细列表</returns>
        /// <remarks>2013-12-30 黄伟 创建</remarks>
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        Result<List<LogisticsDeliveryItem>> GetItemListByDeliverySysNo(int sysNo);

        /// <summary>
        /// 更新单据状态
        /// </summary>
        /// <param name="lstStatus">需要更新的单据集合</param>
        /// <returns>封装的响应实体(状态,状态编码,消息)</returns>
        /// <remarks>2013-06-24 黄伟 创建</remarks>
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json
            )]
        [OperationContract]
        Result Sign(List<CBWCFStatusUpdate> lstStatus);

        /// <summary>
        /// 获取常用文本
        /// </summary>
        /// <param name="codeType">文本类型</param>
        /// <returns>常用文本</returns>
        /// <remarks>2014-03-20 周唐炬 创建</remarks>
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Result<List<AppBsCode>> GetSignCode(int codeType);

        /// <summary>
        /// 部分签收
        /// </summary>
        /// <returns>封装的响应实体(状态,状态编码,消息)</returns>
        /// <remarks>2013-07-15 黄伟 创建</remarks>
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json
            )]
        [OperationContract]
        Result PartialSign(CBCreateSettlement cbCreateSettlement);

        /// <summary>
        /// 补单
        /// </summary>
        /// <param name="order">ParaLogisticsControllerAdditionalOrders实体</param>
        /// <returns>封装的响应实体(状态,状态编码,消息)</returns>
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json
            )]
        [OperationContract]
        Result AddOrder(ParaLogisticsControllerAdditionalOrders order);

        /// <summary>
        /// 部分签收修改数量后返回该订单总金额
        /// </summary>
        /// <param name="model">CBRMAOrderInfo实体</param>
        /// <returns>CBWCFParamGetAmountOrderAmount实体:相关金额</returns>
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Result<CBWCFParamGetAmountOrderAmount> GetAmount(CBRMAOrderInfo model);

        #endregion

        #region 手机App购物车补单接口 (周瑜)

        /// <summary>
        /// 添加一个或多个商品至购物车
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="products">商品系统编号集合</param>
        /// <param name="shopCartSource">购物车商品来源</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <returns>当前当前添加商品的购物车信息对象(包括促销信息)</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        /// <remarks>2014-03-14 周唐炬 重构</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> AddProductsToShopCartCache(string cacheKey, IList<int> products, CustomerStatus.购物车商品来源 shopCartSource, bool isReturn);

        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-09-16 沈强 修改</remarks>
        /// <remarks>2014-03-14 周唐炬 重构</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> GetShoppingCartCache(string cacheKey);

        /// <summary>
        /// 将组促销转换为购物车明细
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="shopCartSource">购物车商品来源</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        /// <remarks>2014-03-14 周唐炬 重构</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> ConvertShopCartCacheToShopCart(string cacheKey, int customerSysNo,
                                                                 CustomerStatus.购物车商品来源 shopCartSource);

        /// <summary>
        /// cache获取当前购物车有效可使用的优惠券
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>可使用的优惠券</returns>
        /// <remarks>2014-03-03 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<List<AppSpCoupon>> GetCurrentCartValidCouponsCache(string cacheKey, int customerSysNo);

        /// <summary>
        /// 添加促销赠品至购物车
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> AddGiftToShopCartCache(string cacheKey, int customerSysNo, int productSysNo, int promotionSysNo, Model.WorkflowStatus.CustomerStatus.购物车商品来源 source, bool isReturn);

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> RemoveShopCartItemsCache(string cacheKey, int customerSysNo, int[] sysNo, bool isReturn);

        /// <summary>
        /// 删除购物车组商品
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> RemoveShopCartGroupCache(string cacheKey, int customerSysNo, string groupCode, string promotionSysNo, bool isReturn);

        /// <summary>
        /// 删除促销赠品
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> RemoveGiftCache(string cacheKey, int customerSysNo, int productSysNo, int promotionSysNo, bool isReturn);

        /// <summary>
        /// 删除购物车所有明细
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result RemoveAllCache(string cache, int customerSysNo);

        /// <summary>
        /// 删除购物车选中的明细
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> RemoveCheckedItemCache(string cacheKey, int customerSysNo, bool isReturn);

        /// <summary>
        /// 更新购物车明细商品数量
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号集合</param>
        /// <param name="quantity">商品数量</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> UpdateItemsQuantityCache(string cacheKey, int customerSysNo, int[] sysNo, int quantity, bool isReturn);

        /// <summary>
        /// 更新购物车组组商品数量
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> UpdateGroupQuantityCache(string cacheKey, int customerSysNo, string groupCode, string promotionSysNo,
                                                          int quantity, bool isReturn);

        /// <summary>
        /// 选择购物车所有明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        /// [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> CheckedAllCache(string cacheKey, int customerSysNo, bool isReturn);

        /// <summary>
        /// 取消选择购物车所有明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> UncheckedAllCache(string cacheKey, int customerSysNo, bool isReturn);

        /// <summary>
        /// 选择购物车明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> CheckedItemCache(string cacheKey, int customerSysNo, int[] itemSysNo, bool isReturn);

        /// <summary>
        /// 取消选择购物车明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> UncheckedItemCache(string cacheKey, int customerSysNo, int[] itemSysNo, bool isReturn);

        /// <summary>
        /// 选择购物车组明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> CheckedGroupItemCache(string cacheKey, int customerSysNo, string groupCode, string promotionSysNo, bool isReturn);

        /// <summary>
        /// 取消选择购物车组明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isReturn">是否返回购物车 true：返回；false：不返回</param>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<LogisShoppingCart> UncheckedGroupItemCache(string cacheKey, int customerSysNo, string groupCode, string promotionSysNo, bool isReturn);

        /// <summary>
        /// 清除购物车明细
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <remarks>2013-09-27 周瑜 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result ClearCache(string cacheKey);

        /// <summary>
        /// 配送员确认补单
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="receiveAddress">收货地址对象</param>
        /// <returns>返回确认状态</returns>
        /// <remarks>2013-09-27 沈强 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result ConfromAddOrders(string cacheKey, int customerSysNo, AppSoReceiveAddress receiveAddress);

        #endregion

        #region 业务员库存 周唐炬 2014-03-05 创建

        /// <summary>
        /// 获取业务员库存
        /// </summary>
        /// <param></param>
        /// <returns>业务员库存</returns>
        /// <remarks>2014-03-05 周唐炬 创建</remarks>
        [OperationContract]
        //[WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<List<AppInventory>> GetInventoryProductList();

        #endregion

        #region 用户优惠券 周唐炬 2014-03-06 创建

        /// <summary>
        /// 获取客户优惠卷
        /// </summary>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <returns>客户优惠卷列表</returns>
        /// <remarks>2014-03-06 周唐炬 创建</remarks>
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<List<AppSpCoupon>> GetUserCoupon(int customerSysNo);

        /// <summary>
        /// 用户绑定优惠券审核
        /// </summary>
        /// <param name="sysNo">优惠券编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2014-03-06 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result UserCouponAudit(int sysNo);

        /// <summary>
        /// 用户绑定优惠券作废
        /// </summary>
        /// <param name="sysNo">优惠券编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2014-03-06 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result UserCouponCancel(int sysNo);

        /// <summary>
        /// 通过优惠卡号获取优惠卡
        /// </summary>
        /// <param name="couponCardNo">优惠卡号</param>
        /// <returns>优惠卡</returns>
        /// <remarks>2014-03-06 周唐炬 创建</remarks>
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<AppCouponCard> GetAggregatedCouponCard(string couponCardNo);

        /// <summary>
        /// 绑定优惠卡
        /// </summary>
        /// <param name="couponCardNo">优惠卡号</param>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2014-03-06 周唐炬 创建</remarks>
        /// <remarks>绑定优惠卡后，有自动审核动作</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result AssignCouponCard(string couponCardNo, int customerSysNo);

        /// <summary>
        /// 待绑定到用户的优惠卷
        /// </summary>
        /// <param name="currentPageIndex">当前索引</param>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="description">优惠卷描述</param>
        /// <returns>分页列表</returns>
        /// <remarks>2014-03-06 周唐炬 创建</remarks>
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<Pager<AppSpCoupon>> GetCouponsToBeAssigned(int currentPageIndex, int pageSize, string description);

        /// <summary>
        /// 绑定优惠券
        /// </summary>
        /// <param name="sysNo">优惠券编号</param>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2014-03-06 周唐炬 创建</remarks>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result AssignUserCoupon(int sysNo, int customerSysNo);

        #endregion

        #region 心怡科技物流
        /// <summary>
        /// 推送订单到心怡科技物流
        /// </summary>
        /// <returns>2015-09-2 王耀发 创建</returns>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        Result SendOrderToXinYi(string postdata);
        #endregion

        #region 海关
        /// <summary>
        /// 上传xml文件 
        /// 2015-10-09 王耀发 创建
        /// </summary>
        /// <param name="RequestText">Post过来的数据串 Json格式</param>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        string UploadXmlFile(string RequestText);
        /// <summary>
        /// 下载xml文件 
        /// 2015-10-09 王耀发 创建
        /// </summary>
        /// <param name="RequestText">要下载的文件名 Json格式</param>
        string DownloadXmlFile(string RequestText);
        #endregion

        #region 支付宝对接海关
        /// <summary>
        /// 支付宝对接海关
        /// </summary>
        /// <returns>2015-10-15 王耀发 创建</returns>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        Result SendOrderToAlipay(string postdata);
        #endregion
    }
}
