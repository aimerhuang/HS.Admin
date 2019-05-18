
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 任务配置表
	/// </summary>
    /// <remarks>
    /// 2013-10-15 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class SyTaskConfig
	{
	  	/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 类型名称
		/// </summary>
		[Description("类型名称")]
		public string TypeName { get; set; }
 		/// <summary>
		/// 任务名称
		/// </summary>
		[Description("任务名称")]
		public string TaskName { get; set; }
 		/// <summary>
		/// 任务描述
		/// </summary>
		[Description("任务描述")]
		public string TaskDescription { get; set; }
 		/// <summary>
		/// 任务执行的时间类型
		/// </summary>
		[Description("任务执行的时间类型")]
		public int Timetype { get; set; }
 		/// <summary>
        /// 间隔任务的时间段
		/// </summary>
		[Description("时间段")]
		public string TimeQuantum { get; set; }
 		/// <summary>
		/// 按周的某一天执行
		/// </summary>
		[Description("按周的某一天执行")]
		public string DayOfWeek { get; set; }
 		/// <summary>
		/// 按月执行
		/// </summary>
		[Description("按月执行")]
		public string Month { get; set; }
 		/// <summary>
		/// 执行时间刻度数
		/// </summary>
		[Description("执行时间")]
		public string ExecuteTime { get; set; }
 		/// <summary>
		/// 开始时间
		/// </summary>
		[Description("开始时间")]
		public DateTime StartTime { get; set; }
 		/// <summary>
		/// 结束时间
		/// </summary>
		[Description("结束时间")]
		public DateTime EndTime { get; set; }
 		/// <summary>
		/// 是否启用结束时间:是(1);否(0)
		/// </summary>
		[Description("是否启用结束时间:是(1);否(0)")]
		public int EnabledEndTime { get; set; }
 		/// <summary>
		/// 任务的最后执行时间
		/// </summary>
		[Description("任务的最后执行时间")]
		public DateTime LastExecuteTime { get; set; }
        /// <summary>
        /// 任务的最新执行消息
        /// </summary>
        [Description("任务的最新执行消息")]
        public string LastMessage { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreateTime { get; set; }
 		/// <summary>
		/// 失败后是否尝试重连:是(1);否(0)
		/// </summary>
		[Description("失败后是否尝试重连:是(1);否(0)")]
		public int IsAgain { get; set; }
 		/// <summary>
		/// 尝试重新启动最大次数
		/// </summary>
		[Description("尝试重新启动最大次数")]
		public int MaxAgainCount { get; set; }
 		/// <summary>
		/// 失败次数
		/// </summary>
		[Description("失败次数")]
		public int FailureCount { get; set; }
 		/// <summary>
		/// 状态:启用(1);禁用(0)
		/// </summary>
		[Description("状态:启用(1);禁用(0)")]
		public int Status { get; set; }

        /// <summary>
        /// 执行参数
        /// </summary>
        public string Contextdata { get; set; }
 	}
}

	