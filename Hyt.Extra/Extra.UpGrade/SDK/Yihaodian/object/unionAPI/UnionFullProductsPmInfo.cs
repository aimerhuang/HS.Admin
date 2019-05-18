using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 全量下架接口返回关于pm信息
	/// </summary>
	[Serializable]
	public class UnionFullProductsPmInfo 
	{
		/**商详页Id */
		[XmlElement("pm_info_id")]
			public long?  Pm_info_id{ get; set; }

		/**商详页url */
		[XmlElement("product_item_url")]
			public string  Product_item_url{ get; set; }

		/**当前1号店价格 */
		[XmlElement("current_price")]
			public double?  Current_price{ get; set; }

		/**1号店市场价 */
		[XmlElement("market_price")]
			public double?  Market_price{ get; set; }

		/**促销价格（当未参与促销，促销价为0） */
		[XmlElement("promotion_price")]
			public double?  Promotion_price{ get; set; }

		/**是否还有库存 1，有库存。0，没有库存 */
		[XmlElement("is_have_stock")]
			public int?  Is_have_stock{ get; set; }

		/**1，自营。2，商城 */
		[XmlElement("site_type")]
			public int?  Site_type{ get; set; }

		/**商品佣金率 */
		[XmlElement("commission_rate")]
			public double?  Commission_rate{ get; set; }

		/**产品销售区域 */
		[XmlElement("merchant_area")]
			public string  Merchant_area{ get; set; }

		/**是否该商品已经下架 1.是。0，否 */
		[XmlElement("is_pulled_off_shelves")]
			public int?  Is_pulled_off_shelves{ get; set; }

		/**H5商详页url */
		[XmlElement("mobile_product_item_url")]
			public string  Mobile_product_item_url{ get; set; }

	}
}
