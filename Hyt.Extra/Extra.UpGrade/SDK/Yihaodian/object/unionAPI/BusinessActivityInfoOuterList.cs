using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	[Serializable]
	public class BusinessActivityInfoOuterList 
	{	
		/// <summary>
		/// 商家活动信息列表
		/// </summary>
		[XmlElement("business_activity_info_outer")]
		public List<BusinessActivityInfoOuter>  Business_activity_info_outer{ get; set; }
	}
}
