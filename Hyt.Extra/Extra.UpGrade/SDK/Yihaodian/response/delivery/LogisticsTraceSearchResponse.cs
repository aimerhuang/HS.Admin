using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Delivery;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 物流流转信息查询（兼容淘宝）
	/// </summary>
	public class LogisticsTraceSearchResponse 
		: YhdResponse 
	{
		/**运单号 */
		[XmlElement("out_sid")]
			public string  Out_sid{ get; set; }

		/**物流公司名称 */
		[XmlElement("company_name")]
			public string  Company_name{ get; set; }

		/**交易号 */
		[XmlElement("tid")]
			public long?  Tid{ get; set; }

		/**订单的物流状态(暂不提供) */
		[XmlElement("status")]
			public string  Status{ get; set; }

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

		/**流转信息列表 */
		[XmlElement("trace_list")]
		public TransitStepInfoList  Trace_list{ get; set; }

	}
}
