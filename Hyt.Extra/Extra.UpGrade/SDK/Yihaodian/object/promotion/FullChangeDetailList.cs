using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Promotion
{
	[Serializable]
	public class FullChangeDetailList 
	{	
		/// <summary>
		/// 返回的结果
		/// </summary>
		[XmlElement("fullChangeDetail")]
		public List<FullChangeDetail>  FullChangeDetail{ get; set; }
	}
}
