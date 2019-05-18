using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Category
{
	[Serializable]
	public class AttributeItemInfoList 
	{	
		/// <summary>
		/// 选项属性子值信息列表（只有类别基本属性才有，系列属性忽略此项）
		/// </summary>
		[XmlElement("attributeItemInfo")]
		public List<AttributeItemInfo>  AttributeItemInfo{ get; set; }
	}
}
