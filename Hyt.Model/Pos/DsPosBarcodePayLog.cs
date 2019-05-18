using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    /// <summary>
    /// 收银软件条码支付记录
    /// </summary>
    public class DsPosBarcodePayLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 经销商编号
        /// </summary>
        public int DsSysNo { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// 收银订单编号
        /// </summary>
        public int PosOrderSysNo { get; set; }
        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal ReceiptAmount { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 交易码
        /// </summary>
        public string TradeNo { get; set; }
        /// <summary>
        /// 反馈内容
        /// </summary>
        public string Content { get; set; }
    }
}
