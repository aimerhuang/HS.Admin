
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 采购明细
	/// </summary>
    /// <remarks>
    /// 2016-06-15 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class PrPurchaseDetails
	{
	    /// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 采购单系统编号
		/// </summary>
		[Description("采购单系统编号")]
		public int PurchaseSysNo { get; set; }
 		/// <summary>
		/// 产品系统编号
		/// </summary>
		[Description("产品系统编号")]
		public int ProductSysNo { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [Description("产品名称")]
        public string ProductName { get; set; }
        /// <summary>
        /// 产品ERP编码
        /// </summary>
        [Description("产品ERP编码")]
        public string ErpCode { get; set; }
 		/// <summary>
		/// 采购数
		/// </summary>
		[Description("采购数")]
		public int Quantity { get; set; }
 		/// <summary>
		/// 已入库数
		/// </summary>
		[Description("已入库数")]
		public int EnterQuantity { get; set; }
 		/// <summary>
		/// 采购价
		/// </summary>
		[Description("采购价")]
		public decimal Money { get; set; }
 		/// <summary>
		/// 采购总金额
		/// </summary>
		[Description("采购总金额")]
		public decimal TotalMoney { get; set; }
 		/// <summary>
		/// 备注
		/// </summary>
		[Description("备注")]
		public string Remarks { get; set; }
 	}
}

	