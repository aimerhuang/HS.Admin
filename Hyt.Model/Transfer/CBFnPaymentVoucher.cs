using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 付款单扩展（包括明细)
    /// </summary>
    /// <remarks>
    /// 2013-07-19 朱成果 创建
    /// </remarks>
    public class CBFnPaymentVoucher : FnPaymentVoucher
    {
        /// <summary>
        /// 付款单明细
        /// </summary>
        /// <remarks>
        /// 2013-07-19 朱成果 创建
        /// </remarks>
        public List<FnPaymentVoucherItem> VoucherItems { get; set; }
    }
}
