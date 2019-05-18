
using System;
using System.ComponentModel;

namespace Hyt.Infrastructure.Memory
{
    /// <summary>
    /// 后台缓存key
    /// </summary>
    /// <remarks>2013-11-05 杨浩 添加</remarks>
    public class KeyConstant
    {
        /// <summary>
        /// 应用Key识别符
        /// </summary>
        public const string Prefix = "Hyt.";

        /// <summary>
        /// 系统用户认证信息key
        /// </summary>
        public const string SyUser = Prefix + "系统用户认证信息(SysNo_{0})";

        /// <summary>
        /// 获取所有地区key
        /// </summary>
        public const string AreaAll = Prefix + "所有地区信息";

        /// <summary>
        /// 所有仓库信息key
        /// </summary>
        public const string WarehouseList = Prefix + "所有仓库信息";

        /// <summary>
        /// 仓库配送方式key
        /// </summary>
        public const string WhWarehouseDeliveryTypeList = Prefix + "所有仓库配送方式";

        /// <summary>
        /// 所有仓库覆盖地区key
        /// </summary>
        public const string WhwarehouseAreaList = Prefix + "所有仓库覆盖地区";

        /// <summary>
        /// 微信授权码key
        /// </summary>
        public const string WeixinAccessToken = Prefix + "微信授权码信息";
        /// <summary>
        /// 微信JS-SDK的使用权限签名
        /// </summary>
        public const string WeixinJsTicket = Prefix + "微信JSSDK权限签名信息";
        /// <summary>
        /// 微信授权码锁
        /// </summary>
        public const string WeixinAccessTokenLock_ = Prefix + "微信授权码锁_{0}";

        /// <summary>
        /// 微信授权码key
        /// </summary>
        public const string WeixinAccessToken_ = Prefix + "微信授权码_{0}";
        /// <summary>
        /// 微信JS-SDK的使用权限签名
        /// </summary>
        public const string WeixinJsTicket_ = Prefix + "微信JSSDK签名_{0}";

        /// <summary>
        /// 分销工具用户认证信息key
        /// </summary>
        public const string FenxiaoUser = Prefix + "分销工具用户认证信息_{0}";

        /// <summary>
        /// 商品支持的促销提示信息缓存
        /// </summary>
        [Description("商品支持的促销提示信息缓存 [商品编号]")]
        public const string ProductPromotionHints = Prefix + "商品支持的促销提示信息缓存(商品编号{0}类型{1})";

        /// <summary>
        /// 根据地址判断是否在百城当日达区域内
        /// </summary>
        public const string LongitudeAndLatitude = Prefix + "根据地址判断是否在百城当日达区域内(地址:{0})";

        /// <summary>
        /// 产品ErpCode
        /// </summary>
        public const string ProductErpCode = Prefix + "ErpCode.{0}";

        /// <summary>
        /// 产品EasName
        /// </summary>
        public const string ProductEasName = Prefix + "EasName.{0}";

        /// <summary>
        /// 所有系统配置信息key
        /// </summary>
        public const string SystemConfigList = Prefix + "所有系统配置信息";

        /// <summary>
        /// 支付类型
        /// </summary>
        public const string PaymentType = Prefix + "支付类型(SysNo_{0})";

        /// <summary>
        /// 根据当前用户判断是否拥有所有仓库权限
        /// </summary>
        public const string HasAllWarehouse = Prefix + "根据当前用户判断是否拥有所有仓库权限(用户编号:{0})";

        /// <summary>
        /// 通过系统编号获取分销商商城信息
        /// </summary>
        public const string DsDealerMall = Prefix + "通过系统编号获取分销商商城信息(分销商商城系统编号:{0})";

        /// <summary>
        /// 订单收货地址
        /// </summary>
        public const string OrderReceiveAddress = Prefix + "订单收货地址(SysNo_{0})";

        /// <summary>
        /// 百城当日达区域信息列表
        /// </summary>
        public const string LgDeliveryScopeList = Prefix + "区域信息列表(AreaSysNo_{0})";

        /// <summary>
        /// 根据系统编号获取发票信息
        /// </summary>
        public const string FnInvoice = Prefix + "根据系统编号获取发票信息(发票编号:{0})";

        /// <summary>
        /// 所有天猫商城图标
        /// </summary>
        public const string MallTypeImgList = Prefix + "所有天猫商城图标";

        /// <summary>
        ///根据系统配置Key获取配置对象
        /// </summary>
        public const string SysConfigInfo = Prefix + "根据系统配置Key获取配置对象(key:{0})";

         /// <summary>
        ///根据订单规则Key获取值订单规则命令对象
        /// </summary>
        public const string OrderRuleCommand = Prefix + "根据订单规则Key获取值订单规则命令对象(key:{0})";

        /// <summary>
        /// 所有仓库配送范围
        /// </summary>
        public const string ALLWhDeliveryScope = Prefix + "所有仓库配送范围";
        /// <summary>
        /// 所有分销商信息key
        /// </summary>
        public const string DealerList = Prefix + "所有分销商信息";
        /// <summary>
        /// 所有产品
        /// </summary>
        public const string AllProduct = Prefix + "所有才产品";
        /// <summary>
        /// 经销商所有产品
        /// </summary>
        public const string DealerAllProduct_ = Prefix + "经销商所有才产品_{0}";
    }
}
