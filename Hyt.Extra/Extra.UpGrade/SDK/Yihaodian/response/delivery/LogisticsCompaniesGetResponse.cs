using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Delivery;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询物流公司信息（兼容淘宝）
	/// </summary>
	public class LogisticsCompaniesGetResponse 
		: YhdResponse 
	{
		/**物流公司信息。返回的LogisticCompany包含的具体信息为入参fields请求的字段信息。 */
		[XmlElement("logistics_companies")]
		public LogisticsCompanyList  Logistics_companies{ get; set; }

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
