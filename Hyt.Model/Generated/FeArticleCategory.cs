
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
	public partial class FeArticleCategory
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 分类类型：新闻（10）、帮助（20）
		/// </summary>
		[Description("分类类型：新闻（10）、帮助（20）")]
		public int Type { get; set; }
 		/// <summary>
		/// 名称
		/// </summary>
		[Description("名称")]
		public string Name { get; set; }
 		/// <summary>
		/// 分类描述
		/// </summary>
		[Description("分类描述")]
		public string Description { get; set; }
 		/// <summary>
		/// 显示顺序
		/// </summary>
		[Description("显示顺序")]
		public int DisplayOrder { get; set; }
 		/// <summary>
		/// 状态：待审（10）、已审（20）、作废（－10）
		/// </summary>
		[Description("状态：待审（10）、已审（20）、作废（－10）")]
		public int Status { get; set; }
 	}
}

	