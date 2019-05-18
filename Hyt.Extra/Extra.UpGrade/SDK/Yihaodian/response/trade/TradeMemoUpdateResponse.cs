using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Trade;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 修改一笔交易备注（兼容淘宝） 
	/// </summary>
	public class TradeMemoUpdateResponse 
		: YhdResponse 
	{
		/**更新交易的备注信息后返回的Trade，其中可用字段为tid和modified */
		[XmlElement("trade")]
		public Trade  Trade{ get; set; }

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
