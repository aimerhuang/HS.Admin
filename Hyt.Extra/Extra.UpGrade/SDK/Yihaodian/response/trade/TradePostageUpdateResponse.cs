using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Trade;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 修改订单邮费价格（兼容淘宝） 
	/// </summary>
	public class TradePostageUpdateResponse 
		: YhdResponse 
	{
		/**返回trade类型，其中包含修改时间modified，修改邮费post_fee，修改后的总费用total_fee和买家实付款payment */
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
