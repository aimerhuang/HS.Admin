using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	[Serializable]
	public class OrderItemList 
	{	
		/// <summary>
		/// 订单明细
		/// </summary>
		[XmlElement("orderItem")]
		public List<OrderItem>  OrderItem{ get; set; }
	}
}
