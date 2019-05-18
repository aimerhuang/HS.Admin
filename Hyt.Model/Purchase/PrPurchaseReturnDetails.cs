
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 采购退货明细
	/// </summary>
    /// <remarks>
    /// 2016-06-15 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class PrPurchaseReturnDetails
	{
	    /// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 采购退货单系统编号
		/// </summary>
		[Description("采购退货单系统编号")]
		public int PurchaseReturnSysNo { get; set; }
 		/// <summary>
		/// 产品系统编号
		/// </summary>
		[Description("产品系统编号")]
		public int ProductSysNo { get; set; }
 		/// <summary>
		/// 退货数量
		/// </summary>
		[Description("退货数量")]
		public int ReturnQuantity { get; set; }
 		/// <summary>
		/// 已出库数
		/// </summary>
		[Description("已出库数")]
		public int OutQuantity { get; set; }
 		/// <summary>
		/// 实付金额
		/// </summary>
		[Description("实付金额")]
		public decimal Payment { get; set; }
 		/// <summary>
		/// 退款总金额
		/// </summary>
		[Description("退款总金额")]
		public decimal ReturnTotalMoney { get; set; }
 	}
}

	