using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Trade
{
	[Serializable]
	public class OrderList 
	{	
		/// <summary>
		/// 订单列表
		/// </summary>
		[XmlElement("order")]
		public List<Order>  Order{ get; set; }
	}
}
