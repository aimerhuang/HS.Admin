using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.GroupMerchantOrderAPI
{
	/// <summary>
	/// 订单商品促销详细信息
	/// </summary>
	[Serializable]
	public class StandardOrderPromotionDetail 
	{
		/**578券对应HOST文件中的ItemNbr */
		[XmlElement("itemNbr")]
			public string  ItemNbr{ get; set; }

		/**券类型（1：578券，2：GP） */
		[XmlElement("couponType")]
			public int?  CouponType{ get; set; }

		/**促销金额=抵用券/折扣/满减 */
		[XmlElement("discountAmount")]
			public double?  DiscountAmount{ get; set; }

		/**订单ID */
		[XmlElement("orderId")]
			public long?  OrderId{ get; set; }

		/**SoItem表的itemID */
		[XmlElement("orderItemId")]
			public long?  OrderItemId{ get; set; }

		/**578券对应的UPC */
		[XmlElement("upcCode")]
			public string  UpcCode{ get; set; }

	}
}
