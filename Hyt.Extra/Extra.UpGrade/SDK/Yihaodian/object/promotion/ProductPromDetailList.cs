using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Promotion
{
	[Serializable]
	public class ProductPromDetailList 
	{	
		/// <summary>
		/// 子品促销信息列表
		/// </summary>
		[XmlElement("productPromDetail")]
		public List<ProductPromDetail>  ProductPromDetail{ get; set; }
	}
}
