using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Data
{
	[Serializable]
	public class MerchantDataList 
	{	
		/// <summary>
		/// 数据详情
		/// </summary>
		[XmlElement("merchantData")]
		public List<MerchantData>  MerchantData{ get; set; }
	}
}
