using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierProd
{
	[Serializable]
	public class MerchantProductInfoList 
	{	
		/// <summary>
		/// 商家产品信息列表
		/// </summary>
		[XmlElement("merchantProductInfo")]
		public List<MerchantProductInfo>  MerchantProductInfo{ get; set; }
	}
}
