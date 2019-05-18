using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	[Serializable]
	public class UnionFlashProductInfoOuterList 
	{	
		/// <summary>
		/// 名品商品集合对象
		/// </summary>
		[XmlElement("union_flash_product_info_outer")]
		public List<UnionFlashProductInfoOuter>  Union_flash_product_info_outer{ get; set; }
	}
}
