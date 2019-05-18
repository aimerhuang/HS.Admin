using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Review
{
	/// <summary>
	/// 评论回复信息
	/// </summary>
	[Serializable]
	public class ReviewReplyInfo 
	{
		/**回复人id */
		[XmlElement("reply_user_id")]
			public long?  Reply_user_id{ get; set; }

		/**回复人昵称 */
		[XmlElement("reply_user_nick_name")]
			public string  Reply_user_nick_name{ get; set; }

		/**被回复人id */
		[XmlElement("to_reply_user_id")]
			public long?  To_reply_user_id{ get; set; }

		/**被回复人昵称 */
		[XmlElement("to_reply_user_nick_name")]
			public string  To_reply_user_nick_name{ get; set; }

		/**回复内容 */
		[XmlElement("reply_content")]
			public string  Reply_content{ get; set; }

		/**回复时间 */
		[XmlElement("create_time")]
			public string  Create_time{ get; set; }

		/**回复来源0：商家回复，1：客服，2：供应商，3：用户 */
		[XmlElement("source")]
			public string  Source{ get; set; }

	}
}
