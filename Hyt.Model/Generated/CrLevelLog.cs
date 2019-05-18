
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
	public partial class CrLevelLog
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
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 原等级编号
		/// </summary>
		[Description("原等级编号")]
		public int OldLevelSysNo { get; set; }
 		/// <summary>
		/// 新等级编号
		/// </summary>
		[Description("新等级编号")]
		public int NewLevelSysNo { get; set; }
 		/// <summary>
		/// 等级变更类型：经验升级（10）、交易取消降级（40）、
		/// </summary>
		[Description("等级变更类型：经验升级（10）、交易取消降级（40）、")]
		public int ChangeType { get; set; }
 		/// <summary>
		/// 等级变更描述
		/// </summary>
		[Description("等级变更描述")]
		public string ChangeDescription { get; set; }
 		/// <summary>
		/// 变更时间
		/// </summary>
		[Description("变更时间")]
		public DateTime ChangeDate { get; set; }
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
 	}
}

	