using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.GroupMerchantOrderAPI
{
	/// <summary>
	/// 订单基本信息
	/// </summary>
	[Serializable]
	public class StandardOrderHead 
	{
		/**订单ID（同orderCode） */
		[XmlElement("orderId")]
			public long?  OrderId{ get; set; }

		/**订单编码（同orderId） */
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

		/**发票需要情况:0不需要，1旧版普通，2新版普通，3增值税发票 */
		[XmlElement("orderNeedInvoice")]
			public int?  OrderNeedInvoice{ get; set; }

		/**发票抬头 */
		[XmlElement("invoiceTitle")]
			public string  InvoiceTitle{ get; set; }

		/**配送商id */
		[XmlElement("deliverySupplierid")]
			public long?  DeliverySupplierid{ get; set; }

		/**配送商名称 */
		[XmlElement("deliverySheetCompanyName")]
			public string  DeliverySheetCompanyName{ get; set; }

		/**收货人姓名 */
		[XmlElement("goodReceiverName")]
			public string  GoodReceiverName{ get; set; }

		/**收货人地址 */
		[XmlElement("goodReceiverAddress")]
			public string  GoodReceiverAddress{ get; set; }

		/**收货人省份ID */
		[XmlElement("goodReceiverProvinceId")]
			public long?  GoodReceiverProvinceId{ get; set; }

		/**收货人省份 */
		[XmlElement("goodReceiverProvince")]
			public string  GoodReceiverProvince{ get; set; }

		/**收货人城市ID */
		[XmlElement("goodReceiverCityId")]
			public long?  GoodReceiverCityId{ get; set; }

		/**收货人城市 */
		[XmlElement("goodReceiverCity")]
			public string  GoodReceiverCity{ get; set; }

		/**收货人地区ID */
		[XmlElement("goodReceiverCountyId")]
			public long?  GoodReceiverCountyId{ get; set; }

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

		/**买家留言 */
		[XmlElement("deliveryRemark")]
			public string  DeliveryRemark{ get; set; }

		/**卖家备注 */
		[XmlElement("merchantRemark")]
			public string  MerchantRemark{ get; set; }

		/**期望收货日期（格式：yyyy-MM-dd） */
		[XmlElement("expectReceiveDate")]
			public string  ExpectReceiveDate{ get; set; }

		/**期望收货时间段（格式：09:00-12:00） */
		[XmlElement("expectReceiveTimes")]
			public string  ExpectReceiveTimes{ get; set; }

		/**订单支付方式0:账户支付1:网上支付2:货到付款3:邮局汇款4:银行转账5:pos机6:万里通7:分期付款8:合同账期9:货到转账10:货到付支票 12:货到刷支付宝 */
		[XmlElement("payServiceType")]
			public int?  PayServiceType{ get; set; }

		/**订单产品类型
第一位数字 虚拟商品 1 实体商品普通 2
前四位数字（卡 10001，票券 10002，手机充值 10003）
100010001  SAM会员卡 
100010002  SAM会员卡续费 */
		[XmlElement("orderProdType")]
			public long?  OrderProdType{ get; set; }

		/**线下门店ID */
		[XmlElement("outStoreId")]
			public long?  OutStoreId{ get; set; }

		/**Sam会员卡卡号 */
		[XmlElement("samMemberCard")]
			public string  SamMemberCard{ get; set; }

		/**配送方式ID */
		[XmlElement("deliveryMethodId")]
			public long?  DeliveryMethodId{ get; set; }

		/**配送方式名称 */
		[XmlElement("deliveryMethodName")]
			public string  DeliveryMethodName{ get; set; }

		/**地图区块名称(如：张江A区) */
		[XmlElement("mapBlockName")]
			public string  MapBlockName{ get; set; }

		/**do号 */
		[XmlElement("doCode")]
			public string  DoCode{ get; set; }

		/**分拣号 */
		[XmlElement("spiCode")]
			public string  SpiCode{ get; set; }

		/**分拣名称 */
		[XmlElement("spiName")]
			public string  SpiName{ get; set; }

		/**配送类型 */
		[XmlElement("deliveryType")]
			public int?  DeliveryType{ get; set; }

		/**运费 */
		[XmlElement("orderDeliveryFee")]
			public double?  OrderDeliveryFee{ get; set; }

		/**商品总金额 =（原价*数量） */
		[XmlElement("productOriginalTotalAmount")]
			public double?  ProductOriginalTotalAmount{ get; set; }

		/**订单折扣金额=满折+满减+SAM抵用券 */
		[XmlElement("orderDiscountAmount")]
			public double?  OrderDiscountAmount{ get; set; }

		/**平台抵用券金额 */
		[XmlElement("platformCouponDiscountAmount")]
			public double?  PlatformCouponDiscountAmount{ get; set; }

		/**应收金额=商品总金额+运费-订单折扣金额+平台抵用券金额 */
		[XmlElement("receiveTotalAmount")]
			public double?  ReceiveTotalAmount{ get; set; }

		/**购买方税号 */
		[XmlElement("purchaserTaxCode")]
			public string  PurchaserTaxCode{ get; set; }

	}
}
