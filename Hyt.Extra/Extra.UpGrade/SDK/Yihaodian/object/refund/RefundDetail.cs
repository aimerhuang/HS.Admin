using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Refund
{
	/// <summary>
	/// 退货详细信息
	/// </summary>
	[Serializable]
	public class RefundDetail 
	{
		/**退货单号(退货编号) */
		[XmlElement("refundCode")]
			public string  RefundCode{ get; set; }

		/**订单ID */
		[XmlElement("orderId")]
			public long?  OrderId{ get; set; }

		/**订单号 */
		[XmlElement("orderCode")]
			public string  OrderCode{ get; set; }

		/**退货状态(0:待审核;3:客服仲裁;4:已拒绝;11:退货中-待顾客寄回;12:退货中-待确认退款;13:换货中;27:退款完成;33:换货完成;34:已取消;40:已关闭) */
		[XmlElement("refundStatus")]
			public int?  RefundStatus{ get; set; }

		/**退款运费 */
		[XmlElement("deliveryFee")]
			public double?  DeliveryFee{ get; set; }

		/**产品退款金额(不包括运费) */
		[XmlElement("productAmount")]
			public double?  ProductAmount{ get; set; }

		/**商家联系人(回寄联系人) */
		[XmlElement("contactName")]
			public string  ContactName{ get; set; }

		/**商家联系电话 */
		[XmlElement("contactPhone")]
			public string  ContactPhone{ get; set; }

		/**商家联系地址 */
		[XmlElement("sendBackAddress")]
			public string  SendBackAddress{ get; set; }

		/**退货原因 */
		[XmlElement("reasonMsg")]
			public string  ReasonMsg{ get; set; }

		/**退货问题描述 */
		[XmlElement("refundProblem")]
			public string  RefundProblem{ get; set; }

		/**图片url列表(逗号分隔) */
		[XmlElement("evidencePicUrls")]
			public string  EvidencePicUrls{ get; set; }

		/**顾客姓名 */
		[XmlElement("receiverName")]
			public string  ReceiverName{ get; set; }

		/**顾客联系电话 */
		[XmlElement("receiverPhone")]
			public string  ReceiverPhone{ get; set; }

		/**顾客地址 */
		[XmlElement("receiverAddress")]
			public string  ReceiverAddress{ get; set; }

		/**申请时间 */
		[XmlElement("applyDate")]
			public string  ApplyDate{ get; set; }

		/**备忘类型(0:red,1:dark_yellow,2:yellow,3:green,4:cyan,5:blue,6: purl) */
		[XmlElement("merchantMark")]
			public string  MerchantMark{ get; set; }

		/**商家备注 */
		[XmlElement("merchantRemark")]
			public string  MerchantRemark{ get; set; }

		/**审核时间 */
		[XmlElement("approveDate")]
			public string  ApproveDate{ get; set; }

		/**顾客寄回时间 */
		[XmlElement("sendBackDate")]
			public string  SendBackDate{ get; set; }

		/**拒绝时间 */
		[XmlElement("rejectDate")]
			public string  RejectDate{ get; set; }

		/**取消时间 */
		[XmlElement("cancelTime")]
			public string  CancelTime{ get; set; }

		/**顾客回寄的快递公司 */
		[XmlElement("expressName")]
			public string  ExpressName{ get; set; }

		/**顾客回寄的快递单号 */
		[XmlElement("expressNbr")]
			public string  ExpressNbr{ get; set; }

		/**0：退货、1：换货 */
		[XmlElement("operateType")]
			public int?  OperateType{ get; set; }

		/**退货id */
		[XmlElement("refundId")]
			public long?  RefundId{ get; set; }

		/**0:实物寄回;1:无需实物寄回 */
		[XmlElement("isMissingProduct")]
			public int?  IsMissingProduct{ get; set; }

	}
}
