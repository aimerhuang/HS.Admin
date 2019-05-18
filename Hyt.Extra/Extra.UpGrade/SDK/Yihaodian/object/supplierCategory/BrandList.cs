using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierCategory
{
	[Serializable]
	public class BrandList 
	{	
		/// <summary>
		/// 新品品牌名称列表
		/// </summary>
		[XmlElement("brand")]
		public List<Brand>  Brand{ get; set; }
	}
}
