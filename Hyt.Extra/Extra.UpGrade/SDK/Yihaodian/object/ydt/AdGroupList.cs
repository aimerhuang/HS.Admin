using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Ydt
{
	[Serializable]
	public class AdGroupList 
	{	
		/// <summary>
		/// 广告组对象数组
		/// </summary>
		[XmlElement("ad_group")]
		public List<AdGroup>  Ad_group{ get; set; }
	}
}
