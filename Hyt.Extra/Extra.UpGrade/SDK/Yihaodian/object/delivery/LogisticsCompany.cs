using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Delivery
{
	/// <summary>
	/// 物流公司信息
	/// </summary>
	[Serializable]
	public class LogisticsCompany 
	{
		/**物流公司标识 */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**物流公司代码 */
		[XmlElement("code")]
			public string  Code{ get; set; }

		/**物流公司简称 */
		[XmlElement("name")]
			public string  Name{ get; set; }

		/**运单号验证正则表达式 */
		[XmlElement("reg_mail_no")]
			public string  Reg_mail_no{ get; set; }

		/**快递公司查询地址 */
		[XmlElement("query_url")]
			public string  Query_url{ get; set; }

		/**状态0:启用1:关闭 */
		[XmlElement("status")]
			public int?  Status{ get; set; }

	}
}
