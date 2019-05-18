
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2015-08-27 王耀发 T4生成
    /// </remarks>
	[Serializable]
    public partial class PdProductStock
	{
        /// <summary>
        /// 系统编号
        /// </summary>	
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 仓库编号
        /// </summary>	
        [Description("仓库编号")]
        public int WarehouseSysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>	
        [Description("商品编号")]
        public int PdProductSysNo { get; set; }
        /// <summary>
        /// StockQuantity
        /// </summary>	
        [Description("StockQuantity")]
        public decimal StockQuantity { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>	
        [Description("创建人")]
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>	
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>	
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>	
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        [Description("条码")]
        public string Barcode { get; set; }
        /// <summary>
        /// 采购价格
        /// </summary>
        [Description("采购价格")]
        public decimal CostPrice { get; set; }
        /// <summary>
        /// 海关备案号
        /// </summary>
        [Description("海关备案号")]
        public string CustomsNo { get; set; }
        /// <summary>
        /// 商品SKU
        /// </summary>
        [Description("商品SKU")]
        public string ProductSku { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        [Description("入库时间")]
        public string InStockTime { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }
         /// <summary>
        ///锁定的库存数
        /// </summary>
        [Description("锁定的库存数")]
        public int LockStockQuantity { get; set; }
        
    }

}

	