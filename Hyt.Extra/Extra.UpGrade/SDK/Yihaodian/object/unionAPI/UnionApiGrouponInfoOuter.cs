using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 网盟团购信息出对象
	/// </summary>
	[Serializable]
	public class UnionApiGrouponInfoOuter 
	{
		/**id */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**团购id */
		[XmlElement("groupon_id")]
			public long?  Groupon_id{ get; set; }

		/**团购名称 */
		[XmlElement("name")]
			public string  Name{ get; set; }

		/**团购状态 */
		[XmlElement("status")]
			public int?  Status{ get; set; }

		/**商家id */
		[XmlElement("merchant_id")]
			public long?  Merchant_id{ get; set; }

		/**产品id */
		[XmlElement("product_id")]
			public long?  Product_id{ get; set; }

		/**商品id */
		[XmlElement("pm_info_id")]
			public long?  Pm_info_id{ get; set; }

		/**品牌id */
		[XmlElement("brand_id")]
			public long?  Brand_id{ get; set; }

		/**品牌名称 */
		[XmlElement("brand_name")]
			public string  Brand_name{ get; set; }

		/**折后价 */
		[XmlElement("discount_price")]
			public double?  Discount_price{ get; set; }

		/**标价 */
		[XmlElement("original_price")]
			public double?  Original_price{ get; set; }

		/**折扣 */
		[XmlElement("discount")]
			public double?  Discount{ get; set; }

		/**参团人数 */
		[XmlElement("people_number")]
			public int?  People_number{ get; set; }

		/**团购产品图片URL(首页图片300 X 200) */
		[XmlElement("image_url")]
			public string  Image_url{ get; set; }

		/**站点，1：1号店自营，2：1号店商城 */
		[XmlElement("site_type")]
			public int?  Site_type{ get; set; }

		/**产品团购详情URL */
		[XmlElement("product_sale_url")]
			public string  Product_sale_url{ get; set; }

		/**1.新品(今日上新)、2热卖（今日团购） */
		[XmlElement("category_type")]
			public int?  Category_type{ get; set; }

		/**团购修改时间 */
		[XmlElement("refresh_time")]
			public string  Refresh_time{ get; set; }

		/**团购开始时间 */
		[XmlElement("sale_start_time")]
			public string  Sale_start_time{ get; set; }

		/**团购结束时间 */
		[XmlElement("sale_end_time")]
			public string  Sale_end_time{ get; set; }

		/**团购类目id */
		[XmlElement("category_id")]
			public long?  Category_id{ get; set; }

		/**佣金率 */
		[XmlElement("commission_rate")]
			public string  Commission_rate{ get; set; }

		/**省份id */
		[XmlElement("province_id")]
			public long?  Province_id{ get; set; }

		/**省份名称 */
		[XmlElement("province_name")]
			public string  Province_name{ get; set; }

		/**免邮类型。0：不显示包邮信息；1：包邮；2：满百包邮 */
		[XmlElement("free_ship_type")]
			public int?  Free_ship_type{ get; set; }

		/**商品是否分佣。0：不分佣；1：分佣。 */
		[XmlElement("is_merchant_cps")]
			public int?  Is_merchant_cps{ get; set; }

		/**无线端产品URL */
		[XmlElement("mobile_sale_url")]
			public string  Mobile_sale_url{ get; set; }

	}
}
