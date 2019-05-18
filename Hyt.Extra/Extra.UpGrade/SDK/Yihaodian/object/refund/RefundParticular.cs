using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Refund
{
	/// <summary>
	/// 退货列表详情
	/// </summary>
	[Serializable]
	public class RefundParticular 
	{
		/** 	产品ID */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**退货数量 */
		[XmlElement("productRefundNum")]
			public long?  ProductRefundNum{ get; set; }

		/** 	订单明细单ID */
		[XmlElement("orderItemId")]
			public long?  OrderItemId{ get; set; }

	}
}
