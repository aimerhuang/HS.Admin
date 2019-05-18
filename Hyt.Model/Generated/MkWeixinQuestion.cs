
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 微信咨询
	/// </summary>
    /// <remarks>
    /// 2013-11-05 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class MkWeixinQuestion
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 客户系统编号
		/// </summary>
		[Description("客户系统编号")]
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 微信号
		/// </summary>
		[Description("微信号")]
		public string WeixinId { get; set; }
 		/// <summary>
		/// 消息
		/// </summary>
		[Description("消息")]
		public string Messages { get; set; }
 		/// <summary>
		/// 类型:咨询(10),回复(20)
		/// </summary>
		[Description("类型:咨询(10),回复(20)")]
		public int Type { get; set; }
 		/// <summary>
		/// 回复人系统编号
		/// </summary>
		[Description("回复人系统编号")]
		public int ReplyerSysNo { get; set; }
 		/// <summary>
		/// 消息时间
		/// </summary>
		[Description("消息时间")]
		public DateTime MessagesTime { get; set; }
 		/// <summary>
		/// 状态:已读(1),未读(0)
		/// </summary>
		[Description("状态:已读(1),未读(0)")]
		public int Status { get; set; }
 	}
}

	