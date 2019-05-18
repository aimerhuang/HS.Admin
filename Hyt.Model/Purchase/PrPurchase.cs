
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 采购单
	/// </summary>
    /// <remarks>
    /// 2016-06-15 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class PrPurchase
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
		/// 采购单编号
		/// </summary>
		[Description("采购单编号")]
		public string PurchaseCode { get; set; }
 		/// <summary>
		/// 供应商编号
		/// </summary>
        [Description("供应商编号")]
		public int ManufacturerSysNo { get; set; }
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
        /// 采购总金额
		/// </summary>
        [Description("采购总金额")]
		public decimal TotalMoney { get; set; }
 		/// <summary>
		/// 状态：待审核（10）、已审核（20）、作废（-10）
		/// </summary>
		[Description("状态：待审核（10）、已审核（20）、作废（-10）")]
		public int Status { get; set; }
 		/// <summary>
		/// 付款状态：未付款（10）、已付款（20）
		/// </summary>
		[Description("付款状态：未付款（10）、已付款（20）")]
		public int PaymentStatus { get; set; }
 		/// <summary>
		/// 入库状态：入库中（10）、已入库（20）、未入库（30）
		/// </summary>
		[Description("入库状态：入库中（10）、已入库（20）、未入库（30）")]
		public int WarehousingStatus { get; set; }
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

        /// <summary>
        /// 审核用户
        /// </summary>
        public int Audit { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>
        public int DepartmentSysNo { get; set; }
        /// <summary>
        /// 业务员编号
        /// </summary>
        public int SyUserSysNo { get; set; }
        /// <summary>
        /// 保管人
        /// </summary>
        public int? custodian { get; set; }
 	}
}

	