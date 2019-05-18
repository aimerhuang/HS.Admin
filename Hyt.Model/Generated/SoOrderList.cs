using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2013-11-04 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class SoOrderList
    {
        /// <summary>
        /// 会员账号
        /// </summary>
        [Description("会员账号")]
        public string Account { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        [Description("仓库")]
        public string WarehouseName { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        [Description("配送方式")]
        public string DeliveryTypeName { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        [Description("支付方式")]
        public string PayType { get; set; }

        /// <summary>
        /// 会员留言
        /// </summary>
        [Description("会员留言")]
        public string CustomerMessage { get; set; }

        /// <summary>
        /// 配送备注
        /// </summary>
        [Description("配送备注")]
        public string DeliveryRemark { get; set; }

        /// <summary>
        /// 对内备注
        /// </summary>
        [Description("对内备注")]
        public string InternalRemark { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        [Description("收货地址")]
        public string ReceiveAddress { get; set; }

        /// <summary>
        /// 订购商品
        /// </summary>
        [Description("订购商品")]
        public string OrderItem { get; set; }

        /// <summary>
        /// 发票类型
        /// </summary>
        [Description("发票类型")]
        public string InvoiceType { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        [Description("发票抬头")]
        public string InvoiceTitle { get; set; }

        /// <summary>
        /// 发票备注
        /// </summary>
        [Description("发票备注")]
        public string InvoiceRemarks { get; set; }
    }
}
