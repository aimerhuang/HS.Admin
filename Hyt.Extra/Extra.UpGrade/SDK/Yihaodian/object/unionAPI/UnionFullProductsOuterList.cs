using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	[Serializable]
	public class UnionFullProductsOuterList 
	{	
		/// <summary>
		/// 网盟全量判断是否下架接口返回的商品对象信息列表
		/// </summary>
		[XmlElement("union_full_products_outer")]
		public List<UnionFullProductsOuter>  Union_full_products_outer{ get; set; }
	}
}
