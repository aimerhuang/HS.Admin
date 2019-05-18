using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 名品卖场
	/// </summary>
	[Serializable]
	public class UnionFlashBuyInfoOuter 
	{
		/**卖场ID */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**品牌ID */
		[XmlElement("brand_id")]
			public long?  Brand_id{ get; set; }

		/**品牌CODE */
		[XmlElement("brand_code")]
			public string  Brand_code{ get; set; }

		/**开始时间 */
		[XmlElement("start_time")]
			public string  Start_time{ get; set; }

		/**结束时间 */
		[XmlElement("end_time")]
			public string  End_time{ get; set; }

		/**卖场介绍信息 */
		[XmlElement("story")]
			public string  Story{ get; set; }

		/**所属类目名称 */
		[XmlElement("category_name")]
			public string  Category_name{ get; set; }

		/**卖场的logo图片，可以理解成品牌logo图 */
		[XmlElement("logo_pic")]
			public string  Logo_pic{ get; set; }

		/**如果存在值，卖场logo图片点击跳转的目标url */
		[XmlElement("logo_url")]
			public string  Logo_url{ get; set; }

		/**首页卖场大图Url，即卖场主图 卖场页面的小图 */
		[XmlElement("big_pic")]
			public string  Big_pic{ get; set; }

		/**如果存在值，卖场主图片点击跳转的目标url */
		[XmlElement("big_pic_url")]
			public string  Big_pic_url{ get; set; }

		/**卖场页产品列表上方横幅大图片,具体卖场页面的大图 */
		[XmlElement("product_com_pic")]
			public string  Product_com_pic{ get; set; }

		/**如果存在值，卖场横幅图片点击跳转的目标url */
		[XmlElement("product_com_url")]
			public string  Product_com_url{ get; set; }

		/**折扣类型：1:XX折起2:XX折封顶3:XX元起4:XX元封顶 */
		[XmlElement("discount_type")]
			public int?  Discount_type{ get; set; }

		/**折扣信息，用于显示卖场上的 */
		[XmlElement("discount_info")]
			public string  Discount_info{ get; set; }

		/**创建时间 */
		[XmlElement("create_time")]
			public string  Create_time{ get; set; }

		/**修改时间 */
		[XmlElement("update_time")]
			public string  Update_time{ get; set; }

		/**卖场排序字段 */
		[XmlElement("seq_no")]
			public int?  Seq_no{ get; set; }

		/**品牌名称 */
		[XmlElement("brand_name")]
			public string  Brand_name{ get; set; }

		/**品牌描述 */
		[XmlElement("brand_desc")]
			public string  Brand_desc{ get; set; }

		/**卖场名称，标题信息 */
		[XmlElement("market_place")]
			public string  Market_place{ get; set; }

		/**卖场前置标题，即每个产品的标题前均添加字样 */
		[XmlElement("appendix_text")]
			public string  Appendix_text{ get; set; }

		/**关联的品牌库品牌信息id */
		[XmlElement("brand_info_id")]
			public long?  Brand_info_id{ get; set; }

		/**卖场售罄率，表示卖场的商品已售完的比例 */
		[XmlElement("activity_soldout_rate")]
			public double?  Activity_soldout_rate{ get; set; }

	}
}
