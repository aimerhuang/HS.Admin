
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
	public partial class SyJobPool
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 任务描述
		/// </summary>
		[Description("任务描述")]
		public string JobDescription { get; set; }
 		/// <summary>
		/// 任务URL
		/// </summary>
		[Description("任务URL")]
		public string JobUrl { get; set; }
 		/// <summary>
        /// 任务对象类型：客服订单审核（10）、客服订单提交出库（15）、商品评论审核（50）、商品评论回复审核（55）、商品晒单审核（60）
		/// </summary>
        [Description("任务对象类型：客服订单审核（10）、客服订单提交出库（15）、商品评论审核（50）、商品评论回复审核（55）、商品晒单审核（60）")]
		public int TaskType { get; set; }
 		/// <summary>
		/// 任务对象编号
		/// </summary>
		[Description("任务对象编号")]
		public int TaskSysNo { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreatedDate { get; set; }
 		/// <summary>
		/// 任务执行者编号
		/// </summary>
		[Description("任务执行者编号")]
		public int ExecutorSysNo { get; set; }
 		/// <summary>
		/// 任务开始时间
		/// </summary>
		[Description("任务开始时间")]
		public DateTime BeginDate { get; set; }
 		/// <summary>
		/// 任务预计结束时间
		/// </summary>
		[Description("任务预计结束时间")]
		public DateTime EndDate { get; set; }
 		/// <summary>
		/// 任务分配者编号
		/// </summary>
		[Description("任务分配者编号")]
		public int AssignerSysNo { get; set; }
 		/// <summary>
		/// 任务分配时间
		/// </summary>
		[Description("任务分配时间")]
		public DateTime AssignDate { get; set; }
 		/// <summary>
		/// 状态：待分配（10）、待处理（20）、处理中（30）、已
		/// </summary>
		[Description("状态：待分配（10）、待处理（20）、处理中（30）、已")]
		public int Status { get; set; }

        /// <summary>
        /// 任务优先级
        /// </summary>
        [Description("任务优先级")]
        public int Priority { get; set; }

        /// <summary>
		/// 备注
		/// </summary>
        [Description("备注")]
        public string Remarks { get; set; }
 	}
}

	