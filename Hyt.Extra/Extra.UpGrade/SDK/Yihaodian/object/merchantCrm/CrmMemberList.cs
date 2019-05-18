using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.MerchantCrm
{
	[Serializable]
	public class CrmMemberList 
	{	
		/// <summary>
		/// 根据一定条件查询的卖家会员
		/// </summary>
		[XmlElement("crm_member")]
		public List<CrmMember>  Crm_member{ get; set; }
	}
}
