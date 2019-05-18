using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	[Serializable]
	public class StringList 
	{	
		/// <summary>
		/// 发票金额
		/// </summary>
		[XmlElement("detailAmount")]
		public List<String>  DetailAmount{ get; set; }
	}
}
