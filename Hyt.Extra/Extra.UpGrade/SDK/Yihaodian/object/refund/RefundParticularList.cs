using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Refund
{
	[Serializable]
	public class RefundParticularList 
	{	
		/// <summary>
		///  	退款单简要详情
		/// </summary>
		[XmlElement("refundParticular")]
		public List<RefundParticular>  RefundParticular{ get; set; }
	}
}
