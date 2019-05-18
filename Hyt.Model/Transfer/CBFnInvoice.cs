using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    public class CBFnInvoice :FnInvoice
    {
        /// <summary>
        ///  出库单系统编号
        /// </summary>
        public int WhStockOutSysNo { get; set; }

        /// <summary>
        /// 发票类型名称
        /// </summary>
        public string InvoiceTypeName { get; set; }

        /// <summary>
        /// 订单系统编号
        /// </summary>
        public int OrderSysNo { get; set; }
    }
}
