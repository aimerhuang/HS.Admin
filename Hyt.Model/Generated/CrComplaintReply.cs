
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
	public partial class CrComplaintReply
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
		public int ComplaintSysNo { get; set; }
 		/// <summary>
		/// 回复人
		/// </summary>
		[Description("回复人")]
		public int ReplyerSysNo { get; set; }
 		/// <summary>
		/// 回复内容
		/// </summary>
		[Description("回复内容")]
		public string ReplyContent { get; set; }
 		/// <summary>
		/// 回复类型：客服回复（10）、客户回复（20）
		/// </summary>
		[Description("回复类型：客服回复（10）、客户回复（20）")]
		public int ReplyerType { get; set; }
 		/// <summary>
		/// 回复时间
		/// </summary>
		[Description("回复时间")]
		public DateTime ReplyDate { get; set; }
 	}
}

	