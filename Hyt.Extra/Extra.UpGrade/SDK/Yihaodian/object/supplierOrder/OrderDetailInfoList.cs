using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierOrder
{
	[Serializable]
	public class OrderDetailInfoList 
	{	
		/// <summary>
		/// 历史订单列表
		/// </summary>
		[XmlElement("orderDetailInfo")]
		public List<OrderDetailInfo>  OrderDetailInfo{ get; set; }
	}
}
