
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
	public partial class SyPermission
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 来源:系统用户(10),用户组(20)
		/// </summary>
		[Description("来源:系统用户(10),用户组(20)")]
		public int Source { get; set; }
 		/// <summary>
		/// 来源编号
		/// </summary>
		[Description("来源编号")]
		public int SourceSysNo { get; set; }
 		/// <summary>
		/// 目标:菜单(10),角色(20),权限(30)
		/// </summary>
		[Description("目标:菜单(10),角色(20),权限(30)")]
		public int Target { get; set; }
 		/// <summary>
		/// 目标编号
		/// </summary>
		[Description("目标编号")]
		public int TargetSysNo { get; set; }
 		/// <summary>
		/// 生效日期
		/// </summary>
		[Description("生效日期")]
		public DateTime EffectiveDate { get; set; }
 		/// <summary>
		/// 过期日期
		/// </summary>
		[Description("过期日期")]
		public DateTime ExpirationDate { get; set; }
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
 	}
}

	