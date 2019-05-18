using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Trade
{
	/// <summary>
	/// 商城虚拟服务子订单
	/// </summary>
	[Serializable]
	public class ServiceOrder 
	{
		/**虚拟服务子订单订单号 */
		[XmlElement("oid")]
			public long?  Oid{ get; set; }

		/**服务所属的交易订单号。如果服务为一年包换，则item_oid这笔订单享受改服务的保护 */
		[XmlElement("item_oid")]
			public long?  Item_oid{ get; set; }

		/**服务数字id */
		[XmlElement("service_id")]
			public long?  Service_id{ get; set; }

		/**服务详情的URL地址 */
		[XmlElement("service_detail_url")]
			public string  Service_detail_url{ get; set; }

		/**购买数量，取值范围为大于0的整数 */
		[XmlElement("num")]
			public long?  Num{ get; set; }

		/**服务价格，精确到小数点后两位：单位:元 */
		[XmlElement("price")]
			public string  Price{ get; set; }

		/**子订单实付金额。精确到2位小数，单位:元。如:200.07，表示:200元7分。 */
		[XmlElement("payment")]
			public string  Payment{ get; set; }

		/**商品名称 */
		[XmlElement("title")]
			public string  Title{ get; set; }

		/**服务子订单总费用 */
		[XmlElement("total_fee")]
			public string  Total_fee{ get; set; }

		/**卖家昵称 */
		[XmlElement("buyer_nick")]
			public string  Buyer_nick{ get; set; }

		/**最近退款的id */
		[XmlElement("refund_id")]
			public long?  Refund_id{ get; set; }

		/**卖家昵称 */
		[XmlElement("seller_nick")]
			public string  Seller_nick{ get; set; }

		/**服务图片地址 */
		[XmlElement("pic_path")]
			public string  Pic_path{ get; set; }

	}
}
