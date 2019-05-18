
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 采购退货单
	/// </summary>
    /// <remarks>
    /// 2016-06-15 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class PrPurchaseReturn
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 仓库系统编号
		/// </summary>
		[Description("仓库系统编号")]
		public int WarehouseSysNo { get; set; }
 		/// <summary>
        /// 采购单系统编号
		/// </summary>
        [Description("采购单系统编号")]
		public int PurchaseSysNo { get; set; }
 		/// <summary>
		/// 状态 :待审核（10）、已审核（20）、作废（-10）
		/// </summary>
		[Description("状态 :待审核（10）、已审核（20）、作废（-10）")]
		public int Status { get; set; }
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
		/// 退款总金额
		/// </summary>
		[Description("退款总金额")]
		public decimal ReturnTotalMoney { get; set; }
 		/// <summary>
		/// 备注
		/// </summary>
		[Description("备注")]
		public string Remarks { get; set; }
 		/// <summary>
		/// 制单人
		/// </summary>
		[Description("制单人")]
		public int CreatedBy { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreatedDate { get; set; }
 	}
}

	