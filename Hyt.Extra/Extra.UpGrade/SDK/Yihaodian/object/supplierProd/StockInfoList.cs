using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierProd
{
	[Serializable]
	public class StockInfoList 
	{	
		/// <summary>
		/// 产品库存信息列表
		/// </summary>
		[XmlElement("stockInfo")]
		public List<StockInfo>  StockInfo{ get; set; }
	}
}
