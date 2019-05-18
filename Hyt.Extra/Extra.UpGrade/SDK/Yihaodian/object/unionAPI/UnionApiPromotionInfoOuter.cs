using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 剁手价
	/// </summary>
	[Serializable]
	public class UnionApiPromotionInfoOuter 
	{
		/**促销ID */
		[XmlElement("promotion_id")]
			public long?  Promotion_id{ get; set; }

		/**商品ID */
		[XmlElement("pm_info_id")]
			public long?  Pm_info_id{ get; set; }

		/**活动渠道 1:PC及无线通用 3:无线专享 */
		[XmlElement("scope")]
			public long?  Scope{ get; set; }

		/**商家Id */
		[XmlElement("merchant_id")]
			public long?  Merchant_id{ get; set; }

		/**开始时间 */
		[XmlElement("start_time")]
			public string  Start_time{ get; set; }

		/**结束时间 */
		[XmlElement("end_time")]
			public string  End_time{ get; set; }

		/**pc链接 */
		[XmlElement("pc_url")]
			public string  Pc_url{ get; set; }

		/**无线链接 */
		[XmlElement("h5_url")]
			public string  H5_url{ get; set; }

		/**产品名称 */
		[XmlElement("product_name")]
			public string  Product_name{ get; set; }

		/**剁手价 */
		[XmlElement("price")]
			public double?  Price{ get; set; }

		/**已售百分百 */
		[XmlElement("sold_per")]
			public double?  Sold_per{ get; set; }

	}
}
