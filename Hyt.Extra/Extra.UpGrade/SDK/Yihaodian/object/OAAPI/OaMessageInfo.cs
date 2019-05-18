using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.OAAPI
{
	/// <summary>
	/// 消息推送信息
	/// </summary>
	[Serializable]
	public class OaMessageInfo 
	{
		/**消息id */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**消息内容 */
		[XmlElement("messageContent")]
			public string  MessageContent{ get; set; }

		/**消息链接 */
		[XmlElement("messageUrl")]
			public string  MessageUrl{ get; set; }

	}
}
