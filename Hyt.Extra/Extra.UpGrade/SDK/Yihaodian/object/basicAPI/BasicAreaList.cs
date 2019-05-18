using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.BasicAPI
{
	[Serializable]
	public class BasicAreaList 
	{	
		/// <summary>
		/// 所有省级列表结果
		/// </summary>
		[XmlElement("basicArea")]
		public List<BasicArea>  BasicArea{ get; set; }
	}
}
