using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// 用于App接口的购物车订单对象
    /// </summary>
    /// <remarks>2013-12-01 沈强 创建</remarks>
    public class AppShopCartOrder
    {
        /// <summary>
        /// 客户系统编号
        /// </summary>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 配送方式编号
        /// </summary>
        public int DeliveryTypeSysNo { get; set; }

        /// <summary>
        /// 支付方式编号
        /// </summary>
        public int PayTypeSysNo { get; set; }

        /// <summary>
        /// 配送备注
        /// </summary>
        public String DeliveryRemarks { get; set; }

        /// <summary>
        /// 配送时间段
        /// </summary>
        public String DeliveryTime { get; set; }

        /// <summary>
        /// 配送前是否联系：是（1）、否（0）
        /// </summary>
        public int ContactBeforeDelivery { get; set; }

        /// <summary>
        /// 客户留言
        /// </summary>
        public String CustomerMessage { get; set; }

        /// <summary>
        /// 对内备注
        /// </summary>
        public String InternalRemarks { get; set; }

        /// <summary>
        /// 优惠券代码
        /// </summary>
        public String CouponCode { get; set; }

        /// <summary>
        /// 发票对象
        /// </summary>
        public Invoice Invoice { get; set; }

        /// <summary>
        /// 交易（银行卡，信用卡）卡号
        /// </summary>
        public string CreditCardNumber { get; set; }

        /// <summary>
        /// 凭证号
        /// </summary>
        public string VoucherNo { get; set; }
    }

    /// <summary>
    /// 用于App接口的发票对象
    /// </summary>
    /// <remarks>2013-12-01 沈强 创建</remarks>
    public class Invoice
    {
        /// <summary>
        /// 发票类型系统编号
        /// </summary>
        public int InvoiceTypeSysNo { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        public String InvoiceTitle { get; set; }

        /// <summary>
        /// 发票备注
        /// </summary>
        public String InvoiceRemarks { get; set; }
    }
}
