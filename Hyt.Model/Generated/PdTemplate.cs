
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
	public partial class PdTemplate
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 模板类型：模板（10）、模块（20）
		/// </summary>
		[Description("模板类型：模板（10）、模块（20）")]
		public int Type { get; set; }
 		/// <summary>
		/// 模板名称
		/// </summary>
		[Description("模板名称")]
		public string Name { get; set; }
 		/// <summary>
		/// 模板图例
		/// </summary>
		[Description("模板图例")]
		public string Icon { get; set; }
 		/// <summary>
		/// 模板备注
		/// </summary>
		[Description("模板备注")]
		public string Remark { get; set; }
 		/// <summary>
		/// 模板内容
		/// </summary>
		[Description("模板内容")]
		public string Content { get; set; }
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

	