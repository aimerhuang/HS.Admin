using System.ComponentModel;
using System.Configuration;

namespace Hyt.Infrastructure.Caching
{
    public class CacheKeys
    {
        
        #region 微信

        public const string SHOPMALL_UI_CARTTOTYPE = "/ShopMall/UI/CartToType-{0}";
        public const string SHOPMALL_UI_ADV = "/ShopMall/UI/Advs/Wap/Adv-{0}";
        public const string SHOPMALL_UI_CART_ALL = "/ShopMall/UI/Cart/All";
        public const string WEIXIN_ACCESSTOKEN = "/WeinXin/AccessToken";
        public const string WEIXIN_JSAPITICKET = "/WeinXin/JsApiTicket";

        #endregion

        #region 缓存前缀编号
        /// <summary>
       /// 缓存前缀编号
       /// </summary>
       public static string SiteCachePrefix
       {
            get
            {
                return ConfigurationManager.AppSettings["SiteCachePrefix"];
            }
        }
        #endregion
        /// <summary>
        /// 缓存关键字 带下划线_的是动态关键字
        /// </summary>
        /// <remarks>2013-4-18 何方 创建</remarks>
        public enum Items
        {
            #region 前台静态页面缓存
            /// <summary>
            /// 首页视图
            /// </summary>
            [Description("首页视图")]
            ViewHtmlHomeIndex_,

            #endregion
            #region 百度定位
            /// <summary>
            /// 前台当前城市地区对应小区的json字符
            /// </summary>
            [Description("前台当前城市地区对应小区的json字符")]
            CityOptions_,
            #endregion
            #region 店铺
            /// <summary>
            /// 经销商商品
            /// </summary>
            [Description("经销商商品")]
            StoresProductList_,
            /// <summary>
            /// 全部经销商
            /// </summary>
            [Description("全部经销商")]
            StoreAll,
            /// <summary>
            /// 经销商产品销售价格
            /// </summary>
            [Description("经销商产品销售价格")]
            StoresProduct_,
            /// <summary>
            /// 经销商详情
            /// </summary>
            [Description("经销商详情")]
            StoresInfo_,
            /// <summary>
            /// 微信授权码key
            /// </summary>
            [Description("微信授权码key")]
            WeixinAccessToken_,
           /// <summary>
           /// 微信JS-SDK的使用权限签名
           /// </summary>
           [Description("微信JS-SDK的使用权限签名")]
           WeixinJsTicket_,
            #endregion

            ///// <summary>
            ///// 订单测试
            ///// </summary>
            //[Description("订单测试")]
            //OrderList_,

            ///// <summary>
            ///// 整型测试
            ///// </summary>
            //[Description("整型测试")]
            //IntTest,

            ///// <summary>
            ///// 日期测试
            ///// </summary>
            //[Description("日期测试")]
            //DateTest,

            ///// <summary>
            ///// 数组测试
            ///// </summary>
            //[Description("数组测试")]
            //ArrayTest,

            #region 三期缓存键值列表

            #region 搜索功能
            
            /// <summary>
            /// 所有可用的分类
            /// </summary>
            [Description("所有可用的分类")]
            AllCategory,

            /// <summary>
            /// 父级分类编号下所有子分类缓存[父级分类编号]
            /// </summary>
            [Description("父级分类编号下所有子分类缓存[父级分类编号]")]
            ChildAllCategory_,

            /// <summary>
            /// 父级分类编号下子分类缓存[父级分类编号]
            /// </summary>
            [Description("父级分类编号下子分类缓存[父级分类编号]")]
            ChildCategory_,

            /// <summary>
            /// PC网站广告[广告组代码]
            /// </summary>
            [Description("PC网站广告[广告组代码]")]
            WebAdvertItem_,

            /// <summary>
            /// PC网站商品广告组[组代码]
            /// </summary>
            [Description("PC网站商品广告组[组代码]")]
            WebProductItem_,
            /// <summary>
            /// 商品组[平台代码]
            /// </summary>
            [Description("网站商品组[平台代码]")]
            WebGroupcode_,

            /// <summary>
            /// PC网站团购[组代码]
            /// </summary>
            [Description("PC网站团购[代码]")]
            WebGroupShopping_,

            /// <summary>
            /// 商品分类
            /// </summary>
            [Description("商品分类")]
            ProductCategory,

            /// <summary>
            /// 搜索热词
            /// </summary>
            [Description("搜索热词")]
            SearchKeys,

            [Description("搜索分类下商品属性列表[商品分类]")]
            SearchCategoryAttributeOptionsList_,
            #endregion

            /// <summary>
            /// 商城用户等级
            /// </summary>
            [Description("用户等级数据")]
            CustomerLevel,
            /// <summary>
            /// 用户详情
            /// </summary>
            [Description("用户详情")]
            CustomerInfo_,

            #region 商品缓存键

            /// <summary>
            /// 商品价格类型
            /// </summary>
            [Description("商品价格类型")]
            ProductPriceTypesNew,

            /// <summary>
            /// 商品详情页，当个商品所以相关数据
            /// </summary>
            [Description("商品详细内容[商品系统编号]")]
            ProductDetailInfo_,

            [Description("商品评价评分[商品系统编号]")]
            ProductAverageReviewSatisfaction_,

            /// <summary>
            /// 商品的主商品分类路径：如 保护 > IPhone > 贴膜
            /// </summary>
            [Description("商品主分类详细路径[商品分类系统编号]")]
            ProductMasterCategoryRoute_,

            /// <summary>
            /// 商品分类详细信息，包含分类和他的上级分类信息
            /// </summary>
            [Description("商品分类详细信息（包含上级分类）[商品分类系统分类编号]")]
            CategoryInfoWithParent_,

            /// <summary>
            /// 单个商品分类信息
            /// </summary>
            [Description("单个商品分类详细信息（仅分类自身数据）[商品分类系统编号]")]
            SingleCategoryInfo_,

            /// <summary>
            /// 关联商品（属性相关联）
            /// </summary>
            [Description("商品属性关联商品列表[商品关联关系码]")]
            ProductAssociation_,

            /// <summary>
            /// 商品用做关联的商品属性，主要用来显示
            /// </summary>
            [Description("商品可用作关联的属性列表[商品系统编号]")]
            ProductAssociationAttribute_,

            /// <summary>
            /// 读取商品关联的所以关联属性值
            /// </summary>
            [Description("读取商品关联的所以关联属性值[关联关系码]")]
            ProductAssociationAllAssociationAttribute_,

            /// <summary>
            /// 推荐商品，用在商品下架时显示
            /// </summary>
            [Description("商品下架时商品推荐[分类系统编号]")]
            ProductDetialOffsaleRcdList_,

            /// <summary>
            /// 一个商品分类的面包屑字符串
            /// </summary>
            [Description("商品分类导航字符串（面包屑路径）[商品分类系统编号]")]
            ProductCategoryNavigationString_,

            [Description("商品分类路径字符串[商品分类系统编号]")]
            ProductCategoryPathString_,

            /// <summary>
            /// 商品的属性信息
            /// </summary>
            [Description("商品属性[商品系统编号]")]
            ProductAttribute_,

            /// <summary>
            /// 其他人还买了哪些商品
            /// </summary>
            [Description("其他人还购买了那些商品[商品系统编号]")]
            OtherCustmerBought_,

            /// <summary>
            /// 根据商品SysNo获取随心配信息
            /// </summary>
            [Description("根据商品SysNo获取随心配信息")]
            FollowWithByProductSysNo_,

            /// <summary>
            /// 根据产品SysNo获取最先评论的5位用户
            /// </summary>
            [Description("根据产品SysNo获取最先评论的5位用户 [商品系统编号]")]
            FirstReviewTop5_,

            /// <summary>
            /// 根据商品SysNo获取评论晒单咨询的记录总数
            /// </summary>
            [Description("根据商品SysNo获取评论记录总数 [商品系统编号]")]
            ProductCommentTotalInfo_,

            /// <summary>
            /// 根据商品SysNo和咨询类型获取咨询内容 [商品系统编号_咨询类型]
            /// </summary>
            [Description("根据商品SysNo和咨询类型获取咨询内容 [商品系统编号_咨询类型]")]
            ProductQuestions_,

            /// <summary>
            /// 根据商品SysNo和咨询类型获取全部咨询内容，分页展示[商品系统编号_咨询类型]
            /// </summary>
            [Description("根据商品SysNo和咨询类型获取全部咨询内容，分页展示 [商品系统编号_咨询类型]")]
            ProductQuestionsList_,

            /// <summary>
            /// 根据商品SysNo获取评论晒单咨询的记录总数
            /// </summary>
            [Description("根据商品SysNo获取咨询的记录总数 [商品系统编号]")]
            ProductQuestionsTotalInfo_,

            /// <summary>
            /// 根据商品SysNo获取咨询总数
            /// </summary>
            [Description("根据商品SysNo获取咨询总数")]
            ProductQuestionsCount_,

            /// <summary>
            /// 根据商品SysNo获取默认图片路径
            /// </summary>
            [Description("根据商品SysNo获取默认图片路径 [商品系统编号]")]
            ProductDefaultImage_,

            [Description("商品分类销售排行[商品分类系统编号+记录个数]")]
            CategorySaleRanking_,

            [Description("商品相关的搭配销售[商品系统编号]")]
            ProductCollocation_,

            [Description("商品相关组合套餐列表[主商品系统编号]")]
            ProductSpCombo_,

            [Description("商品评论带分页和评论类型明细[主商品系统编号_类型_页码]")]
            ProductCommentWithPager_,

            [Description("商品实体_系统编号")]
            CBPdProduct_,
            [Description("商品所有")]
            AllProduct,
            #endregion

            #region 仓库相关
            [Description("根据地区及配送方式筛选仓库列表")]
            WhWarehouseDeliveryType_,

            [Description("地区下的仓库列表")]
            WhWarehouseListByArea_,

            /// <summary>
            /// 仓库缓存key
            /// </summary>
            [Description("仓库信息")]
            WhWarehouse_,
            /// <summary>
            /// 仓库地区缓存key
            /// </summary>
            [Description("仓库地区经销商信息")]
            WhCBWarehouseArea_,
            #endregion

            #region 用户安全验证码缓存键
            [Description("邮件校验码前缀")]
            EmailVerificationCode_,

            [Description("注册手机验证码")]
            RegisterPhoneVerifyCode_,

            [Description("验证注册手机修改次数")]
            RegisterPhoneCount_,

            [Description("找回密码手机验证码")]
            FindPasswordPhoneVerifyCode_,

            [Description("修改手机验证码")]
            ModifyPhoneVerifyCode_,

            [Description("验证邮箱手机修改次数")]
            VerifyMobileCount_,

            [Description("会员中心安全设置修改手机验证码")]
            ModifySafePhoneVerifyCode_,
            [Description("会员中心安全设置修改手机次数")]
            ModifySafePhoneVerifyCount_,

            [Description("找回密码设置修改手机验证码")]
            FindPassPhoneVerifyCode_,

            [Description("找回密码设置手机为可修改状态")]
            FindPassPhoneStatus_,

            [Description("找回密码设置修改手机次数")]
            FindPassPhoneCount_,
            #endregion

            #region 文章管理
            /// <summary>
            /// 2013-08-12 杨晗 创建
            /// </summary>
            [Description("帮助中心文章缓存")]
            AllHelp,

            /// <summary>
            /// 后台全部新闻分类Ztree树节点数据缓存
            /// </summary>
            [Description("后台全部新闻分类Ztree树节点数据缓存")]
            BackendAllNewsCategoryZtreeNodeData,

            /// <summary>
            /// 新闻公告
            /// </summary>
            [Description("新闻公告")]
            WebBulletin,

            /// <summary>
            /// 新闻分类详细信息，包含分类和他的上级分类信息
            /// </summary>
            [Description("新闻分类详细信息（包含上级分类）[新闻分类系统分类编号]")]
            NewsCategoryInfoWithParent_,

            /// <summary>
            /// 单个商品分类信息
            /// </summary>
            [Description("单个新闻分类详细信息（仅分类自身数据）[新闻分类系统编号]")]
            SingleNewsCategoryInfo_,
            #endregion

            #region 订单相关
            /// <summary>
            /// 发票类型列表
            /// </summary>
            [Description("发票类型列表")]
            InvoiceTypeList,

            /// <summary>
            /// 获取系统支付方式
            /// </summary>
            [Description("系统所有的支付方式")]
            PaymentType,

            /// <summary>
            /// 订单支付类型
            /// </summary>
            [Description("订单支付类型 [类型编号]")]
            OrderType_,
            

            /// <summary>
            /// 订单日志记录
            /// </summary>
            [Description("订单日志 [订单编号]")]
            OrderLogList_,
            #endregion

            #region 团购
            /// <summary>
            /// 团购详细页
            /// </summary>
            [Description("团购详细页")]
            GroupShoppingDetail_,
            /// <summary>
            /// 团购详细页
            /// </summary>
            [Description("团购今日推荐")]
            GroupShoppingToDayPageIndex_,
            #endregion

            #region 促销

            [Description("单商品促销信息[商品系统编号]")]
            SingleProductSpPromotionInfo_,

            #endregion

            #region 个人中心

            /// <summary>
            /// 用户未评价商品数
            /// </summary>
            [Description("用户未评价商品数 [用户编号]")]
            UserUnCommentNumber_,

            /// <summary>
            /// 用户未处理的订单
            /// </summary>
            [Description("用户未处理的订单 [用户编号]")]
            UserUntreatedOrderNumber_,
            

            #endregion

            #endregion

            #region 商城后台用缓存

            BackendIndexTotalInfo,

            /// <summary>
            /// 后台全部商品分类Ztree树节点数据缓存
            /// </summary>
            [Description("后台全部商品分类Ztree树节点数据缓存")]
            BackendAllPdCategoryZtreeNodeData,

            #endregion

            /// <summary>
            /// 全部商品分类
            /// </summary>
            [Description("全部商品分类")]
            AllPdCategoryList,
            /// <summary>
            /// 所有国家
            /// </summary>
            [Description("所有国家")]
            AllOrigin,

            #region 行邮税
            [Description("产品行邮税")]
            ProductParcelTaxs,
            #endregion
            #region 地区
            /// <summary>
            /// 全部地区json结构字符
            /// </summary>
            ALLAreaJson,
            #endregion
            //#region 二期的  不用
            //[Description("测试关键字")]
            //TestKey,

            //[Description("注册手机验证码")]
            //RegisterPhoneVerifyCode_,

            //[Description("找回密码手机验证码")]
            //FindPasswordPhoneVerifyCode_,

            //[Description("修改手机验证码")]
            //ModifyPhoneVerifyCode_,

            //[Description("验证邮箱手机修改次数")]
            //VerifyMailorCellPhoneCount_,

            //[Description("用户信息前缀")]
            //CustomerInfo_,

            ///// <summary>
            ///// 根据会员等级SysNo获取会员等级信息
            ///// </summary>
            //[Description("根据会员等级SysNo获取会员等级信息")]
            //CustomerRankBySysNo_,

            //[Description("邮件校验码前缀")]
            //EmailVerificationCode_,

            ///// <summary>
            ///// 2013-4-12 杨晗 创建
            ///// </summary>
            //[Description("帮助中心文章缓存")]
            //AllHelp,

            ///// <summary>
            ///// 产品分类列表
            ///// </summary>
            //[Description("产品分类列表")]
            //ProductCategoryList,
            ///// <summary>
            ///// 新闻公告
            ///// </summary>
            //[Description("新闻公告")]
            //WebBulletin,
            ///// <summary>
            ///// 广告列表缓存
            ///// </summary>
            //[Description("广告列表缓存")]
            //AdvertiseInfo,

            //[Description("地区列表")]
            //AreaInfo,

            ///// <summary>
            ///// 已购商品前缀 何方 创建
            ///// </summary>
            //[Description("已购商品前缀")]
            //SOItem_,

            ///// <summary>
            ///// 订单的所有商品前缀 何方 创建
            ///// </summary>
            //[Description("订单的所有商品前缀")]
            //SOItems_,

            ///// <summary>
            ///// 用户所有已购商品前缀 何方 创建
            ///// </summary>
            //[Description("用户所有已购商品前缀")]
            //CustomerSOItems_,

            ///// <summary>
            ///// 用户所有已购商品前缀 何方 创建
            ///// </summary>
            //[Description("用户所有已购商品前缀")]
            //CustomerUnReviewSOItems_,

            //#region 产品相关 region by hefang 2013.4.18

            ///// <summary>
            ///// 根据产品SysNo查询产品所有会员等级价格
            ///// </summary>
            //[Description("根据产品SysNo查询产品所有会员等级价格")]
            //ProductCustomerRankPriceByProductSysNo_,

            ///// <summary>
            ///// 根据产品SysNo获得产品价格
            ///// </summary>
            //[Description("根据产品SysNo获得产品价格")]
            //ProductPriceInfoByProductSysNo_,

            ///// <summary>
            ///// 根据产品SysNo获取产品总仓库存信息
            ///// </summary>
            //[Description("根据产品SysNo获取产品总仓库存信息")]
            //InventoryByProductSysNo_,

            ///// <summary>
            /////根据产品SysNo获得产品关联属性(Attribute2)
            ///// </summary>
            //[Description("根据产品SysNo获得产品关联属性(Attribute2)")]
            //AssociateAttribute2ByProductSysNo_,

            ///// <summary>
            ///// 根据产品SysNo获取其他购买(购买此商品的用户还购买了其他商品)
            ///// </summary>
            //[Description("根据产品SysNo获取其他购买(购买此商品的用户还购买了其他商品)")]
            //OtherBuyByProductSysNo_,

            ///// <summary>
            ///// 根据产品SysNo和小类别SysNo获取产品类别属性
            ///// </summary>
            //[Description("根据产品SysNo和小类别SysNo获取产品类别属性")]
            //ProductCategoryAttributeByProductSysNo_,

            ///// <summary>
            ///// 根据产品SysNo获得包装清单
            ///// </summary>
            //[Description("根据产品SysNo获得包装清单")]
            //ProductPackageListByProductSysNo_,

            ///// <summary>
            ///// 根据商品SysNo获取随心配信息
            ///// </summary>
            //[Description("根据商品SysNo获取随心配信息")]
            //FollowWithByProductSysNo_,

            //#endregion

            //#region 评论相关 region by hefang 2013.4.18
            ///// <summary>
            ///// 评论前缀 何方 创建
            ///// </summary>
            //[Description("评论前缀")]
            //Review_,

            ///// <summary>
            ///// 评论回复前缀 何方 创建
            ///// </summary>
            //[Description("评论前缀")]
            //ReviewReplys_,

            ///// <summary>
            ///// 一个晒单所有图片 何方 创建
            ///// </summary>
            //[Description("一个晒单所有图片")]
            //ReviewPhotos_,

            ///// <summary>
            ///// 商品总评分 何方 创建
            ///// </summary>
            //[Description("商品总评分")]
            //ReviewTotalScore_,

            ///// <summary>
            ///// 商品总评数 何方 创建
            ///// </summary>
            //[Description("商品总评数")]
            //ReviewCount_,

            ///// <summary>
            ///// 根据产品SysNo获取产品满意度平均值(已计算平均值)
            ///// </summary>
            //[Description("根据产品SysNo获取产品满意度平均值(已计算平均值)")]
            //AverageReviewSatisfaction_,

            //#endregion

            //[Description("根据小类sysno获得属性组合")]
            //AttributesByC3SysNo_,//杨浩

            //[Description("推荐搜索关键字")]
            //SearchKeys//杨浩

            //#endregion

            [Description("爱勤专用Web分类页面缓存")]
            FrontCategoryPageHtml_,
        }
    }
}