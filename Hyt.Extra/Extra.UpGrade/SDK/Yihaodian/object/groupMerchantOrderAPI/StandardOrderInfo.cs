using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.GroupMerchantOrderAPI
{
	/// <summary>
	/// 标准订单信息
	/// </summary>
	[Serializable]
	public class StandardOrderInfo 
	{
		/**订单基本信息 */
		[XmlElement("orderHead")]
		public StandardOrderHead  OrderHead{ get; set; }

		/**订单商品明细信息 */
		[XmlElement("orderItemList")]
		public StandardOrderItemList  OrderItemList{ get; set; }

		/**订单参加促销的明细 */
		[XmlElement("promotionDetailList")]
		public StandardOrderPromotionDetailList  PromotionDetailList{ get; set; }

	}
}
