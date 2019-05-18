using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    public class DsPaymentToPointConfig
    {
        public int SysNo { get; set; }
        /// <summary>
        /// 经销商编号
        /// </summary>
        public int DsSysNo { get; set; }
        /// <summary>
        /// 支付金额转积分
        /// </summary>
        public decimal PaymentMoney { get; set; }
    }
}
