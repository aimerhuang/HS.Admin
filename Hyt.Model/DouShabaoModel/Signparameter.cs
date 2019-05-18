using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.DouShabaoModel
{
    public class Signparameter
    {
        /// <summary>
        /// 商户订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 保单总价
        /// </summary>
        public string TotalAmount { get; set; }
        /// <summary>
        /// 买家名称
        /// </summary>
        public string BuyerName { get; set; }
        /// <summary>
        /// 买家手机号
        /// </summary>
        public string BuyerMobile { get; set; }
    }
}
