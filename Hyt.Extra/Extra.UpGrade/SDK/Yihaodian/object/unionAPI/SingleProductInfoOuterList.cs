using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	[Serializable]
	public class SingleProductInfoOuterList 
	{	
		/// <summary>
		/// 单品接口返回的商品对象信息列表
		/// </summary>
		[XmlElement("single_product_info_outer")]
		public List<SingleProductInfoOuter>  Single_product_info_outer{ get; set; }
	}
}
