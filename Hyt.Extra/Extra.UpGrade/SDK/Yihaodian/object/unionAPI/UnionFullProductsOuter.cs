using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 全量下架接口返回的商品信息
	/// </summary>
	[Serializable]
	public class UnionFullProductsOuter 
	{
		/**产品id */
		[XmlElement("product_id")]
			public long?  Product_id{ get; set; }

		/**产品编码 */
		[XmlElement("product_code")]
			public string  Product_code{ get; set; }

		/**产品评论数量 */
		[XmlElement("comment_num")]
			public long?  Comment_num{ get; set; }

		/**产品名称 */
		[XmlElement("product_name")]
			public string  Product_name{ get; set; }

		/**产品副标题 */
		[XmlElement("product_subtitle")]
			public string  Product_subtitle{ get; set; }

		/**一级类目Id(前台) */
		[XmlElement("first_level_category_id")]
			public long?  First_level_category_id{ get; set; }

		/**品牌名称 */
		[XmlElement("brand_name")]
			public string  Brand_name{ get; set; }

		/**条形码 */
		[XmlElement("bar_code")]
			public string  Bar_code{ get; set; }

		/**更新时间 */
		[XmlElement("update_time")]
			public string  Update_time{ get; set; }

		/**产品主图地址 */
		[XmlElement("product_img_url")]
			public string  Product_img_url{ get; set; }

		/**产品相关的商品信息 */
		[XmlElement("union_full_products_pm_infos")]
		public UnionFullProductsPmInfoList  Union_full_products_pm_infos{ get; set; }

	}
}
