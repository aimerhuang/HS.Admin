using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.User
{
	/// <summary>
	/// 用户Ticket信息
	/// </summary>
	[Serializable]
	public class Ticket 
	{
		/**1号店彩票用户的唯一标识 */
		[XmlElement("sessionId")]
			public string  SessionId{ get; set; }

		/**用户昵称字串 */
		[XmlElement("uname")]
			public string  Uname{ get; set; }

		/**过期时间字串(未格式化) */
		[XmlElement("expiredTime")]
			public string  ExpiredTime{ get; set; }

	}
}
