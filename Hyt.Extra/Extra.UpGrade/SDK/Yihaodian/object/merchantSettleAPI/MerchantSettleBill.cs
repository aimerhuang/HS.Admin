using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.MerchantSettleAPI
{
	/// <summary>
	/// 商家结算明细
	/// </summary>
	[Serializable]
	public class MerchantSettleBill 
	{
		/**可结算时间 */
		[XmlElement("createDate")]
			public string  CreateDate{ get; set; }

		/**付款时间 */
		[XmlElement("orderPaymentConfirm")]
			public string  OrderPaymentConfirm{ get; set; }

		/**商家Id */
		[XmlElement("merchantId")]
			public long?  MerchantId{ get; set; }

		/**商家名称 */
		[XmlElement("merchantName")]
			public string  MerchantName{ get; set; }

		/**门店编号 */
		[XmlElement("merchantStoreOrder")]
			public long?  MerchantStoreOrder{ get; set; }

		/**门店名称 */
		[XmlElement("merchantStoreName")]
			public string  MerchantStoreName{ get; set; }

		/**订单编号 */
		[XmlElement("orderCode")]
			public string  OrderCode{ get; set; }

		/**商品编号 */
		[XmlElement("productCode")]
			public string  ProductCode{ get; set; }

		/**商品名称 */
		[XmlElement("productCname")]
			public string  ProductCname{ get; set; }

		/**结算金额 */
		[XmlElement("unpayMoney")]
			public double?  UnpayMoney{ get; set; }

		/**账单编号 */
		[XmlElement("collectId")]
			public long?  CollectId{ get; set; }

		/**商品款 */
		[XmlElement("saleFee")]
			public double?  SaleFee{ get; set; }

		/**退货额 */
		[XmlElement("retnFee")]
			public double?  RetnFee{ get; set; }

		/**商家佣金 */
		[XmlElement("comsn")]
			public double?  Comsn{ get; set; }

		/**运费 */
		[XmlElement("joinFee")]
			public double?  JoinFee{ get; set; }

		/**CPS佣金 */
		[XmlElement("cpsComsn")]
			public double?  CpsComsn{ get; set; }

		/**其他费用金额（包括：
行邮税,,
商城COD商品款,COD佣金,服务费,
LBY费用,
4PL费用, 
1订贷服务费,
技术服务费 等
） */
		[XmlElement("otherAmount")]
			public double?  OtherAmount{ get; set; }

	}
}
