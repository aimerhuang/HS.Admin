
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
	public partial class PdProductAttribute
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
		public int AttributeSysNo { get; set; }
 		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int ProductSysNo { get; set; }
 		/// <summary>
		/// 属性名称
		/// </summary>
		[Description("属性名称")]
		public string AttributeName { get; set; }
 		/// <summary>
		/// 属性分组系统编号
		/// </summary>
		[Description("属性分组系统编号")]
		public int AttributeGroupSysNo { get; set; }
 		/// <summary>
		/// 属性选项编号
		/// </summary>
		[Description("属性选项编号")]
		public int AttributeOptionSysNo { get; set; }
 		/// <summary>
		/// 属性文本值
		/// </summary>
		[Description("属性文本值")]
		public string AttributeText { get; set; }
 		/// <summary>
		/// 属性图片值
		/// </summary>
		[Description("属性图片值")]
		public string AttributeImage { get; set; }
 		/// <summary>
		/// 属性类型：文本类型（10）、图片类型（20）、选项类型
		/// </summary>
		[Description("属性类型：文本类型（10）、图片类型（20）、选项类型")]
		public int AttributeType { get; set; }
 		/// <summary>
		/// 排序
		/// </summary>
		[Description("排序")]
		public int DisplayOrder { get; set; }
 		/// <summary>
		/// 状态：有效（1）、无效（0）
		/// </summary>
		[Description("状态：有效（1）、无效（0）")]
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

	