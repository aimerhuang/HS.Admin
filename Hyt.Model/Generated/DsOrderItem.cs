
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 分销商升舱订单明细
	/// </summary>
    /// <remarks>
    /// 2013-09-10 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsOrderItem
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 分销商升舱订单系统编号
		/// </summary>
		[Description("分销商升舱订单系统编号")]
		public int DsOrderSysNo { get; set; }
 		/// <summary>
		/// 商城订单事物编号
		/// </summary>
		[Description("商城订单事物编号")]
		public string OrderTransactionSysNo { get; set; }
 		/// <summary>
		/// 商城商品编码
		/// </summary>
		[Description("商城商品编码")]
		public string MallProductId { get; set; }
 		/// <summary>
		/// 商城商品属性
		/// </summary>
		[Description("商城商品属性")]
		public string MallProductAttribute { get; set; }
 		/// <summary>
		/// 商城商品名称
		/// </summary>
		[Description("商城商品名称")]
		public string MallProductName { get; set; }
 		/// <summary>
		/// 商城订单明细编号
		/// </summary>
		[Description("商城订单明细编号")]
		public long MallItemNo { get; set; }
 		/// <summary>
		/// 商品价格
		/// </summary>
		[Description("商品价格")]
		public decimal Price { get; set; }
 		/// <summary>
		/// 商品数量
		/// </summary>
		[Description("商品数量")]
		public int Quantity { get; set; }
 		/// <summary>
		/// 优惠
		/// </summary>
		[Description("优惠")]
		public decimal DiscountAmount { get; set; }
 	}
}

	