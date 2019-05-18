
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 软件分类
	/// </summary>
    /// <remarks>
    /// 2014-01-14 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class FeSoftCategory
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 分类名称
		/// </summary>
		[Description("分类名称")]
		public string Name { get; set; }
 		/// <summary>
		/// 显示序号
		/// </summary>
		[Description("显示序号")]
		public int DisplayOrder { get; set; }
 		/// <summary>
		/// 状态:启用(1),禁用(0)
		/// </summary>
		[Description("状态:启用(1),禁用(0)")]
		public int Status { get; set; }
 	}
}

	