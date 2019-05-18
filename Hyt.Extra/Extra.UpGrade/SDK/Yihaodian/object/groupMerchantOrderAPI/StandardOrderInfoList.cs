using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.GroupMerchantOrderAPI
{
	[Serializable]
	public class StandardOrderInfoList 
	{	
		/// <summary>
		/// 订单信息列表
		/// </summary>
		[XmlElement("standardOrderInfo")]
		public List<StandardOrderInfo>  StandardOrderInfo{ get; set; }
	}
}
