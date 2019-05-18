using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Refund
{
	[Serializable]
	public class RefundItemList 
	{	
		/// <summary>
		/// 退货明细列表
		/// </summary>
		[XmlElement("refundItem")]
		public List<RefundItem>  RefundItem{ get; set; }
	}
}
