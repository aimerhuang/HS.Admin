using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Category
{
	[Serializable]
	public class SerialAttributeInfoList 
	{	
		/// <summary>
		/// 系列属性信息列表
		/// </summary>
		[XmlElement("serialAttributeInfo")]
		public List<SerialAttributeInfo>  SerialAttributeInfo{ get; set; }
	}
}
