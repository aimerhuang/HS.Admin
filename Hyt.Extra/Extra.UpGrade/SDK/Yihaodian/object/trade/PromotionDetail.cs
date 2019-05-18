using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Trade
{
	/// <summary>
	/// 交易的优惠信息详情 
	/// </summary>
	[Serializable]
	public class PromotionDetail 
	{
		/**交易的主订单或子订单号 */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**优惠信息的名称 */
		[XmlElement("promotion_name")]
			public string  Promotion_name{ get; set; }

		/**优惠金额（免运费、限时打折时为空）,单位：元 */
		[XmlElement("discount_fee")]
			public string  Discount_fee{ get; set; }

		/**满就送商品时，所送商品的名称 */
		[XmlElement("gift_item_name")]
			public string  Gift_item_name{ get; set; }

		/**赠品的宝贝id */
		[XmlElement("gift_item_id")]
			public long?  Gift_item_id{ get; set; }

		/**满就送礼物的礼物数量 */
		[XmlElement("gift_item_num")]
			public string  Gift_item_num{ get; set; }

		/**优惠活动的描述 */
		[XmlElement("promotion_desc")]
			public string  Promotion_desc{ get; set; }

		/**优惠id，(由营销工具id、优惠活动id和优惠详情id组成，结构为：营销工具id-优惠活动id_优惠详情id，如mjs-123024_211143） */
		[XmlElement("promotion_id")]
			public string  Promotion_id{ get; set; }

	}
}
