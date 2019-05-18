using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    public class CBDsDealerBailOrder : DsDealerBailOrder
    {
        /// <summary>
        /// 收款单编号
        /// </summary>
        /// <remarks>
        /// 2016-05-17 王耀发 创建
        /// </remarks>
        public int? FnReceiptVoucherSysNo { get; set; }

        /// <summary>
        /// 分销商
        /// </summary>
        public string DealerName { set; get; }
    }
}
