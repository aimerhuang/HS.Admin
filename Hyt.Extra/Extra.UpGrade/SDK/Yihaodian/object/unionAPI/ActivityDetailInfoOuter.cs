using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 商家活动详情接口返回的商品信息
	/// </summary>
	[Serializable]
	public class ActivityDetailInfoOuter 
	{
		/**产品id */
		[XmlElement("product_id")]
			public long?  Product_id{ get; set; }

		/**产品名称 */
		[XmlElement("product_name")]
			public string  Product_name{ get; set; }

		/**产品无线链接 */
		[XmlElement("product_url_for_phone")]
			public string  Product_url_for_phone{ get; set; }

		/**产品PC链接 */
		[XmlElement("product_url")]
			public string  Product_url{ get; set; }

		/**产品图片链接 */
		[XmlElement("product_pic_url")]
			public string  Product_pic_url{ get; set; }

		/**站点：1，1号店自营商品。2，商城商品 */
		[XmlElement("site_type")]
			public int?  Site_type{ get; set; }

		/**1号店价格 */
		[XmlElement("yhd_price")]
			public double?  Yhd_price{ get; set; }

		/**市场价 */
		[XmlElement("market_price")]
			public double?  Market_price{ get; set; }

		/**佣金率 */
		[XmlElement("commission_rate")]
			public double?  Commission_rate{ get; set; }

		/**佣金 */
		[XmlElement("commission")]
			public double?  Commission{ get; set; }

		/**商家id */
		[XmlElement("merchant_id")]
			public long?  Merchant_id{ get; set; }

		/**推广类型:1PC品台、2无线平台、3pc和无线 */
		[XmlElement("expand_type")]
			public int?  Expand_type{ get; set; }

		/**通用类目佣金率 */
		[XmlElement("category_comm_rate")]
			public double?  Category_comm_rate{ get; set; }

		/**佣金冻结时间 */
		[XmlElement("freeze_time")]
			public string  Freeze_time{ get; set; }

		/**商品的审核状态：0-未审核，1-审核通过，2-审核不通过 */
		[XmlElement("product_check_status")]
			public int?  Product_check_status{ get; set; }

		/**商品id */
		[XmlElement("pm_info_id")]
			public long?  Pm_info_id{ get; set; }

		/**商品对应店铺名称 */
		[XmlElement("store_name")]
			public string  Store_name{ get; set; }

	}
}
