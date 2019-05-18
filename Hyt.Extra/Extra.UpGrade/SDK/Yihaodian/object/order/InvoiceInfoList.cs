using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	[Serializable]
	public class InvoiceInfoList 
	{	
		/// <summary>
		/// 发票信息列表
		/// </summary>
		[XmlElement("invoiceInfo")]
		public List<InvoiceInfo>  InvoiceInfo{ get; set; }
	}
}
