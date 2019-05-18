
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 分销商退换货明细
	/// </summary>
    /// <remarks>
    /// 2013-09-10 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsReturnItem
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 分销商退换货单系统编号
		/// </summary>
		[Description("分销商退换货单系统编号")]
		public int DsReturnSysNo { get; set; }
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
		/// 商城订单明细编号(暂时不管该属性)
		/// </summary>
		[Description("商城订单明细编号")]
		public int MallItemNo { get; set; }
 		/// <summary>
		/// 数量
		/// </summary>
		[Description("数量")]
		public int Quantity { get; set; }
 		/// <summary>
		/// 退款金额
		/// </summary>
		[Description("退款金额")]
		public decimal Amount { get; set; }
 	}
}

	