using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 名品商品
	/// </summary>
	[Serializable]
	public class UnionFlashProductInfoOuter 
	{
		/**卖场id */
		[XmlElement("activity_id")]
			public long?  Activity_id{ get; set; }

		/**产品所在卖场关联的品牌id */
		[XmlElement("brand_id")]
			public long?  Brand_id{ get; set; }

		/**产品id */
		[XmlElement("product_id")]
			public long?  Product_id{ get; set; }

		/**产品code */
		[XmlElement("product_code")]
			public string  Product_code{ get; set; }

		/**产品名称 */
		[XmlElement("product_name")]
			public string  Product_name{ get; set; }

		/**产品图片url */
		[XmlElement("pic_url")]
			public string  Pic_url{ get; set; }

		/**产品图片跳转url */
		[XmlElement("pic_target_url")]
			public string  Pic_target_url{ get; set; }

		/**商品id */
		[XmlElement("pm_info_id")]
			public long?  Pm_info_id{ get; set; }

		/**价格 */
		[XmlElement("price")]
			public double?  Price{ get; set; }

		/**产品折扣值 */
		[XmlElement("discount")]
			public double?  Discount{ get; set; }

		/**商家id */
		[XmlElement("merchant_id")]
			public long?  Merchant_id{ get; set; }

		/**佣金率 */
		[XmlElement("commission_rate")]
			public double?  Commission_rate{ get; set; }

		/**H5产品图片跳转url */
		[XmlElement("moblie_pic_target_url")]
			public string  Moblie_pic_target_url{ get; set; }

	}
}
