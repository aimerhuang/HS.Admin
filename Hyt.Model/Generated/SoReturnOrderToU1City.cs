using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    public class SoReturnOrderToU1City
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 外部退货编号
        /// </summary>
        [Description("外部退货编号")]
        public string TransactionReturnNo { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        [Description("订单编号")]
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 外部订单编号
        /// </summary>
        [Description("外部订单编号")]
        public string TransactionOrderNo { get; set; }
        /// <summary>
        /// 外部退货编号
        /// </summary>
        [Description("外部出库编号")]
        public string TransactionOutBillNo { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        [Description("时间")]
        public DateTime CreateDate { get; set; }
    }
}
