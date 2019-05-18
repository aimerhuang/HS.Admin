using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.GroupMerchantOrderAPI
{
	[Serializable]
	public class StandardOrderItemList 
	{	
		/// <summary>
		/// 订单商品明细信息
		/// </summary>
		[XmlElement("standardOrderItem")]
		public List<StandardOrderItem>  StandardOrderItem{ get; set; }
	}
}
