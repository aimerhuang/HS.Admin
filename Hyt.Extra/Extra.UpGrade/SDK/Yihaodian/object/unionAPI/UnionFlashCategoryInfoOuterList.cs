using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	[Serializable]
	public class UnionFlashCategoryInfoOuterList 
	{	
		/// <summary>
		/// 名品类目集合对象
		/// </summary>
		[XmlElement("union_flash_category_info_outer")]
		public List<UnionFlashCategoryInfoOuter>  Union_flash_category_info_outer{ get; set; }
	}
}
