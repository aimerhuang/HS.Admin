
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
	public partial class WhStockOutItem
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
		public int StockOutSysNo { get; set; }
 		/// <summary>
		/// 事务编号
		/// </summary>
		[Description("事务编号")]
		public string TransactionSysNo { get; set; }
 		/// <summary>
		/// 订单编号
		/// </summary>
		[Description("订单编号")]
		public int OrderSysNo { get; set; }
 		/// <summary>
		/// 订单明细编号
		/// </summary>
		[Description("订单明细编号")]
		public int OrderItemSysNo { get; set; }
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
		/// 商品数量
		/// </summary>
		[Description("商品数量")]
		public int ProductQuantity { get; set; }
 		/// <summary>
		/// 退换货数量
		/// </summary>
		[Description("退换货数量")]
		public int ReturnQuantity { get; set; }
 		/// <summary>
		/// 重量
		/// </summary>
		[Description("重量")]
		public decimal Weight { get; set; }
 		/// <summary>
		/// 尺寸
		/// </summary>
		[Description("尺寸")]
		public string Measurement { get; set; }
 		/// <summary>
		/// 原单价：商品会员等级价格
		/// </summary>
		[Description("原单价：商品会员等级价格")]
		public decimal OriginalPrice { get; set; }
 		/// <summary>
		/// 实际销售金额
		/// </summary>
		[Description("实际销售金额")]
		public decimal RealSalesAmount { get; set; }
 		/// <summary>
		/// 备注
		/// </summary>
		[Description("备注")]
		public string Remarks { get; set; }
 		/// <summary>
		/// 状态：有效（1）、无效（0）
		/// </summary>
		[Description("状态：有效（1）、无效（0）")]
		public int Status { get; set; }
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
        /// 库位系统编号
        /// </summary>
        [Description("库位系统编号")]
        public int WarehousePositionSysNo { get; set; }
    

 	}
}

	