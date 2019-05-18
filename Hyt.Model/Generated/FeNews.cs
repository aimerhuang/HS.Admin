
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 新闻
	/// </summary>
    /// <remarks>
    /// 2014-01-14 杨浩 T4生成
    /// 2016-06-07 罗远康 新增字段
    /// </remarks>
	[Serializable]
	public partial class FeNews
	{
	    /// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
        /// <summary>
        /// 新闻分类系统编号
        /// </summary>
        [Description("新闻分类系统编号")]
        public int CategorySysNo { get; set; }
 		/// <summary>
		/// 新闻标题
		/// </summary>
		[Description("新闻标题")]
		public string Title { get; set; }
 		/// <summary>
		/// 新闻摘要
		/// </summary>
		[Description("新闻摘要")]
		public string HeadLine { get; set; }
 		/// <summary>
		/// 新闻内容
		/// </summary>
		[Description("新闻内容")]
		public string Content { get; set; }
 		/// <summary>
		/// 文章来源
		/// </summary>
		[Description("文章来源")]
		public string Source { get; set; }
 		/// <summary>
		/// 来源网址
		/// </summary>
		[Description("来源网址")]
		public string SourceUri { get; set; }
 		/// <summary>
		/// 新闻作者
		/// </summary>
		[Description("新闻作者")]
		public string Author { get; set; }
        /// <summary>
        /// 封面图片地址
        /// </summary>
        [Description("封面图片地址")]
        public string CoverImage { get; set; }
        /// <summary>
        /// TAG标签逗号分隔
        /// </summary>
        [Description("TAG标签逗号分隔")]
        public string Tags { get; set; }
        /// <summary>
        /// 是否置顶：是（1）、否（0）
        /// </summary>
        [Description("是否置顶：是（1）、否（0）")]
        public int IsTop { get; set; }
        /// <summary>
        /// 是否热门：是（1）、否（0）
        /// </summary>
        [Description("是否热门：是（1）、否（0）")]
        public int IsHot { get; set; }
        /// <summary>
        /// 是否推荐：是（1）、否（0）
        /// </summary>
        [Description("是否推荐：是（1）、否（0）")]
        public int IsRecommend { get; set; }
 		/// <summary>
		/// 查看次数
		/// </summary>
		[Description("查看次数")]
		public int Views { get; set; }
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
        /// SEO标题
        /// </summary>
        [Description("SEO标题")]
        public string SeoTitle { get; set; }
        /// <summary>
        /// SEO关键字
        /// </summary>
        [Description("SEO关键字")]
        public string SeoKeyword { get; set; }
        /// <summary>
        /// SEO描述
        /// </summary>
        [Description("SEO描述")]
        public string SeoDescription { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        [Description("发布时间")]
        public DateTime ReleaseDate { get; set; }
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

	