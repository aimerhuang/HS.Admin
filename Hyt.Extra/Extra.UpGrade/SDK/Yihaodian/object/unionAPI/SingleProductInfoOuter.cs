using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 单品接口返回的商品信息
	/// </summary>
	[Serializable]
	public class SingleProductInfoOuter 
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

		/**站点：1，1号店自营商品。2，商城商品 */
		[XmlElement("site_type")]
			public int?  Site_type{ get; set; }

		/**1号店价格 */
		[XmlElement("yhd_price")]
			public double?  Yhd_price{ get; set; }

		/**市场价 */
		[XmlElement("market_price")]
			public double?  Market_price{ get; set; }

		/**折扣 */
		[XmlElement("discount")]
			public double?  Discount{ get; set; }

		/**佣金率 */
		[XmlElement("commission_rate")]
			public double?  Commission_rate{ get; set; }

		/**佣金 */
		[XmlElement("commission")]
			public double?  Commission{ get; set; }

		/**质量得分 */
		[XmlElement("prod_quality_score")]
			public double?  Prod_quality_score{ get; set; }

		/**产品图片链接 */
		[XmlElement("product_pic_url")]
			public string  Product_pic_url{ get; set; }

	}
}
