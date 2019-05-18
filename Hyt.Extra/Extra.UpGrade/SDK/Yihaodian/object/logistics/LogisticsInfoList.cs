using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Logistics
{
	[Serializable]
	public class LogisticsInfoList 
	{	
		/// <summary>
		/// 物流信息列表
		/// </summary>
		[XmlElement("logisticsInfo")]
		public List<LogisticsInfo>  LogisticsInfo{ get; set; }
	}
}
