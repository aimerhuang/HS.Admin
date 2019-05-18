using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.MerchantCrm
{
	[Serializable]
	public class MemberBadgeLevelList 
	{	
		/// <summary>
		/// 商家设置的会员等级信息
		/// </summary>
		[XmlElement("member_badge_level")]
		public List<MemberBadgeLevel>  Member_badge_level{ get; set; }
	}
}
