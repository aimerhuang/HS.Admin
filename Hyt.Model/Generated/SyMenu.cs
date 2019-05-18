
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
	public partial class SyMenu
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 父级菜单编号
		/// </summary>
		[Description("父级菜单编号")]
		public int ParentSysNo { get; set; }
 		/// <summary>
		/// 菜单名称
		/// </summary>
		[Description("菜单名称")]
		public string MenuName { get; set; }
 		/// <summary>
		/// 菜单地址
		/// </summary>
		[Description("菜单地址")]
		public string MenuUrl { get; set; }
 		/// <summary>
		/// 菜单图标
		/// </summary>
		[Description("菜单图标")]
		public string MenuImage { get; set; }
 		/// <summary>
		/// 显示顺序
		/// </summary>
		[Description("显示顺序")]
		public int DisplayOrder { get; set; }
 		/// <summary>
		/// 是否在导航栏显示
		/// </summary>
		[Description("是否在导航栏显示")]
		public int InNavigator { get; set; }
 		/// <summary>
		/// 状态:启用(1),禁用(0)
		/// </summary>
		[Description("状态:启用(1),禁用(0)")]
		public int Status { get; set; }
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

	