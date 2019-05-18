using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	[Serializable]
	public class ProductDescInfoList 
	{	
		/// <summary>
		/// 产品文描信息列表
		/// </summary>
		[XmlElement("productDescInfo")]
		public List<ProductDescInfo>  ProductDescInfo{ get; set; }
	}
}
