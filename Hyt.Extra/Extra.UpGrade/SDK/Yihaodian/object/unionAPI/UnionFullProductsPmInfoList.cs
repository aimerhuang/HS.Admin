using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	[Serializable]
	public class UnionFullProductsPmInfoList 
	{	
		/// <summary>
		/// 产品相关的商品信息
		/// </summary>
		[XmlElement("union_full_products_pm_info")]
		public List<UnionFullProductsPmInfo>  Union_full_products_pm_info{ get; set; }
	}
}
