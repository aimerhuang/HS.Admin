using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	[Serializable]
	public class UnionApiGrouponInfoOuterList 
	{	
		/// <summary>
		/// 网盟团购信息列表
		/// </summary>
		[XmlElement("union_api_groupon_info_outer")]
		public List<UnionApiGrouponInfoOuter>  Union_api_groupon_info_outer{ get; set; }
	}
}
