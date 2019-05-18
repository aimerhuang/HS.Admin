using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	[Serializable]
	public class ProductAttrInfoList 
	{	
		/// <summary>
		/// 产品属性信息列表
		/// </summary>
		[XmlElement("productAttrInfo")]
		public List<ProductAttrInfo>  ProductAttrInfo{ get; set; }
	}
}
