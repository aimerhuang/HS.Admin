
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 软件下载
	/// </summary>
    /// <remarks>
    /// 2014-01-14 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class FeSoftware
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 软件分类系统编号
		/// </summary>
		[Description("软件分类系统编号")]
		public int SoftCategorySysNo { get; set; }
 		/// <summary>
		/// 标题
		/// </summary>
		[Description("标题")]
		public string Title { get; set; }
 		/// <summary>
		/// 摘要
		/// </summary>
		[Description("摘要")]
		public string HeadLine { get; set; }
 		/// <summary>
		/// 介绍
		/// </summary>
		[Description("介绍")]
		public string Description { get; set; }
 		/// <summary>
		/// 显示序号
		/// </summary>
		[Description("显示序号")]
		public int DisplayOrder { get; set; }
 		/// <summary>
		/// 状态:待审(10),已审(20),作废(-10)
		/// </summary>
		[Description("状态:待审(10),已审(20),作废(-10)")]
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
 	}
}

	