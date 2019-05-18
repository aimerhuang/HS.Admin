using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	/// <summary>
	/// 用户订单信息
	/// </summary>
	[Serializable]
	public class UserOrderInfo 
	{
		/**订单ID */
		[XmlElement("orderId")]
			public long?  OrderId{ get; set; }

		/**订单编码 */
		[XmlElement("orderCode")]
			public string  OrderCode{ get; set; }

		/**订单状态: ORDER_WAIT_PAY：已下单（货款未全收） ORDER_PAYED：已下单（货款已收） ORDER_TRUNED_TO_DO：可以发货（已送仓库） ORDER_CAN_OUT_OF_WH：已出库（货在途）  ORDER_RECEIVED：货物用户已收到 ORDER_FINISH：订单完成 ORDER_CANCEL：订单取消 */
		[XmlElement("orderStatus")]
			public string  OrderStatus{ get; set; }

		/**订购金额=商品金额+运费-优惠，即为顾客应付款（抵用券属于支付手段） */
		[XmlElement("orderAmount")]
			public double?  OrderAmount{ get; set; }

		/**产品总额 */
		[XmlElement("productAmount")]
			public double?  ProductAmount{ get; set; }

		/**订单创建日期 */
		[XmlElement("orderCreateTime")]
			public string  OrderCreateTime{ get; set; }

		/**运费 */
		[XmlElement("orderDeliveryFee")]
			public double?  OrderDeliveryFee{ get; set; }

		/**发票需要情况:0不需要，1旧版普通，2新版普通，3增值税发票 */
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

		/**收货人邮编 */
		[XmlElement("goodReceiverPostCode")]
			public string  GoodReceiverPostCode{ get; set; }

		/**收货人电话 */
		[XmlElement("goodReceiverPhone")]
			public string  GoodReceiverPhone{ get; set; }

		/**收货人手机号 */
		[XmlElement("goodReceiverMoblie")]
			public string  GoodReceiverMoblie{ get; set; }

		/**发货时间 */
		[XmlElement("deliveryDate")]
			public string  DeliveryDate{ get; set; }

		/**确认收货时间 */
		[XmlElement("receiveDate")]
			public string  ReceiveDate{ get; set; }

		/**买家留言 */
		[XmlElement("deliveryRemark")]
			public string  DeliveryRemark{ get; set; }

		/**配送商ID */
		[XmlElement("deliverySupplierId")]
			public long?  DeliverySupplierId{ get; set; }

		/**付款确认时间(实际付款时间) */
		[XmlElement("orderPaymentConfirmDate")]
			public string  OrderPaymentConfirmDate{ get; set; }

		/**订单支付方式0:账户支付1:网上支付2:货到付款3:邮局汇款4:银行转账5:pos机6:万里通7:分期付款8:合同账期9:货到转账10:货到付支票 */
		[XmlElement("payServiceType")]
			public int?  PayServiceType{ get; set; }

		/**参加促销活动立减金额 */
		[XmlElement("orderPromotionDiscount")]
			public double?  OrderPromotionDiscount{ get; set; }

		/**配送商送货编号(运单号) */
		[XmlElement("merchantExpressNbr")]
			public string  MerchantExpressNbr{ get; set; }

		/**更新时间 */
		[XmlElement("updateTime")]
			public string  UpdateTime{ get; set; }

		/**用户ID */
		[XmlElement("endUserId")]
			public long?  EndUserId{ get; set; }

	}
}
