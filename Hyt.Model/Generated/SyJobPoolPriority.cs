
using System;
namespace Hyt.Model
{
    /// <summary>
	/// 任务池优先级
	/// </summary>
    /// <remarks>
    /// 2014-02-28 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class SyJobPoolPriority : BaseEntity
	{
	  
		/// <summary>
		/// 系统编号
		/// </summary>
		public int SysNo { get; set; }
 
		/// <summary>
		/// 优先级名称
		/// </summary>
		public string PriorityDescription { get; set; }
 
		/// <summary>
		/// 任务优先级
		/// </summary>
		public int Priority { get; set; }
 
		/// <summary>
		/// 优先级编码
		/// </summary>
		public string PriorityCode { get; set; }
 	}
}

	