using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	[Serializable]
	public class HoldPmInfoExtInfoList 
	{	
		/// <summary>
		/// 集团区域商家商品信息，限集团商家
		/// </summary>
		[XmlElement("holdPmInfoExtInfo")]
		public List<HoldPmInfoExtInfo>  HoldPmInfoExtInfo{ get; set; }
	}
}
