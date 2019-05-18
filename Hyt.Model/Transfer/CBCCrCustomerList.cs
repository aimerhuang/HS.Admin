using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    public class CBCCrCustomerList : CrCustomer
    {
        public IList<CBCCrCustomerList> CrCustomerLists { get; set; }
        /// <summary>
        /// 会员订单总数
        /// </summary>
        public int OrderNums { get; set; }
        /// <summary>
        /// 分销订单总数
        /// </summary>
        public int RebagesOrderCount { get; set; }
    }
}
