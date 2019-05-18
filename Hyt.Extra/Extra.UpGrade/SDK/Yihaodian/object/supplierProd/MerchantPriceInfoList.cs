using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierProd
{
	[Serializable]
	public class MerchantPriceInfoList 
	{	
		/// <summary>
		/// 商家产品价格信息列表
		/// </summary>
		[XmlElement("merchantPriceInfo")]
		public List<MerchantPriceInfo>  MerchantPriceInfo{ get; set; }
	}
}
