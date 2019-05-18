using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	[Serializable]
	public class UnionLimitProductsOuterList 
	{	
		/// <summary>
		/// 限时购接口返回的商品对象信息列表
		/// </summary>
		[XmlElement("union_limit_products_outer")]
		public List<UnionLimitProductsOuter>  Union_limit_products_outer{ get; set; }
	}
}
