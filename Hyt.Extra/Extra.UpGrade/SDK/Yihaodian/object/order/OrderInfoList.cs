using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	[Serializable]
	public class OrderInfoList 
	{	
		/// <summary>
		/// 订单详情列表
		/// </summary>
		[XmlElement("orderInfo")]
		public List<OrderInfo>  OrderInfo{ get; set; }
	}
}
