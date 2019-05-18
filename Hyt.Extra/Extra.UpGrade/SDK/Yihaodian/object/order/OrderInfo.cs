using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	/// <summary>
	/// 订单信息
	/// </summary>
	[Serializable]
	public class OrderInfo 
	{
		/**订单详细信息 */
		[XmlElement("orderDetail")]
		public OrderDetail  OrderDetail{ get; set; }

		/**订单明细 */
		[XmlElement("orderItemList")]
		public OrderItemList  OrderItemList{ get; set; }

	}
}
