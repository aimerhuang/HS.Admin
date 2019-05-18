
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
	public partial class SyPrivilege
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 权限代码
		/// </summary>
		[Description("权限代码")]
		public string Code { get; set; }
 		/// <summary>
		/// 权限名称
		/// </summary>
		[Description("权限名称")]
		public string Name { get; set; }
 		/// <summary>
		/// 描述
		/// </summary>
		[Description("描述")]
		public string Description { get; set; }
 		/// <summary>
		/// 状态:启用(1),禁用(0)
		/// </summary>
		[Description("状态:启用(1),禁用(0)")]
		public int Status { get; set; }
 	}
}

	