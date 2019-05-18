using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	[Serializable]
	public class UnionLimitProductPriceCompareList 
	{	
		/// <summary>
		/// 比价的商家和价格，链接
		/// </summary>
		[XmlElement("union_limit_product_price_compare")]
		public List<UnionLimitProductPriceCompare>  Union_limit_product_price_compare{ get; set; }
	}
}
