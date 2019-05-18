using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	/// <summary>
	/// 取消订单信息
	/// </summary>
	[Serializable]
	public class OrderCancelInfo 
	{
		/**订单编码 */
		[XmlElement("orderCode")]
			public string  OrderCode{ get; set; }

		/**订单取消申请时间 */
		[XmlElement("orderCancelApplyTime")]
			public string  OrderCancelApplyTime{ get; set; }

		/**处理时间 */
		[XmlElement("processTime")]
			public string  ProcessTime{ get; set; }

		/**取消流程状态 1:申请取消（待商家处理），2:强制取消、3:商家同意取消、4:超时取消、5:商家发货处理 */
		[XmlElement("cancelStatus")]
			public int?  CancelStatus{ get; set; }

		/**记录来源 1:客服、2:顾客 */
		[XmlElement("recordType")]
			public int?  RecordType{ get; set; }

		/**订单取消原因状态码 */
		[XmlElement("orderCancelReason")]
			public int?  OrderCancelReason{ get; set; }

		/**取消原因说明 */
		[XmlElement("orderCancelReasonStr")]
			public string  OrderCancelReasonStr{ get; set; }

		/**商家拒绝取消原因 */
		[XmlElement("cancelRefuseReason")]
			public string  CancelRefuseReason{ get; set; }

	}
}
