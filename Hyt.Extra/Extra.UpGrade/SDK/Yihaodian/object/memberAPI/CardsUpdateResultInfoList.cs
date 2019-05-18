using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.MemberAPI
{
	[Serializable]
	public class CardsUpdateResultInfoList 
	{	
		/// <summary>
		/// 卡信息更新返回结果
		/// </summary>
		[XmlElement("cardsUpdateResultInfo")]
		public List<CardsUpdateResultInfo>  CardsUpdateResultInfo{ get; set; }
	}
}
