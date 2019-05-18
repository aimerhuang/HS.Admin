using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Promotion
{
	[Serializable]
	public class ActivityUsCategoryList 
	{	
		/// <summary>
		/// 活动关联的类目信息列表
		/// </summary>
		[XmlElement("activityUsCategory")]
		public List<ActivityUsCategory>  ActivityUsCategory{ get; set; }
	}
}
