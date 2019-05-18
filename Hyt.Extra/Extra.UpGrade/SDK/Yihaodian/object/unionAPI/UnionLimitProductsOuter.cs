using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 限时购接口返回的商品信息
	/// </summary>
	[Serializable]
	public class UnionLimitProductsOuter 
	{
		/**随机id，没有实际意义 */
		[XmlElement("id")]
			public string  Id{ get; set; }

		/**产品编号 */
		[XmlElement("product_code")]
			public string  Product_code{ get; set; }

		/**产品id */
		[XmlElement("product_id")]
			public long?  Product_id{ get; set; }

		/**产品对应类目 */
		[XmlElement("category_id")]
			public long?  Category_id{ get; set; }

		/**产品标题 */
		[XmlElement("product_title")]
			public string  Product_title{ get; set; }

		/**推荐理由 */
		[XmlElement("recommend_reason")]
			public string  Recommend_reason{ get; set; }

		/**1号店价格 */
		[XmlElement("yhd_price")]
			public double?  Yhd_price{ get; set; }

		/**市场价 */
		[XmlElement("market_price")]
			public double?  Market_price{ get; set; }

		/**抢购价 */
		[XmlElement("special_price")]
			public double?  Special_price{ get; set; }

		/**是否包邮 0.否，1.是 */
		[XmlElement("is_free_postage")]
			public int?  Is_free_postage{ get; set; }

		/**抢购件数 */
		[XmlElement("limit_num")]
			public int?  Limit_num{ get; set; }

		/**限购地区 */
		[XmlElement("limit_area")]
			public string  Limit_area{ get; set; }

		/**开始推广时间 */
		[XmlElement("prod_start_time")]
			public string  Prod_start_time{ get; set; }

		/**结束推广时间 */
		[XmlElement("prod_end_time")]
			public string  Prod_end_time{ get; set; }

		/**产品PC链接 */
		[XmlElement("pc_u_r_l")]
			public string  Pc_u_r_l{ get; set; }

		/**开始无线时间 */
		[XmlElement("mobile_u_r_l")]
			public string  Mobile_u_r_l{ get; set; }

		/**自营、商城表示：0表示商城；1表示自营 */
		[XmlElement("site_type")]
			public string  Site_type{ get; set; }

		/**佣金率 */
		[XmlElement("commission_rate")]
			public double?  Commission_rate{ get; set; }

		/**佣金 */
		[XmlElement("commission")]
			public double?  Commission{ get; set; }

		/**产品参加活动的PC链接 */
		[XmlElement("activity_pc_url")]
			public string  Activity_pc_url{ get; set; }

		/**产品参加活动的无线链接 */
		[XmlElement("activity_mobile_url")]
			public string  Activity_mobile_url{ get; set; }

		/**比价的商家和价格，链接 */
		[XmlElement("product_price_comparelist")]
		public UnionLimitProductPriceCompareList  Product_price_comparelist{ get; set; }

	}
}
