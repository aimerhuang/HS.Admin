using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierOrder
{
	/// <summary>
	/// 订单详细信息
	/// </summary>
	[Serializable]
	public class OrderDetailInfo 
	{
		/**订单Id */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**订购金额=商品金额+运费-优惠，即为顾客应付款（抵用券属于支付手段） */
		[XmlElement("orderAmount")]
			public double?  OrderAmount{ get; set; }

		/**订单编号 */
		[XmlElement("orderCode")]
			public string  OrderCode{ get; set; }

		/**是否是直发订单（0：不是，1：是） */
		[XmlElement("isB2CDirect")]
			public int?  IsB2CDirect{ get; set; }

		/**用户ID */
		[XmlElement("endUserId")]
			public long?  EndUserId{ get; set; }

		/**运费 */
		[XmlElement("orderDeliveryFee")]
			public double?  OrderDeliveryFee{ get; set; }

		/**订单创建日期 */
		[XmlElement("orderCreateTime")]
			public string  OrderCreateTime{ get; set; }

		/**投递人 */
		[XmlElement("orderDeliveryPerson")]
			public string  OrderDeliveryPerson{ get; set; }

		/**发票需要情况（0:不需要，1:旧版普通，2:新版普通，3:增值税发票） */
		[XmlElement("orderNeedInvoice")]
			public int?  OrderNeedInvoice{ get; set; }

		/**收货人姓名 */
		[XmlElement("goodReceiverName")]
			public string  GoodReceiverName{ get; set; }

		/**收货人地址 */
		[XmlElement("goodReceiverAddress")]
			public string  GoodReceiverAddress{ get; set; }

		/**收货人省份 */
		[XmlElement("goodReceiverProvince")]
			public string  GoodReceiverProvince{ get; set; }

		/**收货人城市 */
		[XmlElement("goodReceiverCity")]
			public string  GoodReceiverCity{ get; set; }

		/**收货人地区 */
		[XmlElement("goodReceiverCounty")]
			public string  GoodReceiverCounty{ get; set; }

		/**收货人电话 */
		[XmlElement("goodReceiverPhone")]
			public string  GoodReceiverPhone{ get; set; }

		/**收货人手机号 */
		[XmlElement("goodReceiverMobile")]
			public string  GoodReceiverMobile{ get; set; }

		/**配送员手机号 */
		[XmlElement("orderDeliveryPersonMobile")]
			public string  OrderDeliveryPersonMobile{ get; set; }

		/**发货备注 */
		[XmlElement("deliveryRemark")]
			public string  DeliveryRemark{ get; set; }

		/**订单类型（0:前台普通订单，1:团购订单，2:EPP订单，3:处方药订单，4:B2B订单，5:店中店代售，6:平安3g标志，10:定期购订单） */
		[XmlElement("businessType")]
			public int?  BusinessType{ get; set; }

		/**配送方式类型 {10001:普通快递、20001:EMS、30001:供应商直送、40001:自提、30002:店中店商家直送}  */
		[XmlElement("deliveryMethodType")]
			public int?  DeliveryMethodType{ get; set; }

		/**订单状态（1：待发货，2：已发货，3：用户已收到货，4:已完成，5:订单已关闭，6：退换货，7：未支付） */
		[XmlElement("orderStatus")]
			public int?  OrderStatus{ get; set; }

		/**订单更新时间 */
		[XmlElement("orderUpdateTime")]
			public string  OrderUpdateTime{ get; set; }

		/**发票抬头 */
		[XmlElement("invoiceTitle")]
			public string  InvoiceTitle{ get; set; }

		/**发票内容 */
		[XmlElement("invoiceContent")]
			public string  InvoiceContent{ get; set; }

		/**实收款=产品金额-促销活动立减金额-商家抵用卷金额+运费 */
		[XmlElement("realAmount")]
			public double?  RealAmount{ get; set; }

		/**产品总额=定购金额-运费 */
		[XmlElement("productAmount")]
			public double?  ProductAmount{ get; set; }

		/**收货人邮编 */
		[XmlElement("goodReceiverPostCode")]
			public string  GoodReceiverPostCode{ get; set; }

		/**购买人税号 */
		[XmlElement("purchaserTaxCode")]
			public string  PurchaserTaxCode{ get; set; }

	}
}
