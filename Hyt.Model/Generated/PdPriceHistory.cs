
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
	public partial class PdPriceHistory
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
		public int PriceSysNo { get; set; }
 		/// <summary>
		/// 关联关系码
		/// </summary>
		[Description("关联关系码")]
		public string RelationCode { get; set; }
 		/// <summary>
		/// 原单价：商品会员等级价格
		/// </summary>
		[Description("原单价：商品会员等级价格")]
		public decimal OriginalPrice { get; set; }
 		/// <summary>
		/// 申请价格
		/// </summary>
		[Description("申请价格")]
		public decimal ApplyPrice { get; set; }
 		/// <summary>
		/// 申请人
		/// </summary>
		[Description("申请人")]
		public int ApplySysNo { get; set; }
 		/// <summary>
		/// 申请时间
		/// </summary>
		[Description("申请时间")]
		public DateTime ApplyDate { get; set; }
 		/// <summary>
		/// 审核意见
		/// </summary>
		[Description("审核意见")]
		public string Opinion { get; set; }
 		/// <summary>
		/// 审核人
		/// </summary>
		[Description("审核人")]
		public int Auditor { get; set; }
 		/// <summary>
		/// 审核时间
		/// </summary>
		[Description("审核时间")]
		public DateTime AuditDate { get; set; }
 		/// <summary>
		/// 状态：待审（10）、已审（20）、作废（－10）
		/// </summary>
		[Description("状态：待审（10）、已审（20）、作废（－10）")]
		public int Status { get; set; }
 	}
}

	