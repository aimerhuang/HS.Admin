using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	/// <summary>
	/// 订单详细信息
	/// </summary>
	[Serializable]
	public class OrderDetail 
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

		/**订购金额=商品金额+运费-满立减金额，即为顾客应付款 */
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

		/**卖家备注 */
		[XmlElement("merchantRemark")]
			public string  MerchantRemark{ get; set; }

		/**付款确认时间(实际付款时间) */
		[XmlElement("orderPaymentConfirmDate")]
			public string  OrderPaymentConfirmDate{ get; set; }

		/**订单支付方式0:账户支付1:网上支付2:货到付款3:邮局汇款4:银行转账5:pos机6:万里通7:分期付款8:合同账期9:货到转账10:货到付支票 12:货到刷支付宝 */
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

		/**商家抵用券支付金额 */
		[XmlElement("orderCouponDiscount")]
			public double?  OrderCouponDiscount{ get; set; }

		/**1mall平台抵用券支付金额 */
		[XmlElement("orderPlatformDiscount")]
			public double?  OrderPlatformDiscount{ get; set; }

		/** 发票抬头 */
		[XmlElement("invoiceTitle")]
			public string  InvoiceTitle{ get; set; }

		/**发票内容 */
		[XmlElement("invoiceContent")]
			public string  InvoiceContent{ get; set; }

		/**用户ID */
		[XmlElement("endUserId")]
			public long?  EndUserId{ get; set; }

		/**实收款==产品金额-促销活动立减金额 -商家抵用券金额+运费 */
		[XmlElement("realAmount")]
			public double?  RealAmount{ get; set; }

		/**仓库ID */
		[XmlElement("warehouseId")]
			public long?  WarehouseId{ get; set; }

		/**	 是否无线端订单(0:PC端订单;1:无线端订单) */
		[XmlElement("isMobileOrder")]
			public int?  IsMobileOrder{ get; set; }

		/**订单类型：0:前台普通订单1:团购订单              2:EPP订单              3:处方药订单              4:B2B订单              5:店中店代售              6:平安3g标志              10:定期购订单             11:预售订单              12:闪购              13:合约机              14:手机充值              15:选号入网              16:二手品             17:1闪团订单             18:生活团订单             19:定金预售             20:社区团订单             21:微店订单             22:OMO订单             23:跨境通海购订单             24:快拍订单             25:手机流量充值订单             26:支付宝渠道订单             27:门店订单   */
		[XmlElement("businessType")]
			public int?  BusinessType{ get; set; }

		/**订单订金 */
		[XmlElement("orderDeposit")]
			public double?  OrderDeposit{ get; set; }

		/**是否为预售订单 */
		[XmlElement("isDepositOrder")]
			public int?  IsDepositOrder{ get; set; }

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

		/**货到应收金额(订单支付方式为2、5、12时候有值) */
		[XmlElement("collectOnDeliveryAmount")]
			public double?  CollectOnDeliveryAmount{ get; set; }

		/**订单商品类型 */
		[XmlElement("orderProdType")]
			public long?  OrderProdType{ get; set; }

		/**海购订单报关流水号 */
		[XmlElement("paymentNo")]
			public string  PaymentNo{ get; set; }

		/**网关名称 */
		[XmlElement("gatewayName")]
			public string  GatewayName{ get; set; }

		/**海购订单支付流水号 */
		[XmlElement("paymentTransaction")]
			public string  PaymentTransaction{ get; set; }

		/** 成单标志（3：O2O待商家备货成单） */
		[XmlElement("orderProcessStatus")]
			public int?  OrderProcessStatus{ get; set; }

		/**定金抵用金额 */
		[XmlElement("depositDiscount")]
			public double?  DepositDiscount{ get; set; }

		/**购买方税号 */
		[XmlElement("purchaserTaxCode")]
			public string  PurchaserTaxCode{ get; set; }

	}
}
