using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	[Serializable]
	public class UnionApiGrouponHourbuyInfoOuterList 
	{	
		/// <summary>
		/// 金牌秒杀团购信息
		/// </summary>
		[XmlElement("union_api_groupon_hourbuy_info_outer")]
		public List<UnionApiGrouponHourbuyInfoOuter>  Union_api_groupon_hourbuy_info_outer{ get; set; }
	}
}
