using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierProd
{
	[Serializable]
	public class ProductList 
	{	
		/// <summary>
		/// 产品信息列表
		/// </summary>
		[XmlElement("product")]
		public List<Product>  Product{ get; set; }
	}
}
