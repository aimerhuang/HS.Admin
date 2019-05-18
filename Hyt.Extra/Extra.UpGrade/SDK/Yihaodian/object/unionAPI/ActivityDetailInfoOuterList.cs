using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	[Serializable]
	public class ActivityDetailInfoOuterList 
	{	
		/// <summary>
		/// 商家活动详情接口返回的商品对象信息列表
		/// </summary>
		[XmlElement("activity_detail_info_outer")]
		public List<ActivityDetailInfoOuter>  Activity_detail_info_outer{ get; set; }
	}
}
