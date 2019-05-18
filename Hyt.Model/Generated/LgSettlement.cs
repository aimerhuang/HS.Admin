
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-11-27 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class LgSettlement
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
		/// 配送员系统编号
		/// </summary>
		[Description("配送员系统编号")]
		public int DeliveryUserSysNo { get; set; }
 		/// <summary>
		/// 配送单商品总金额
		/// </summary>
		[Description("配送单商品总金额")]
		public decimal TotalAmount { get; set; }
 		/// <summary>
		/// 预付金额
		/// </summary>
		[Description("预付金额")]
		public decimal PaidAmount { get; set; }
 		/// <summary>
		/// 到付金额
		/// </summary>
		[Description("到付金额")]
		public decimal CODAmount { get; set; }
 		/// <summary>
		/// 状态：待结算（10）、已结算（20）、作废（－10）
		/// </summary>
		[Description("状态：待结算（10）、已结算（20）、作废（－10）")]
		public int Status { get; set; }
 		/// <summary>
		/// 备注
		/// </summary>
		[Description("备注")]
		public string Remarks { get; set; }
 		/// <summary>
		/// 审核人
		/// </summary>
		[Description("审核人")]
		public int AuditorSysNo { get; set; }
 		/// <summary>
		/// 审核时间
		/// </summary>
		[Description("审核时间")]
		public DateTime AuditDate { get; set; }
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
		/// 时间戳
		/// </summary>
		[Description("时间戳")]
		public DateTime Stamp { get; set; }
 	}
}

	