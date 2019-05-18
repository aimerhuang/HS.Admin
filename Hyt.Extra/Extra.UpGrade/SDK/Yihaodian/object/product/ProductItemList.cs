using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	[Serializable]
	public class ProductItemList 
	{	
		/// <summary>
		/// 商品信息列表
		/// </summary>
		[XmlElement("productItem")]
		public List<ProductItem>  ProductItem{ get; set; }
	}
}
