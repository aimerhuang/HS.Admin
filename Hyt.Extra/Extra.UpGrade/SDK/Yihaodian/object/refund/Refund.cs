using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Refund
{
	/// <summary>
	/// 退货列表信息
	/// </summary>
	[Serializable]
	public class Refund 
	{
		/**退货单号 */
		[XmlElement("refundCode")]
			public string  RefundCode{ get; set; }

		/**订单ID */
		[XmlElement("orderId")]
			public long?  OrderId{ get; set; }

		/**订单code */
		[XmlElement("orderCode")]
			public string  OrderCode{ get; set; }

		/**退货状态(0:待审核;3:客服仲裁;4:已拒绝;11:退货中-待顾客寄回;12:退货中-待确认退款;13:换货中;27:退款完成;33:换货完成;34:已撤销;40:已关闭) */
		[XmlElement("refundStatus")]
			public int?  RefundStatus{ get; set; }

		/**退款金额 */
		[XmlElement("refundAmount")]
			public double?  RefundAmount{ get; set; }

		/** 	退货申请时间 */
		[XmlElement("applyDate")]
			public string  ApplyDate{ get; set; }

		/** 	退款单简要详情 */
		[XmlElement("refundParticularList")]
		public RefundParticularList  RefundParticularList{ get; set; }

		/**退货更新时间 */
		[XmlElement("updateTime")]
			public string  UpdateTime{ get; set; }

		/**0:实物寄回;1:无需实物寄回 */
		[XmlElement("isMissingProduct")]
			public int?  IsMissingProduct{ get; set; }

		/**退货更新时间（Date类型） */
		[XmlElement("updateDate")]
			public string  UpdateDate{ get; set; }

	}
}
