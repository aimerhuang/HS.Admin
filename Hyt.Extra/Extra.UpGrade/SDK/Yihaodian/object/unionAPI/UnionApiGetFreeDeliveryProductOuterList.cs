using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	[Serializable]
	public class UnionApiGetFreeDeliveryProductOuterList 
	{	
		/// <summary>
		/// 包邮商品对象
		/// </summary>
		[XmlElement("union_api_get_free_delivery_product_outer")]
		public List<UnionApiGetFreeDeliveryProductOuter>  Union_api_get_free_delivery_product_outer{ get; set; }
	}
}
