
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
    public partial class PdCategorySql
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 父级编号
		/// </summary>
		[Description("父级编号")]
		public int ParentSysNo { get; set; }
        /// <summary>
        /// 系统编号路由(,上级系统编号,...,系统编号,)
        /// </summary>
        [Description("系统编号路由")]
        public string SysNos { get; set; }
 		/// <summary>
		/// 分类名称
		/// </summary>
		[Description("分类名称")]
		public string CategoryName { get; set; }
 		/// <summary>
		/// 代码
		/// </summary>
		[Description("代码")]
		public string Code { get; set; }
 		/// <summary>
		/// 分类图片地址
		/// </summary>
		[Description("分类图片地址")]
		public string CategoryImage { get; set; }
 		/// <summary>
		/// SeoTitle
		/// </summary>
		[Description("SeoTitle")]
		public string SeoTitle { get; set; }
 		/// <summary>
		/// SeoKeyword
		/// </summary>
		[Description("SeoKeyword")]
		public string SeoKeyword { get; set; }
 		/// <summary>
		/// SeoDescription
		/// </summary>
		[Description("SeoDescription")]
		public string SeoDescription { get; set; }
 		/// <summary>
		/// 属性模板编号
		/// </summary>
		[Description("属性模板编号")]
		public int TemplateSysNo { get; set; }
 		/// <summary>
		/// 是否前台展示：是（1）、否（0）
		/// </summary>
		[Description("是否前台展示：是（1）、否（0）")]
		public int IsOnline { get; set; }
 		/// <summary>
		/// 排序
		/// </summary>
		[Description("排序")]
		public int DisplayOrder { get; set; }
 		/// <summary>
		/// 备注
		/// </summary>
		[Description("备注")]
		public string Remarks { get; set; }
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
 		/// <summary>
		/// 状态：有效（1）、无效（0）
		/// </summary>
		[Description("状态：有效（1）、无效（0）")]
		public int Status { get; set; }
 	}
}

	