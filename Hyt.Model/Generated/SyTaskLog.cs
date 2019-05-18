
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 任务日志表
	/// </summary>
    /// <remarks>
    /// 2013-10-15 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class SyTaskLog
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 任务配置系统编号
		/// </summary>
		[Description("任务配置系统编号")]
		public int TaskConfigSysNo { get; set; }
 		/// <summary>
		/// 执行开始时间
		/// </summary>
		[Description("执行开始时间")]
		public DateTime ExecuteStartTime { get; set; }
 		/// <summary>
		/// 执行结束时间
		/// </summary>
		[Description("执行结束时间")]
		public DateTime ExecuteEndTime { get; set; }
 		/// <summary>
		/// 执行消息
		/// </summary>
		[Description("执行消息")]
		public string ExecuteMessage { get; set; }
 		/// <summary>
		/// 任务执行状态
		/// </summary>
		[Description("任务执行状态")]
		public int Status { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreateTime { get; set; }
 	}
}

	