using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	/// <summary>
	/// 发票
	/// </summary>
	[Serializable]
	public class InvoiceInfo 
	{
		/**订单编码 */
		[XmlElement("orderCode")]
			public string  OrderCode{ get; set; }

		/**发票内容 */
		[XmlElement("invoiceContents")]
		public StringList  InvoiceContents{ get; set; }

		/**发票金额 */
		[XmlElement("detailAmounts")]
		public StringList  DetailAmounts{ get; set; }

		/**发票抬头（多个抬头用/来分隔） */
		[XmlElement("invoiceTitle")]
			public string  InvoiceTitle{ get; set; }

		/**是否补开发票：0.否 1.是（多个记录用 / 来分隔） */
		[XmlElement("isSupplement")]
			public string  IsSupplement{ get; set; }

		/**是否已寄出：0.否 1.是（多个记录用 / 来分隔） */
		[XmlElement("isSend")]
			public string  IsSend{ get; set; }

		/** 	申请时间 */
		[XmlElement("createTime")]
			public string  CreateTime{ get; set; }

		/**购买方税号（多个税号用 / 来分隔） */
		[XmlElement("purchaserTaxCode")]
			public string  PurchaserTaxCode{ get; set; }

	}
}
