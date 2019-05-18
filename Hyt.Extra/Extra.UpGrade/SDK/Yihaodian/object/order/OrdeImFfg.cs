using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	/// <summary>
	/// 订单明细
	/// </summary>
	[Serializable]
	public class OrdeImFfg 
	{
		/**订单明细ID */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**订单ID */
		[XmlElement("orderId")]
			public long?  OrderId{ get; set; }

		/**产品名称 */
		[XmlElement("productCName")]
			public string  ProductCName{ get; set; }

		/**金额 */
		[XmlElement("orderItemAmount")]
			public double?  OrderItemAmount{ get; set; }

		/**数量 */
		[XmlElement("orderItemNum")]
			public int?  OrderItemNum{ get; set; }

		/**单价 */
		[XmlElement("orderItemPrice")]
			public double?  OrderItemPrice{ get; set; }

		/**产品原价 */
		[XmlElement("originalPrice")]
			public double?  OriginalPrice{ get; set; }

		/**产品id */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**团购产品标识，1表示团购产品,0表示非团购产品 */
		[XmlElement("groupFlag")]
			public int?  GroupFlag{ get; set; }

		/**商家id */
		[XmlElement("merchantId")]
			public long?  MerchantId{ get; set; }

		/**退换货完成时间 */
		[XmlElement("processFinishDate")]
			public string  ProcessFinishDate{ get; set; }

		/**更新时间 */
		[XmlElement("updateTime")]
			public string  UpdateTime{ get; set; }

		/**产品外部编码 */
		[XmlElement("outerId")]
			public string  OuterId{ get; set; }

		/**商品运费分摊金额 */
		[XmlElement("deliveryFeeAmount")]
			public double?  DeliveryFeeAmount{ get; set; }

		/**促销活动立减分摊金额 */
		[XmlElement("promotionAmount")]
			public double?  PromotionAmount{ get; set; }

		/**商家抵用券分摊金额 */
		[XmlElement("couponAmountMerchant")]
			public double?  CouponAmountMerchant{ get; set; }

		/**1mall平台抵用券分摊金额 */
		[XmlElement("couponPlatformDiscount")]
			public double?  CouponPlatformDiscount{ get; set; }

		/**节能补贴金额 */
		[XmlElement("subsidyAmount")]
			public double?  SubsidyAmount{ get; set; }

		/**单品订金金额 */
		[XmlElement("productDeposit")]
			public double?  ProductDeposit{ get; set; }

	}
}
