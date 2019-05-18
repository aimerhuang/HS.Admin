using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Promotion
{
	[Serializable]
	public class ActivityUsProductList 
	{	
		/// <summary>
		/// 活动关联的商品列表
		/// </summary>
		[XmlElement("activityUsProduct")]
		public List<ActivityUsProduct>  ActivityUsProduct{ get; set; }
	}
}
