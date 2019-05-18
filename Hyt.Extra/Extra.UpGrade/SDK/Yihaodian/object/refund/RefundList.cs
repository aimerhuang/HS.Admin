using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Refund
{
	[Serializable]
	public class RefundList 
	{	
		/// <summary>
		///  	退货信息列表
		/// </summary>
		[XmlElement("refund")]
		public List<Refund>  Refund{ get; set; }
	}
}
