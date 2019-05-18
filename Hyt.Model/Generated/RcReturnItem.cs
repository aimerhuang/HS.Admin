
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-08-27 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class RcReturnItem
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 退换货系统编号
		/// </summary>
		[Description("退换货系统编号")]
		public int ReturnSysNo { get; set; }
 		/// <summary>
		/// 事务编号
		/// </summary>
		[Description("事务编号")]
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
		/// 退换货数量
		/// </summary>
		[Description("退换货数量")]
		public int RmaQuantity { get; set; }
 		/// <summary>
		/// 商品退换货类型:新品(10),坏品(20),二手(30)
		/// </summary>
		[Description("商品退换货类型:新品(10),坏品(20),二手(30)")]
		public int ReturnType { get; set; }
 		/// <summary>
		/// 商品退款价格类型:原价(10),自定义价格(20)
		/// </summary>
		[Description("商品退款价格类型:原价(10),自定义价格(20)")]
		public int ReturnPriceType { get; set; }
 		/// <summary>
		/// 原单价
		/// </summary>
		[Description("原单价")]
		public decimal OriginPrice { get; set; }
 		/// <summary>
		/// 实退金额(不包含遗失发票扣款金额)
		/// </summary>
		[Description("实退金额(不包含遗失发票扣款金额)")]
		public decimal RefundProductAmount { get; set; }
 		/// <summary>
		/// 出库单明细编号
		/// </summary>
		[Description("出库单明细编号")]
		public int StockOutItemSysNo { get; set; }
 		/// <summary>
		/// 退换货原因
		/// </summary>
		[Description("退换货原因")]
		public string RmaReason { get; set; }
        /// <summary>
        /// 扣除返利
        /// </summary>
        [Description("扣除返利")]
        public decimal DeductRebates { get; set; }
 	}
}

	