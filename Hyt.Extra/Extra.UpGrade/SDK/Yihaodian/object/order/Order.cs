using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	/// <summary>
	/// 订单
	/// </summary>
	[Serializable]
	public class Order 
	{
		/**订单ID */
		[XmlElement("orderId")]
			public long?  OrderId{ get; set; }

		/**订单编码 */
		[XmlElement("orderCode")]
			public string  OrderCode{ get; set; }

		/**订单状态: 
ORDER_WAIT_PAY：已下单（货款未全收）
ORDER_PAYED：已下单（货款已收）
ORDER_TRUNED_TO_DO：可以发货（已送仓库）
ORDER_OUT_OF_WH：已出库（货在途）
ORDER_RECEIVED：货物用户已收到
ORDER_FINISH：订单完成
ORDER_CANCEL：订单取消 */
		[XmlElement("orderStatus")]
			public string  OrderStatus{ get; set; }

		/**订购金额(=商品总金额-优惠金额+运费,为顾客应付额,抵用券也为支付手段) */
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

		/**更新时间 */
		[XmlElement("updateTime")]
			public string  UpdateTime{ get; set; }

		/**发票抬头 */
		[XmlElement("invoiceTitle")]
			public string  InvoiceTitle{ get; set; }

		/**发票内容 */
		[XmlElement("invoiceContent")]
			public string  InvoiceContent{ get; set; }

		/**用户ID */
		[XmlElement("endUserId")]
			public long?  EndUserId{ get; set; }

		/**仓库ID */
		[XmlElement("warehouseId")]
			public long?  WarehouseId{ get; set; }

		/**否为预售订单(1代表预售订单，0代表否) */
		[XmlElement("isDepositOrder")]
			public int?  IsDepositOrder{ get; set; }

		/**是否无线端订单(0:PC端订单;1:无线端订单) */
		[XmlElement("isMobileOrder")]
			public int?  IsMobileOrder{ get; set; }

		/**订单类型 */
		[XmlElement("businessType")]
			public int?  BusinessType{ get; set; }

		/**订单订金 */
		[XmlElement("orderDeposit")]
			public double?  OrderDeposit{ get; set; }

		/**订金支付时间 */
		[XmlElement("depositPaidTime")]
			public string  DepositPaidTime{ get; set; }

		/**是否为申请取消订单:1是, 0否 */
		[XmlElement("applyCancel")]
			public int?  ApplyCancel{ get; set; }

		/**门店ID */
		[XmlElement("storeId")]
			public long?  StoreId{ get; set; }

		/**门店名称 */
		[XmlElement("storeName")]
			public string  StoreName{ get; set; }

		/**沃尔玛外部门店ID */
		[XmlElement("externalStoreId")]
			public long?  ExternalStoreId{ get; set; }

		/**配送商id */
		[XmlElement("deliverySupplierid")]
			public long?  DeliverySupplierid{ get; set; }

		/**配送单号 */
		[XmlElement("merchantExpressNbr")]
			public string  MerchantExpressNbr{ get; set; }

		/**订购商品类型 */
		[XmlElement("orderProdType")]
			public long?  OrderProdType{ get; set; }

		/**do号 */
		[XmlElement("doCode")]
			public string  DoCode{ get; set; }

		/**身份证 */
		[XmlElement("cardNo")]
			public string  CardNo{ get; set; }

		/**手机号 */
		[XmlElement("mobile")]
			public string  Mobile{ get; set; }

		/**真实姓名 */
		[XmlElement("realName")]
			public string  RealName{ get; set; }

		/**分拣号 */
		[XmlElement("spiCode")]
			public string  SpiCode{ get; set; }

		/**配送商名称 */
		[XmlElement("deliverySheetCompanyName")]
			public string  DeliverySheetCompanyName{ get; set; }

		/**主卡权益码 */
		[XmlElement("masterCardNo")]
			public string  MasterCardNo{ get; set; }

		/**副卡权益码 */
		[XmlElement("childCardNo")]
			public string  ChildCardNo{ get; set; }

		/**海购订单报关流水号 */
		[XmlElement("paymentNo")]
			public string  PaymentNo{ get; set; }

		/**网关名称 */
		[XmlElement("gatewayName")]
			public string  GatewayName{ get; set; }

		/**海购订单支付流水号 */
		[XmlElement("paymentTransaction")]
			public string  PaymentTransaction{ get; set; }

		/**行邮税（进口税）金额 */
		[XmlElement("travelBaggageTaxAmount")]
			public double?  TravelBaggageTaxAmount{ get; set; }

		/**orderProcessStatus 成单标志（3：O2O待商家备货成单） */
		[XmlElement("orderProcessStatus")]
			public int?  OrderProcessStatus{ get; set; }

		/**支付状态：1.完全支付，2.未支付，3.部分支付，4.待审核 */
		[XmlElement("orderPaymentSignal")]
			public int?  OrderPaymentSignal{ get; set; }

		/**定金预售订单尾款开始支付时间 */
		[XmlElement("finalStartPayTime")]
			public string  FinalStartPayTime{ get; set; }

		/**定金预售订单尾款截止支付时间 */
		[XmlElement("finalEndPayTime")]
			public string  FinalEndPayTime{ get; set; }

		/**购买方税号 */
		[XmlElement("purchaserTaxCode")]
			public string  PurchaserTaxCode{ get; set; }

	}
}
