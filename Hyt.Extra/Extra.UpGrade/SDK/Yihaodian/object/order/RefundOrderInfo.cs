using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	/// <summary>
	/// 异常订单退款相关信息
	/// </summary>
	[Serializable]
	public class RefundOrderInfo 
	{
		/**订单号 */
		[XmlElement("orderCode")]
			public string  OrderCode{ get; set; }

		/**退货单号 */
		[XmlElement("refundCode")]
			public string  RefundCode{ get; set; }

		/**退款单号 */
		[XmlElement("refundOrderCode")]
			public string  RefundOrderCode{ get; set; }

		/**退款金额 */
		[XmlElement("refundAmount")]
			public string  RefundAmount{ get; set; }

		/**申请时间 */
		[XmlElement("applyDate")]
			public string  ApplyDate{ get; set; }

		/**批准时间 */
		[XmlElement("approvalDate")]
			public string  ApprovalDate{ get; set; }

		/**收货人电话 */
		[XmlElement("receiverPhone")]
			public string  ReceiverPhone{ get; set; }

		/**收货人手机 */
		[XmlElement("receiverMobile")]
			public string  ReceiverMobile{ get; set; }

		/**备注 */
		[XmlElement("remarkes")]
			public string  Remarkes{ get; set; }

		/**退款单状态  ： 已取消、已拒绝、待审核 、已退款 */
		[XmlElement("refundStatus")]
			public string  RefundStatus{ get; set; }

		/**操作人  ： 商家、客服、系统超时 */
		[XmlElement("operatorType")]
			public string  OperatorType{ get; set; }

		/**退款原因  ：运费补偿、商品破损补偿、商品质量有问题、发货延迟、其他返利、监督赔付 、商品差价补偿、节能补贴赔付 */
		[XmlElement("reasonMsg")]
			public string  ReasonMsg{ get; set; }

	}
}
