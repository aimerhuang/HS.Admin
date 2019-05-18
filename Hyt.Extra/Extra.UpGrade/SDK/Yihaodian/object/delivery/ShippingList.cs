using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Delivery
{
	[Serializable]
	public class ShippingList 
	{	
		/// <summary>
		/// 物流订单列表
		/// </summary>
		[XmlElement("shipping")]
		public List<Shipping>  Shipping{ get; set; }
	}
}
