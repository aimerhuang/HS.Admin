
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
	public partial class PdAttribute
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 属性名称(前台显示用)
		/// </summary>
		[Description("属性名称(前台显示用)")]
		public string AttributeName { get; set; }
 		/// <summary>
		/// 属性名称(后台显示用)
		/// </summary>
		[Description("属性名称(后台显示用)")]
		public string BackEndName { get; set; }
 		/// <summary>
		/// 是否用做关联属性：是（1）、否（0）
		/// </summary>
		[Description("是否用做关联属性：是（1）、否（0）")]
		public int IsRelationFlag { get; set; }
 		/// <summary>
		/// 属性类型：文本类型（10）、图片类型（20）、选项类型
		/// </summary>
		[Description("属性类型：文本类型（10）、图片类型（20）、选项类型")]
		public int AttributeType { get; set; }
 		/// <summary>
		/// 是否作为搜索条件：是（1）、否（0）
		/// </summary>
		[Description("是否作为搜索条件：是（1）、否（0）")]
		public int IsSearchKey { get; set; }
 		/// <summary>
		/// 状态：启用（1）、禁用（0）
		/// </summary>
		[Description("状态：启用（1）、禁用（0）")]
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

	