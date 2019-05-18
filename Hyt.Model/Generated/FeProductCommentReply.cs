
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
	public partial class FeProductCommentReply
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
		public int CommentSysNo { get; set; }
 		/// <summary>
		/// 会员编号
		/// </summary>
		[Description("会员编号")]
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 回复内容
		/// </summary>
		[Description("回复内容")]
		public string ReplyContent { get; set; }
 		/// <summary>
		/// 回复时间
		/// </summary>
		[Description("回复时间")]
		public DateTime ReplyDate { get; set; }
 		/// <summary>
		/// 状态：待审（10）、已审（20）、作废（－10）
		/// </summary>
		[Description("状态：待审（10）、已审（20）、作废（－10）")]
		public int Status { get; set; }
 	}
}

	