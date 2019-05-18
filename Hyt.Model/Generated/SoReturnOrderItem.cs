
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// RMA销售单关系表
    /// </summary>
    /// <remarks>
    /// 2013-08-27 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class SoReturnOrderItem
    {
        /// <summary>
        /// SysNo
        /// </summary>	
        [Description("SysNo")]
        public int SysNo { get; set; }
        /// <summary>
        /// RMA销售单明细编号
        /// </summary>
        [Description("RMA销售单明细编号")]
        public int OrderItemSysNo { get; set; }

        /// <summary>
        /// 事物编号(订单)
        /// </summary>
        [Description("事物编号(订单)")]
        public string TransactionSysNo { get; set; }

        /// <summary>
        /// 来源出库单明细编号
        /// </summary>
        [Description("来源出库单明细编号")]
        public int FromStockOutItemSysNo { get; set; }


        /// <summary>
        /// 来源出库单明细销售金额
        /// </summary>
        [Description("来源出库单明细销售金额")]
        public decimal FromStockOutItemAmount { get; set; }

        /// <summary>
        /// 来源出库单明细商品数量
        /// </summary>
        [Description("来源出库单明细商品数量")]
        public int FromStockOutItemQuantity { get; set; }

        /// <summary>
        /// RMA销售单明细商品数量
        /// </summary>
        [Description("RMA销售单明细商品数量")]
        public int OrderItemQuantity { get; set; }

    }
}
