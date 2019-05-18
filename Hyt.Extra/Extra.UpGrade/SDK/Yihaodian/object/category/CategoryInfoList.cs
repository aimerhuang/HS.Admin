using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Category
{
	[Serializable]
	public class CategoryInfoList 
	{	
		/// <summary>
		/// 产品信息列表
		/// </summary>
		[XmlElement("categoryInfo")]
		public List<CategoryInfo>  CategoryInfo{ get; set; }
	}
}
