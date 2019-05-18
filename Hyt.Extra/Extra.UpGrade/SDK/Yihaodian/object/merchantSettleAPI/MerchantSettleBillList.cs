using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.MerchantSettleAPI
{
	[Serializable]
	public class MerchantSettleBillList 
	{	
		/// <summary>
		/// 账单明细
		/// </summary>
		[XmlElement("merchantSettleBill")]
		public List<MerchantSettleBill>  MerchantSettleBill{ get; set; }
	}
}
