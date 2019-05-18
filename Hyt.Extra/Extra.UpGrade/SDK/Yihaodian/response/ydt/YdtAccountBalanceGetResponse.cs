using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 获取实时余额，”元”为单位
	/// </summary>
	public class YdtAccountBalanceGetResponse 
		: YhdResponse 
	{
		/**实时余额，”元”为单位 */
		[XmlElement("balance")]
			public double?  Balance{ get; set; }

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
