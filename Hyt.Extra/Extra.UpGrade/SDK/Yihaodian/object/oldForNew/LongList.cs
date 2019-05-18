using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.OldForNew
{
	[Serializable]
	public class LongList 
	{	
		/// <summary>
		/// 交易单ids
		/// </summary>
		[XmlElement("transactionBillId")]
		public List<long?>  TransactionBillId{ get; set; }
	}
}
