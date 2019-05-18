using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	[Serializable]
	public class PmStockInfoList 
	{	
		/// <summary>
		/// 所有仓库库存信息
		/// </summary>
		[XmlElement("pmStockInfo")]
		public List<PmStockInfo>  PmStockInfo{ get; set; }
	}
}
