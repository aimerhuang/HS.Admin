using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Review
{
	/// <summary>
	/// 评论信息
	/// </summary>
	[Serializable]
	public class ReviewInfo 
	{
		/**评论id */
		[XmlElement("review_id")]
			public long?  Review_id{ get; set; }

		/**产品id */
		[XmlElement("product_id")]
			public long?  Product_id{ get; set; }

		/**订单id */
		[XmlElement("order_id")]
			public long?  Order_id{ get; set; }

		/**用户名称 */
		[XmlElement("user_name")]
			public string  User_name{ get; set; }

		/**评论结果 */
		[XmlElement("result")]
			public string  Result{ get; set; }

		/**评论时间 */
		[XmlElement("create_time")]
			public string  Create_time{ get; set; }

		/**评论内容 */
		[XmlElement("content")]
			public string  Content{ get; set; }

		/**用户id */
		[XmlElement("user_id")]
			public long?  User_id{ get; set; }

		/**点赞数量 */
		[XmlElement("up_num")]
			public int?  Up_num{ get; set; }

		/**回复list */
		[XmlElement("reply_list")]
		public ReviewReplyInfoList  Reply_list{ get; set; }

		/**回复数量 */
		[XmlElement("reply_count")]
			public int?  Reply_count{ get; set; }

		/**评论晒图（之间用逗号隔开） */
		[XmlElement("shines")]
			public string  Shines{ get; set; }

		/**商品描述评分 */
		[XmlElement("description_point")]
			public int?  Description_point{ get; set; }

		/**服务态度评分 */
		[XmlElement("service_point")]
			public int?  Service_point{ get; set; }

		/**物流配送评分 */
		[XmlElement("delivery_point")]
			public int?  Delivery_point{ get; set; }

		/**评论满意度评分 */
		[XmlElement("score")]
			public int?  Score{ get; set; }

		/**更新时间 */
		[XmlElement("update_time")]
			public string  Update_time{ get; set; }

	}
}
