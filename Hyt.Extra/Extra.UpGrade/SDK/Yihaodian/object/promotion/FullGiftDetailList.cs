using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Promotion
{
	[Serializable]
	public class FullGiftDetailList 
	{	
		/// <summary>
		/// 返回促销信息的结果列表
		/// </summary>
		[XmlElement("fullGiftDetail")]
		public List<FullGiftDetail>  FullGiftDetail{ get; set; }
	}
}
