using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 包邮商品
	/// </summary>
	[Serializable]
	public class UnionApiGetFreeDeliveryProductOuter 
	{
		/**类目ID */
		[XmlElement("cid")]
			public long?  Cid{ get; set; }

		/**当前价格 */
		[XmlElement("current_price")]
			public double?  Current_price{ get; set; }

		/**包邮结束时间 */
		[XmlElement("free_delivery_end_time")]
			public string  Free_delivery_end_time{ get; set; }

		/**包邮开始时间 */
		[XmlElement("free_delivery_start_time")]
			public string  Free_delivery_start_time{ get; set; }

		/**手机端商品详情页URL */
		[XmlElement("item_mobile_url")]
			public string  Item_mobile_url{ get; set; }

		/**商品详情页URL */
		[XmlElement("item_url")]
			public string  Item_url{ get; set; }

		/**商家ID */
		[XmlElement("merchant_id")]
			public long?  Merchant_id{ get; set; }

		/**商家名称 */
		[XmlElement("merchant_name")]
			public string  Merchant_name{ get; set; }

		/** 图片URL */
		[XmlElement("pic_url")]
			public string  Pic_url{ get; set; }

		/**商品ID */
		[XmlElement("pm_info_id")]
			public long?  Pm_info_id{ get; set; }

		/**产品ID */
		[XmlElement("product_id")]
			public long?  Product_id{ get; set; }

		/**产品名称 */
		[XmlElement("product_name")]
			public string  Product_name{ get; set; }

		/**手机端商品详情页URL */
		[XmlElement("shop_mobile_url")]
			public string  Shop_mobile_url{ get; set; }

		/**店铺URL */
		[XmlElement("shop_url")]
			public string  Shop_url{ get; set; }

	}
}
