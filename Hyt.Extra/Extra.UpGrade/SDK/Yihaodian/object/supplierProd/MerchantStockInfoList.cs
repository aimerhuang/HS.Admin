using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierProd
{
	[Serializable]
	public class MerchantStockInfoList 
	{	
		/// <summary>
		/// 商家产品库存信息列表
		/// </summary>
		[XmlElement("merchantStockInfo")]
		public List<MerchantStockInfo>  MerchantStockInfo{ get; set; }
	}
}
