using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Category
{
	[Serializable]
	public class CategoryAttributeInfoList 
	{	
		/// <summary>
		/// 属性信息列表
		/// </summary>
		[XmlElement("categoryAttributeInfo")]
		public List<CategoryAttributeInfo>  CategoryAttributeInfo{ get; set; }
	}
}
