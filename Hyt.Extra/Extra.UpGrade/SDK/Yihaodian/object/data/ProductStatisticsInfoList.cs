using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Data
{
	[Serializable]
	public class ProductStatisticsInfoList 
	{	
		/// <summary>
		/// 产品销售统计详细信息
		/// </summary>
		[XmlElement("productStatisticsInfo")]
		public List<ProductStatisticsInfo>  ProductStatisticsInfo{ get; set; }
	}
}
