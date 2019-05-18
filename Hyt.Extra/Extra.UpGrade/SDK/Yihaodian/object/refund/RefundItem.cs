using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Refund
{
	/// <summary>
	/// 退货单明细
	/// </summary>
	[Serializable]
	public class RefundItem 
	{
		/**1号店产品ID */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**产品名称 */
		[XmlElement("productCname")]
			public string  ProductCname{ get; set; }

		/**顾客购买数量  */
		[XmlElement("orderItemNum")]
			public long?  OrderItemNum{ get; set; }

		/**退货单价 */
		[XmlElement("orderItemPrice")]
			public double?  OrderItemPrice{ get; set; }

		/**退货数量 */
		[XmlElement("productRefundNum")]
			public long?  ProductRefundNum{ get; set; }

		/**用户申请退货数量 */
		[XmlElement("originalRefundNum")]
			public long?  OriginalRefundNum{ get; set; }

		/**订单明细ID */
		[XmlElement("orderItemId")]
			public long?  OrderItemId{ get; set; }

	}
}
