using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	[Serializable]
	public class ProductList 
	{	
		/// <summary>
		/// 产品信息
		/// </summary>
		[XmlElement("product")]
		public List<Product>  Product{ get; set; }
	}
}
