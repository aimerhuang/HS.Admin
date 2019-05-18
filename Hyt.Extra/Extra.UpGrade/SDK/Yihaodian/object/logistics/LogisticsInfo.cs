using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Logistics
{
	/// <summary>
	/// 物流信息
	/// </summary>
	[Serializable]
	public class LogisticsInfo 
	{
		/**快递公司ID(可能发生变化) */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**快递公司名称 */
		[XmlElement("companyName")]
			public string  CompanyName{ get; set; }

		/**快递公司查询地址 */
		[XmlElement("queryURL")]
			public string  QueryURL{ get; set; }

		/**状态0:启用1:关闭 */
		[XmlElement("status")]
			public int?  Status{ get; set; }

	}
}
