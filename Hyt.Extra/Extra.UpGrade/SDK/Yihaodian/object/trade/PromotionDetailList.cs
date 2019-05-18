using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Trade
{
	[Serializable]
	public class PromotionDetailList 
	{	
		/// <summary>
		/// 优惠详情（暂不提供）
		/// </summary>
		[XmlElement("promotion_detail")]
		public List<PromotionDetail>  Promotion_detail{ get; set; }
	}
}
