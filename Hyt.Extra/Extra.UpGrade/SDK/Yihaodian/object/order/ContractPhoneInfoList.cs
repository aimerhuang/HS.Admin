using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	[Serializable]
	public class ContractPhoneInfoList 
	{	
		/// <summary>
		/// 合约机订单记录列表
		/// </summary>
		[XmlElement("contractPhoneInfo")]
		public List<ContractPhoneInfo>  ContractPhoneInfo{ get; set; }
	}
}
