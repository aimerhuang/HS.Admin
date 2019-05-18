using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	[Serializable]
	public class UnionApiPromotionInfoOuterList 
	{	
		/// <summary>
		/// 剁手价信息
		/// </summary>
		[XmlElement("union_api_promotion_info_outer")]
		public List<UnionApiPromotionInfoOuter>  Union_api_promotion_info_outer{ get; set; }
	}
}
