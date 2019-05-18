using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.GroupMerchantOrderAPI
{
	[Serializable]
	public class StandardOrderPromotionDetailList 
	{	
		/// <summary>
		/// 订单参加促销的明细
		/// </summary>
		[XmlElement("standardOrderPromotionDetail")]
		public List<StandardOrderPromotionDetail>  StandardOrderPromotionDetail{ get; set; }
	}
}
