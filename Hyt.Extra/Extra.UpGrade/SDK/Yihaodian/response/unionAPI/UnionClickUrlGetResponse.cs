using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 获取加密推广链接
	/// </summary>
	public class UnionClickUrlGetResponse 
		: YhdResponse 
	{
		/**加密推广链接 */
		[XmlElement("click_url")]
			public string  Click_url{ get; set; }

		/**接口调用状态码 */
		[XmlElement("status_code")]
			public string  Status_code{ get; set; }

		/**接口调用状态信息 */
		[XmlElement("error_message")]
			public string  Error_message{ get; set; }

		/**错误码 */
		[XmlElement("error_code")]
			public string  Error_code{ get; set; }

		/**子错误描述 */
		[XmlElement("sub_msg")]
			public string  Sub_msg{ get; set; }

		/**错误描述 */
		[XmlElement("msg")]
			public string  Msg{ get; set; }

		/**子错误码 */
		[XmlElement("sub_code")]
			public string  Sub_code{ get; set; }

	}
}
