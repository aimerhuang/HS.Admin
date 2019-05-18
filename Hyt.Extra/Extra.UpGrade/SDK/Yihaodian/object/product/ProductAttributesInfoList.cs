using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	[Serializable]
	public class ProductAttributesInfoList 
	{	
		/// <summary>
		/// 单个产品属性信息列表
		/// </summary>
		[XmlElement("productAttributesInfo")]
		public List<ProductAttributesInfo>  ProductAttributesInfo{ get; set; }
	}
}
