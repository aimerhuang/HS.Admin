using System.ComponentModel;

namespace Hyt.Model
{
    public class DBAtAllocationItem:AtAllocationItem
    {
        public decimal CostPrice { get; set; }
        public decimal CostAmount { get; set; }
    }
    public class AtAllocationItem
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 调拨单系统编号
        /// </summary>
        [Description("调拨单系统编号")]
        public int AllocationSysNo { get; set; }

        /// <summary>
        /// 产品系统编号
        /// </summary>
        [Description("产品系统编号")]
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        [Description("商品编码")]
        public string ErpCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Description("商品名称")]
        public string ProductName { get; set; }

        /// <summary>
        /// 调出数量
        /// </summary>
        [Description("调出数量")]
        public int Quantity { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }
    }
}
