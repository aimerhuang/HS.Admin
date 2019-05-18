using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Review;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询评论信息
	/// </summary>
	public class ReviewGetreviewResponse 
		: YhdResponse 
	{
		/**评论列表 */
		[XmlElement("review_info_list")]
		public ReviewInfoList  Review_info_list{ get; set; }

		/**搜索的到的评价总数 */
		[XmlElement("total_num")]
			public int?  Total_num{ get; set; }

		/**启用userHasNext返回该值；如果还有下一页返回true */
		[XmlElement("has_next")]
			public bool  Has_next{ get; set; }

		/**错误码 */
		[XmlElement("error_code")]
			public string  Error_code{ get; set; }

		/**错误描述 */
		[XmlElement("msg")]
			public string  Msg{ get; set; }

		/**子错误码 */
		[XmlElement("sub_code")]
			public string  Sub_code{ get; set; }

		/**子错误描述 */
		[XmlElement("sub_msg")]
			public string  Sub_msg{ get; set; }

	}
}
