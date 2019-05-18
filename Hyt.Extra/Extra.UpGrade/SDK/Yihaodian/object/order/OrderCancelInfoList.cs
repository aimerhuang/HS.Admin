using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	[Serializable]
	public class OrderCancelInfoList 
	{	
		/// <summary>
		/// 取消订单信息列表
		/// </summary>
		[XmlElement("orderCancelInfo")]
		public List<OrderCancelInfo>  OrderCancelInfo{ get; set; }
	}
}
