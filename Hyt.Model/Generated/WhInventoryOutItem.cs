
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 仓库出库单明细
	/// </summary>
    /// <remarks>
    /// 2016-06-23 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class WhInventoryOutItem
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int InventoryOutSysNo { get; set; }
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
		/// 入库数量
		/// </summary>
		[Description("出库数量")]
        public int StockOutQuantity { get; set; }
 		/// <summary>
		/// 实际入库数量
		/// </summary>
        [Description("实际出库数量")]
        public int RealStockOutQuantity { get; set; }
 		/// <summary>
		/// 来源单据明细编号(主表来源单据类型明细)
		/// </summary>
		[Description("来源单据明细编号(主表来源单据类型明细)")]
		public int SourceItemSysNo { get; set; }
 		/// <summary>
		/// 备注
		/// </summary>
		[Description("备注")]
		public string Remarks { get; set; }
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
 	}
}

	