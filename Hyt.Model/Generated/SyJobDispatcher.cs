
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
	public partial class SyJobDispatcher
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
		public int UserSysNo { get; set; }
 		/// <summary>
		/// 任务对象类型：客服订单审核（10）、客服订单提交出库
		/// </summary>
		[Description("任务对象类型：客服订单审核（10）、客服订单提交出库")]
		public int TaskType { get; set; }
 		/// <summary>
		/// 状态：开启自动分配（1）、关闭自动分配（0）
		/// </summary>
		[Description("状态：开启自动分配（1）、关闭自动分配（0）")]
		public int Status { get; set; }
        /// <summary>
        /// 开始优先级
        /// </summary>
        [Description("开始优先级")]
        public int BeginPriority { get; set; }

        /// <summary>
        /// 结束优先级
        /// </summary>
        [Description("结束优先级")]
        public int EndPriority { get; set; }

        /// <summary>
        /// 可执行优先级(多优先级分号分隔)
        /// </summary>
        public string Prioritys { get; set; }

        /// <summary>
        /// 最大任务数
        /// </summary>
        [Description("最大任务数")]
        public int MaxTaskQuantity { get; set; }
	}
}

	