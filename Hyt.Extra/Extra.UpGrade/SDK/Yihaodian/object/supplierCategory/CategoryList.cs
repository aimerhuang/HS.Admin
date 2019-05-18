using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierCategory
{
	[Serializable]
	public class CategoryList 
	{	
		/// <summary>
		/// 新品类别名称列表
		/// </summary>
		[XmlElement("category")]
		public List<Category>  Category{ get; set; }
	}
}
