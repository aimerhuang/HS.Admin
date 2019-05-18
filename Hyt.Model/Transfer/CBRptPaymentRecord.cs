using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 支付统计
    /// </summary>
    /// <remarks>
    /// 2017-10-10 罗勤瑶
    /// </remarks>
    [Serializable]
    public partial class CBRptPaymentRecord
    {
        /// <summary>
        /// 支付方式编号
        /// </summary>
        [Description("支付方式编号")]
        public int PaymentTypeSysNo { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        [Description("支付方式")]
        public string PaymentName { get; set; }

        /// <summary>
        /// 支付方式销售合计
        /// </summary>
        [Description("支付方式销售合计")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 销售合计
        /// </summary>
        [Description("销售合计")]
        public decimal ALLAmount { get; set; }

        /// <summary>
        /// 起止时间
        /// </summary>
        [Description("起止时间")]
        public string TimeString { get; set; }
    }
}
