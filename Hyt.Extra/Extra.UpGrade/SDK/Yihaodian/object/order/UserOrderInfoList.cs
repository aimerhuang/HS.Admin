using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	[Serializable]
	public class UserOrderInfoList 
	{	
		/// <summary>
		/// 用户订单列表信息
		/// </summary>
		[XmlElement("userOrderInfo")]
		public List<UserOrderInfo>  UserOrderInfo{ get; set; }
	}
}
