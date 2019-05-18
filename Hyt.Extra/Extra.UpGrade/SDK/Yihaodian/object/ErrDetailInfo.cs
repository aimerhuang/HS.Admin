using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object
{
	/// <summary>
	/// 错误信息
	/// </summary>
	[Serializable]
	public class ErrDetailInfo 
	{
		/**错误编码 */
		[XmlElement("errorCode")]
			public string  ErrorCode{ get; set; }

		/**错误描述 */
		[XmlElement("errorDes")]
			public string  ErrorDes{ get; set; }

		/**发生错误对应的数据的关键字信息 */
		[XmlElement("pkInfo")]
			public string  PkInfo{ get; set; }

	}
}
