using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	[Serializable]
	public class SerialProdAttributeInfoList 
	{	
		/// <summary>
		/// 系列子品属性信息列表
		/// </summary>
		[XmlElement("serialProdAttributeInfo")]
		public List<SerialProdAttributeInfo>  SerialProdAttributeInfo{ get; set; }
	}
}
