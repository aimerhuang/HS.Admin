using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Review
{
	[Serializable]
	public class ReviewReplyInfoList 
	{	
		/// <summary>
		/// 回复list
		/// </summary>
		[XmlElement("review_reply_info")]
		public List<ReviewReplyInfo>  Review_reply_info{ get; set; }
	}
}
