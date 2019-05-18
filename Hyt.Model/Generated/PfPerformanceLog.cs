
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
	public partial class PfPerformanceLog
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 事务编号
		/// </summary>
		[Description("事务编号")]
		public string TransactionSysNo { get; set; }
 		/// <summary>
		/// 业务类型(枚举):10:评论审核20:晒单审核30:咨询回复40
		/// </summary>
		[Description("业务类型(枚举):10:评论审核20:晒单审核30:咨询回复40")]
		public int BusinessType { get; set; }
 		/// <summary>
		/// 业务来源表
		/// </summary>
		[Description("业务来源表")]
		public int SourceTable { get; set; }
 		/// <summary>
		/// 单据来源编号
		/// </summary>
		[Description("单据来源编号")]
		public int SourceSysNo { get; set; }
 		/// <summary>
		/// 状态:待审核(10),已审核(20),作废(-10)
		/// </summary>
		[Description("状态:待审核(10),已审核(20),作废(-10)")]
		public int Status { get; set; }
 		/// <summary>
		/// 操作人
		/// </summary>
		[Description("操作人")]
		public int Operator { get; set; }
 		/// <summary>
		/// 操作时间
		/// </summary>
		[Description("操作时间")]
		public DateTime OperateDate { get; set; }
 		/// <summary>
		/// 操作内容
		/// </summary>
		[Description("操作内容")]
		public string OperateRemark { get; set; }
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
		/// 审核备注
		/// </summary>
		[Description("审核备注")]
		public string AuditRemark { get; set; }
 	}
}

	