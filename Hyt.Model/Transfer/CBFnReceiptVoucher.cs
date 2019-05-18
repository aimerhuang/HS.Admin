using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 收款单
    /// </summary>
    /// <remarks>2013-07-19 朱家宏 创建</remarks>
    public class CBFnReceiptVoucher : FnReceiptVoucher
    {
        /// <summary>
        /// 付款人
        /// </summary>
        public string PaymentName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 创建人民
        /// </summary>
        public string CreatorName { get; set; }
        /// <summary>
        /// 客户名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 确认者名称
        /// </summary>
        public string ConfirmeName { get; set; }
        /// <summary>
        /// 银行 卡号
        /// </summary>
        public string CreditCardNumber { get; set; }
        /// <summary>
        /// 交易流水好
        /// </summary>
        public string VoucherNo { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 确认人
        /// </summary>
        public string Confirmer { get; set; }

        /// <summary>
        /// 收款单明细
        /// </summary>
        /// <remarks>
        /// 2013-07-19 余勇 创建
        /// </remarks>
        public List<CBFnReceiptVoucherItem> VoucherItems { get; set; }
    }

    /// <summary>
    /// 收款单明细
    /// </summary>
    /// <remarks>2013-07-19 余勇 创建</remarks>
    public class CBFnReceiptVoucherItem : FnReceiptVoucherItem
    {
        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string PaymentTypeName
        {
            get;
            set;
        }
    }
}
