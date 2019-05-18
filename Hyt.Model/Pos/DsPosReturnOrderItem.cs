using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    public class CBDsPosReturnOrderItem : DsPosReturnOrderItem
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string CodeBar { get; set; }
    }
    /// <summary>
    /// 退货单明细列表
    /// </summary>
    public class DsPosReturnOrderItem
    {
        
        public int SysNo { get; set; }
        /// <summary>
        /// 退货父订单编号
        /// </summary>
        public int pSysNo { get; set; }
        /// <summary>
        /// 订单明细编号
        /// </summary>
        public int OrderItemSysNo { get; set; }
        /// <summary>
        /// 退货数量
        /// </summary>
        public int ProSysNo { get; set; }
        public int ReturnNum { get; set; }

        public decimal ReturnValue { get; set; }
        public decimal ReturnTotalValue { get; set; }
    }
}
