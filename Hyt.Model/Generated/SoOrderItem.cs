
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2013-09-13 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class SoOrderItem
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 销售单系统编号
        /// </summary>
        [Description("销售单系统编号")]
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 事物编号(订单)
        /// </summary>
        [Description("事物编号(订单)")]
        public string TransactionSysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        [Description("商品编号")]
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Description("商品名称")]
        public string ProductName { get; set; }
        /// <summary>
        /// 订购数量
        /// </summary>
        [Description("订购数量")]
        public int Quantity { get; set; }
        /// <summary>
        /// 原单价：商品会员等级价格
        /// </summary>
        [Description("原单价：商品会员等级价格")]
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 销售单价(不包含优惠金额)
        /// </summary>
        [Description("销售单价(不包含优惠金额)")]
        public decimal SalesUnitPrice { get; set; }
        /// <summary>
        /// 销售金额(销售单价*数量)
        /// </summary>
        [Description("销售金额(销售单价*数量)")]
        public decimal SalesAmount { get; set; }
        /// <summary>
        /// 折扣金额(商品分摊折扣金额)
        /// </summary>
        [Description("折扣金额(商品分摊折扣金额)")]
        public decimal DiscountAmount { get; set; }
        /// <summary>
        /// 调价金额(正负调价金额)
        /// </summary>
        [Description("调价金额(正负调价金额)")]
        public decimal ChangeAmount { get; set; }
        /// <summary>
        /// 实际出库数量(创建出库单数量)
        /// </summary>
        [Description("实际出库数量(创建出库单数量)")]
        public int RealStockOutQuantity { get; set; }
        /// <summary>
        /// 商品销售类型：普通(10),团购(20),秒杀(30),抢购(40),
        /// </summary>
        [Description("商品销售类型：普通(10),团购(20),秒杀(30),抢购(40),")]
        public int ProductSalesType { get; set; }
        /// <summary>
        /// 商品销售类型编号
        /// </summary>
        [Description("商品销售类型编号")]
        public int ProductSalesTypeSysNo { get; set; }
        /// <summary>
        /// 组代码(组合,团购时使用)
        /// </summary>
        [Description("组代码(组合,团购时使用)")]
        public string GroupCode { get; set; }
        /// <summary>
        /// 组名称(组合,团购时使用)
        /// </summary>
        [Description("组名称(组合,团购时使用)")]
        public string GroupName { get; set; }
        /// <summary>
        /// 已使用促销系统编号(多个促销分号分隔)
        /// </summary>
        [Description("已使用促销系统编号(多个促销分号分隔)")]
        public string UsedPromotions { get; set; }

        /// <summary>
        /// 商品利润
        /// </summary>
        [Description("商品利润")]
        public decimal Catle { get; set; }
        /// <summary>
        /// 单个商品利润
        /// </summary>
        [Description("单个商品利润")]
        public decimal UnitCatle { get; set; }
        /// <summary>
        /// 商品利润
        /// </summary>
        [Description("总部销售单价")]
        public decimal OriginalSalesUnitPrice { get; set; }
        /// <summary>
        /// 返利状态
        /// </summary>
        [Description("返利状态")]
        public int RebatesStatus { get; set; }
    }
}

