using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    public class CBDsPosReturnOrder : DsPosReturnOrder
    {
        public string StoreName { get; set; }
        public string PosName { get; set; }

        public string AliPayNumber { get; set; }
        public string SellOrderNumber { get; set; }
        public int DsSysNo { get; set; }

        public List<DsPosReturnOrderItem> Items = new List<DsPosReturnOrderItem>();
    }
    /// <summary>
    /// 退货订单
    /// </summary>
    public class DsPosReturnOrder
    {
        public int SysNo { get; set; }
        public int OrderSysNo { get; set; }
        public string SerialNumber { get; set; }
        public string Clerker { get; set; }
        public DateTime ReturnTime { get; set; }
        public int TotalNum { get; set; }
        public decimal TotalReturnValue { get; set; }
        public decimal TotalMayReturnValue { get; set; }
        public decimal ReturnPoint { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// 订单单状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 付款码数据
        /// </summary>
        public string PayAuthCode { get; set; } 
    }
}
