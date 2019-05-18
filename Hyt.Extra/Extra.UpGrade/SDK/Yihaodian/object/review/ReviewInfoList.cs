using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Review
{
	[Serializable]
	public class ReviewInfoList 
	{	
		/// <summary>
		/// 评论列表
		/// </summary>
		[XmlElement("review_info")]
		public List<ReviewInfo>  Review_info{ get; set; }
	}
}
