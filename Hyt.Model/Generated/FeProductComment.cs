
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
	public partial class FeProductComment
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 销售单编号
		/// </summary>
		[Description("销售单编号")]
		public int OrderSysNo { get; set; }
 		/// <summary>
		/// 商品编号
		/// </summary>
		[Description("商品编号")]
		public int ProductSysNo { get; set; }
 		/// <summary>
		/// 会员编号
		/// </summary>
		[Description("会员编号")]
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 标题
		/// </summary>
		[Description("标题")]
		public string Title { get; set; }
 		/// <summary>
		/// 内容
		/// </summary>
		[Description("内容")]
		public string Content { get; set; }
 		/// <summary>
		/// 评分
		/// </summary>
		[Description("评分")]
		public int Score { get; set; }
 		/// <summary>
		/// 优点
		/// </summary>
		[Description("优点")]
		public string Advantage { get; set; }
 		/// <summary>
		/// 缺点
		/// </summary>
		[Description("缺点")]
		public string Disadvantage { get; set; }
 		/// <summary>
		/// 精华：是（1）、否（0）
		/// </summary>
		[Description("精华：是（1）、否（0）")]
		public int IsBest { get; set; }
 		/// <summary>
		/// 置顶：是（1）、否（0）
		/// </summary>
		[Description("置顶：是（1）、否（0）")]
		public int IsTop { get; set; }
 		/// <summary>
		/// 有用
		/// </summary>
		[Description("有用")]
		public int SupportCount { get; set; }
 		/// <summary>
		/// 没用
		/// </summary>
		[Description("没用")]
		public int UnSupportCount { get; set; }
 		/// <summary>
		/// 回复数量
		/// </summary>
		[Description("回复数量")]
		public int ReplyCount { get; set; }
 		/// <summary>
		/// 评论时间
		/// </summary>
		[Description("评论时间")]
		public DateTime CommentTime { get; set; }
 		/// <summary>
		/// 是否评论：是（1）、否（0）
		/// </summary>
		[Description("是否评论：是（1）、否（0）")]
		public int IsComment { get; set; }
 		/// <summary>
		/// 是否晒单：是（1）、否（0）
		/// </summary>
		[Description("是否晒单：是（1）、否（0）")]
		public int IsShare { get; set; }
 		/// <summary>
		/// 晒单时间
		/// </summary>
		[Description("晒单时间")]
		public DateTime ShareTime { get; set; }
 		/// <summary>
		/// 晒单标题
		/// </summary>
		[Description("晒单标题")]
		public string ShareTitle { get; set; }
 		/// <summary>
		/// 晒单内容
		/// </summary>
		[Description("晒单内容")]
		public string ShareContent { get; set; }
 		/// <summary>
		/// 评论状态：待审（10）、已审（20）、作废（－10）
		/// </summary>
		[Description("评论状态：待审（10）、已审（20）、作废（－10）")]
		public int CommentStatus { get; set; }
 		/// <summary>
		/// 晒单状态：待审（10）、已审（20）、作废（－10）
		/// </summary>
		[Description("晒单状态：待审（10）、已审（20）、作废（－10）")]
		public int ShareStatus { get; set; }
 	}
}

	