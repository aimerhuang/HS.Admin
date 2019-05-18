using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Trade;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 对一笔交易添加备注(兼容淘宝) 
	/// </summary>
	public class TradeMemoAddResponse 
		: YhdResponse 
	{
		/**对一笔交易添加备注后返回其对应的Trade，Trade中可用的返回字段有tid和created */
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
