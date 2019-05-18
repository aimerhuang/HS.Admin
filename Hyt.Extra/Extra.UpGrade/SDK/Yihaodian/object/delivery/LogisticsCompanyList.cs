using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Delivery
{
	[Serializable]
	public class LogisticsCompanyList 
	{	
		/// <summary>
		/// 物流公司信息。返回的LogisticCompany包含的具体信息为入参fields请求的字段信息。
		/// </summary>
		[XmlElement("logistics_company")]
		public List<LogisticsCompany>  Logistics_company{ get; set; }
	}
}
