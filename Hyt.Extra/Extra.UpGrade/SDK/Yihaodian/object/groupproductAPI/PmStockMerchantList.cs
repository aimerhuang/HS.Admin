using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.GroupproductAPI
{
	[Serializable]
	public class PmStockMerchantList 
	{	
		/// <summary>
		/// 区域商家产品库存对象
		/// </summary>
		[XmlElement("pmStockMerchant")]
		public List<PmStockMerchant>  PmStockMerchant{ get; set; }
	}
}
