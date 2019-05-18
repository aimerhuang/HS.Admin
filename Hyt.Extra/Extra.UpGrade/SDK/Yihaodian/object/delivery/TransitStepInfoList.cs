using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Delivery
{
	[Serializable]
	public class TransitStepInfoList 
	{	
		/// <summary>
		/// 流转信息列表
		/// </summary>
		[XmlElement("transit_step_info")]
		public List<TransitStepInfo>  Transit_step_info{ get; set; }
	}
}
