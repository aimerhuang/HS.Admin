using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierOrder
{
	[Serializable]
	public class OrderItemDetailInfoList 
	{	
		/// <summary>
		/// 商品详情
		/// </summary>
		[XmlElement("orderItemDetailInfo")]
		public List<OrderItemDetailInfo>  OrderItemDetailInfo{ get; set; }
	}
}
