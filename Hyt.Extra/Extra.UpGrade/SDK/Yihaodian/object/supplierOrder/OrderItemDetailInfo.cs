using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierOrder
{
	/// <summary>
	/// 订单项详情
	/// </summary>
	[Serializable]
	public class OrderItemDetailInfo 
	{
		/**订单详情Id */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**订单Id */
		[XmlElement("orderId")]
			public long?  OrderId{ get; set; }

		/**促销价格 */
		[XmlElement("promotionAmount")]
			public double?  PromotionAmount{ get; set; }

		/**产品Id */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**单价 */
		[XmlElement("orderItemPrice")]
			public double?  OrderItemPrice{ get; set; }

		/**数量 */
		[XmlElement("orderItemNum")]
			public int?  OrderItemNum{ get; set; }

		/**产品名称 */
		[XmlElement("productCname")]
			public string  ProductCname{ get; set; }

		/**仓库ID */
		[XmlElement("warehouseId")]
			public long?  WarehouseId{ get; set; }

		/**运费 */
		[XmlElement("deliveryFeeAmount")]
			public double?  DeliveryFeeAmount{ get; set; }

		/**外部产品编码 */
		[XmlElement("outerId")]
			public string  OuterId{ get; set; }

		/**进价 */
		[XmlElement("inPrice")]
			public double?  InPrice{ get; set; }

	}
}
