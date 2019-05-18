using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	[Serializable]
	public class SerialAttributeInfoList 
	{	
		/// <summary>
		/// 系列属性信息
		/// </summary>
		[XmlElement("serialAttributeInfo")]
		public List<SerialAttributeInfo>  SerialAttributeInfo{ get; set; }
	}
}
