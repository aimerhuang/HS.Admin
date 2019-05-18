
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 组织机构
	/// </summary>
    /// <remarks>
    /// 2013-10-08 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class BsOrganization
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 父级系统编号
		/// </summary>
		[Description("父级系统编号")]
		public int ParentSysNo { get; set; }
 		/// <summary>
		/// 机构代码
		/// </summary>
		[Description("机构代码")]
		public string Code { get; set; }
 		/// <summary>
		/// 机构名称
		/// </summary>
		[Description("机构名称")]
		public string Name { get; set; }
 		/// <summary>
		/// 机构描述
		/// </summary>
		[Description("机构描述")]
		public string Description { get; set; }
 		/// <summary>
		/// 显示顺序
		/// </summary>
		[Description("显示顺序")]
		public int DisplayOrder { get; set; }
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

	