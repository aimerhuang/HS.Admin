using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.GroupMerchantOrderAPI
{
	/// <summary>
	/// 订单商品明细信息
	/// </summary>
	[Serializable]
	public class StandardOrderItem 
	{
		/**订单明细ID */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**订单ID */
		[XmlElement("orderId")]
			public long?  OrderId{ get; set; }

		/**产品id */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**产品名称 */
		[XmlElement("productCName")]
			public string  ProductCName{ get; set; }

		/**数量 */
		[XmlElement("orderItemNum")]
			public int?  OrderItemNum{ get; set; }

		/**Sam使用, 对应HOST文件中的ItemNbr */
		[XmlElement("itemNbr")]
			public string  ItemNbr{ get; set; }

		/**商品原价 */
		[XmlElement("productOriginalPrice")]
			public double?  ProductOriginalPrice{ get; set; }

		/**商品总金额=商品原价*数量 */
		[XmlElement("productOriginalAmount")]
			public double?  ProductOriginalAmount{ get; set; }

		/**促销的分摊金额=商品折扣金额 */
		[XmlElement("orderItemDiscountAmount")]
			public double?  OrderItemDiscountAmount{ get; set; }

		/**平台抵用券分摊金额=平台抵用券金额 */
		[XmlElement("platformCouponShareAmount")]
			public double?  PlatformCouponShareAmount{ get; set; }

		/**SAM应收金额=商品总金额-商品折扣金额+平台抵用券金额 */
		[XmlElement("receiveAmount")]
			public double?  ReceiveAmount{ get; set; }

		/**Sam应收单价（过机用）=SAM应收金额/数量 */
		[XmlElement("receivableUnitPrice")]
			public double?  ReceivableUnitPrice{ get; set; }

		/**UPC（PSS处获取） */
		[XmlElement("upcCode")]
			public string  UpcCode{ get; set; }

	}
}
