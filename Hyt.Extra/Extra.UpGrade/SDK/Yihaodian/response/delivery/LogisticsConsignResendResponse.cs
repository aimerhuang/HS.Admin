using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Delivery;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 修改物流公司和运单号（兼容淘宝）
	/// </summary>
	public class LogisticsConsignResendResponse 
		: YhdResponse 
	{
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

		/**物流信息列表 */
		[XmlElement("shipping")]
		public Shipping  Shipping{ get; set; }

	}
}
