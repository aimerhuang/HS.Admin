using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Promotion
{
	[Serializable]
	public class FullMinusDetailList 
	{	
		/// <summary>
		/// 满减促销信息列表
		/// </summary>
		[XmlElement("fullMinusDetail")]
		public List<FullMinusDetail>  FullMinusDetail{ get; set; }
	}
}
