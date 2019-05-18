
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-08-27 杨浩 T4生成
    /// </remarks>
	[Serializable]
    [DataContract]
	public partial class CrSiteMessage
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
        [DataMember]
		public int SysNo { get; set; }
 		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
        [DataMember]
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 发送时间
		/// </summary>
		[Description("发送时间")]
        [DataMember]
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
        [DataMember]
		public string MessageTitle { get; set; }
 		/// <summary>
		/// 信息内容
		/// </summary>
		[Description("信息内容")]
        [DataMember]
		public string MessageContent { get; set; }
 		/// <summary>
		/// 状态：未读（10）、已读（20）
		/// </summary>
		[Description("状态：未读（10）、已读（20）")]
        [DataMember]
		public int Status { get; set; }
 	}
}

	