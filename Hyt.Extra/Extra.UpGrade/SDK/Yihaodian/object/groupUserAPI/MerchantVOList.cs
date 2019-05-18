using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.GroupUserAPI
{
	[Serializable]
	public class MerchantVOList 
	{	
		/// <summary>
		/// 区域商家列表
		/// </summary>
		[XmlElement("merchantVO")]
		public List<MerchantVO>  MerchantVO{ get; set; }
	}
}
