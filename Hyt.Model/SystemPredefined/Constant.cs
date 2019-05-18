using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.SystemPredefined
{
    /// <summary>
    /// 常量
    /// </summary>
    /// <remarks>2013-07-03 吴文强 创建</remarks>
    public static class Constant
    {
        #region 百度地图API
        /// <summary>
        /// 百度地图API密钥
        /// 版本2.0+ 来源于黄波的帐号 hb
        /// </summary>
        public const string BAIDU_MAP_AK = "0974cea41adfdfcddcc965204edfa617";
        #endregion

        #region 购物车相关
        /// <summary>
        /// 购物车cookies名称
        /// </summary>
        public const string CART_COOKIE_NAME = "hytcart";

        /// <summary>
        /// 购物车COOKIE过期时间(天)
        /// </summary>
        public const int CART_COOKIE_EXPIRY = 1440;
        #endregion

        #region 后台Cookie
        /// <summary>
        /// 记录后台登录用户的用户名
        /// </summary>
        public const string ADMIN_LOGINHISTORYUSERNAME_COOKIE = "ADMIN_LOGINHISTORYUSERNAME_COOKIE";
        /// <summary>
        /// 记录后台登录用户的COOKIE过期时间
        /// </summary>
        public const int ADMIN_LOGINHISTORYUSERNAME_COOKIE_EXPIRY = 14400;
        #endregion

        #region 格式化货币
        /// <summary>
        /// #########0
        /// </summary>
        public const string DecimalToInt = "#########0"; //用于point的显示，一般来说currentprice应该没有分。
        /// <summary>
        /// #########0.00
        /// </summary>
        public const string DecimalFormat = "#########0.00";
        /// <summary>
        /// #########0.000
        /// </summary>
        public const string DecimalFormatLong = "#########0.000";
        /// <summary>
        /// #,###,###,##0.00
        /// </summary>
        public const string DecimalFormatWithGroup = "#,###,###,##0.00";
        /// <summary>
        /// ¥#########0.00
        /// </summary>
        public const string DecimalFormatWithCurrency = "¥#########0.00";
        #endregion

        #region 任务池Url和描述

        /// <summary>
        /// 订单审核描述
        /// </summary>
        /// <remarks>2013-07-03 吴文强 创建</remarks>
        public const string JOBPOOL_ORDERAUDIT_DESCRIPTION = "订单审核:{0},姓名:{1},电话:{2}";

        /// <summary>
        /// 订单审核Url
        /// </summary>
        /// <remarks>2013-07-03 吴文强 创建</remarks>
        public const string JOBPOOL_ORDERAUDIT_URL = "/order/detail/{0}";

        #endregion

        #region 积分描述

        /// <summary>
        /// 订单{0}完成后增加！
        /// </summary>
        /// <remarks>2013-07-03 吴文强 创建</remarks>
        public const string INCREASED_POINT_SHOPPING = "订单{0}完成后增加！";

        /// <summary>
        /// {0}活动赠送！
        /// </summary>
        /// <remarks>2013-07-03 吴文强 创建</remarks>
        public const string INCREASED_POINT_ACTIVITY = "{0}活动赠送！";

        #endregion

        #region 订单事务日志

        /// <summary>
        ///创建订单：
        /// </summary>
        /// <remarks>2013-07-03 黄志勇 创建</remarks>
        public const string ORDER_TRANSACTIONLOG_CREATE = "订单提交成功，请等待系统确认";

        /// <summary>
        ///审核订单：
        /// </summary>
        /// <remarks>2013-07-03 黄志勇 创建</remarks>
        public const string ORDER_TRANSACTIONLOG_AUDIT = "订单审核通过，等待分配出库";

        /// <summary>
        /// 取消订单审核:{0} (订单号)
        /// </summary>
        /// <remarks>2013-07-03 朱家宏 创建</remarks>
        public const string ORDER_TRANSACTIONLOG_AUDIT_CANCEL = "您的订单已取消审核";

        /// <summary>
        ///作废订单：{0} (订单号)
        /// </summary>
        /// <remarks>2013-07-03 黄志勇 创建</remarks>
        public const string ORDER_TRANSACTIONLOG_CANCEL = "您的订单已作废";

        /// <summary>
        /// 订单网上支付成功:{0} (支付金额)
        /// </summary>
        /// <remarks>2013-07-03 朱家宏 创建</remarks>
        public const string ORDER_TRANSACTIONLOG_PAY = "您的订单在线支付成功，支付金额：{0}";

        /// <summary>
        ///作废出库单：{0} (出库单号)
        /// </summary>
        /// <remarks>2013-07-03 朱成果 创建</remarks>
        public const string ORDER_OUTSTOCK_CANCEL = "您的出库单已作废，原因：{1}";

        /// <summary>
        ///签收出库单：{0} (出库单号)
        /// </summary>
        /// <remarks>2013-07-03 朱成果 创建</remarks>
        public const string ORDER_OUTSTOCK_SIGN = "客户已签收";

        /// <summary>
        ///订单完成全部配送：{0} (订单号)
        /// </summary>
        /// <remarks>2013-07-03 朱成果 创建</remarks>
        public const string ORDER_FINISH = "已完成全部配送";

        /// <summary>
        ///完成作废订单退款，付款单
        /// </summary>
        /// <remarks>2013-07-03 朱成果 创建</remarks>
        public const string ORDER_RETURNCASH = "您的作废单已退款，付款单：{0}，请您注意查收";

        /// <summary>
        /// 订单生成出库单:{0} (出库仓库){1} (出库单号)
        /// </summary>
        /// <remarks>2013-07-03 朱家宏 创建</remarks>
        public const string ORDER_TRANSACTIONLOG_OUTSTOCK_CREATE = "订单分配出库成功，{0}，出库单{1}，待选择配送方式";

        /// <summary>
        /// 门店【{0}】创建订单：{1} (门店名称，订单号)
        /// </summary>
        /// <remarks>2013-07-03 朱家宏 创建</remarks>
        public const string ORDER_TRANSACTIONLOG_SHOPORDER_CREATE = "门店【{0}】已创建订单";

        /// <summary>
        /// 门店订单已确认，等待提货
        /// </summary>
        /// <remarks>2014-02-24 余勇 创建</remarks>
        public const string ORDER_TRANSACTIONLOG_SHOPORDER_SURE = "门店订单已确认，等待提货";

        /// <summary>
        /// 门店自提(不发送验证码)
        /// </summary>
        /// <remarks>2013-07-06 朱成果 创建</remarks>
        public const string SELF_DELIVERY_SURE = "您订购的商品已准备好，请及时提货";

        /// <summary>
        /// 门店自提(发送验证码)
        /// </summary>
        /// <remarks>2013-07-06 朱成果 创建</remarks>
        public const string SELF_DELIVERY_SURE_CODE = "您的退换货单已申请成功，请等待系统确认";

        #endregion

        #region 退换货日志
        /// <summary>
        ///创建退换货单：{0} (退换货单号)
        /// </summary>
        /// <remarks>2013-07-03 朱成果 创建</remarks>
        public const string RMA_CREATE = "创建退换货单：{0}";
        /// <summary>
        ///退换货单审核通过：{0} (退换货单号)
        /// </summary>
        /// <remarks>2013-07-03 朱成果 创建</remarks>
        public const string RMA_Checked = "您的退换货单已审核通过";
        /// <summary>
        ///作废退换货单：{0} (退换货单号)
        /// </summary>
        /// <remarks>2013-07-03 朱成果 创建</remarks>
        public const string RMA_Cancel = "您的退换货单已作废";
        /// <summary>
        ///创建退换货入库单：{0} (入库单号)
        /// </summary>
        /// <remarks>2013-07-03 朱成果 创建</remarks>
        public const string RMA_InStock = "您的退换货入库单已创建";
        /// <summary>
        /// 退货单生成付款单:{0}(付款单号)
        /// </summary>
        /// <remarks>2013-07-13 朱成果 创建</remarks>
        public const string RMA_NEWPAY = "您的退货付款单已生成";
        /// <summary>
        /// 换货货单生成RMA销售单:{0}(销售单单)
        /// </summary>
        /// <remarks>2013-07-13 朱成果 创建</remarks>
        public const string RMA_NEWORDER = "您的换货单已生成RMA销售单，单号:{0}";
        /// <summary>
        /// 入库单（{0}）作废，退换货单更新为待审核状态(取件单)
        /// </summary>
        /// <remarks>2013-07-13 朱成果 创建</remarks>
        public const string RMA_INSTOCKCANCEL = "您的入库单（{0}）已作废，退换货单更新为待审核状态";
        /// <summary>
        /// 退换货完成
        /// </summary>
        /// <remarks>2013-07-13 朱成果 创建</remarks>
        public const string RMA_COMPLETE = "您的退换货申请已完成";
        #endregion

        /// <summary>
        /// 打印配货单每页下面广告语，可变可配置 不应该放在这里
        /// </summary>
        /// <remarks>2013-07-16 郑荣华 创建</remarks>
        public static string PRINT_ADVERTISING_SLOGAN = @"<br/>祝生活愉快！商城";
        #region 更新订单前台显示状态
        /// <summary>
        /// 订单在线状态
        /// </summary>
        /// <remarks>2013-07-05 余勇 创建</remarks> 
        public struct OlineStatusType
        {
            public const string 待审核 = "待审核";
            public const string 待支付 = "待支付";
            public const string 待出库 = "待出库";
            public const string 作废 = "已作废";
            public const string 已发货 = "已发货";
            public const string 已完成 = "已完成";
        }
        #endregion

        #region 菜单管理

        /// <summary>
        /// 菜单图片存放地址
        /// </summary>
        /// <remarks>2013-08-09 朱家宏 创建</remarks>
        public const string MENUICO_FOLDER_NAME = @"/Theme/images/icons/";

        #endregion

        #region 邮箱验证地址

        /// <summary>
        /// 邮箱验证地址
        /// </summary>
        /// <remarks>2013-08-29 苟治国 创建</remarks>
        public const string webroot = @"http://localhost:60002/";

        #endregion

        #region 后台天台管理李洪编号

        /// <summary>
        /// 固定用户
        /// </summary>
        /// <remarks>2013-12-25 苟治国 创建</remarks>
        public const int UserSysNo = 29;

        #endregion

        #region 优惠券来源描述

        /// <summary>
        /// 绑定用户优惠卷
        /// </summary>
        /// <remarks>2013-12-10 朱家宏 创建</remarks>
        public static string COUPONDESCRIPTION_BINDUSERCOUPON = @"活动赠送";
        #endregion


        #region 产品条码
        /// <summary>
        /// 产品自定义条码前缀
        /// </summary>
        public const string CUSTOMIZE_BARCODE = "CUSTOMIZEBARCODE-";
        #endregion

        #region

        /// <summary>
        /// 手机客户端注册送积分
        /// </summary>
        public const string App_Register_Integral = "手机客户端注册送积分";

        #endregion
    }
}
