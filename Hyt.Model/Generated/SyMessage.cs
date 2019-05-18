
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-10-14 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class SyMessage
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 系统用户系统编号
		/// </summary>
		[Description("系统用户系统编号")]
		public int UserSysNo { get; set; }
 		/// <summary>
		/// 发送时间
		/// </summary>
		[Description("发送时间")]
		public DateTime SendDate { get; set; }
 		/// <summary>
		/// 发送人
		/// </summary>
		[Description("发送人")]
		public int SenderSysNo { get; set; }
 		/// <summary>
		/// 信息标题
		/// </summary>
		[Description("信息标题")]
		public string MessageTitle { get; set; }
 		/// <summary>
		/// 信息内容
		/// </summary>
		[Description("信息内容")]
		public string MessageContent { get; set; }
 		/// <summary>
		/// 状态：未读（10）、已读（20）
		/// </summary>
		[Description("状态：未读（10）、已读（20）")]
		public int Status { get; set; }
 	}
}

	