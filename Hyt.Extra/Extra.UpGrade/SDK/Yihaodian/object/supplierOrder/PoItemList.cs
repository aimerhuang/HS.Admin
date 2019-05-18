using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierOrder
{
	[Serializable]
	public class PoItemList 
	{	
		/// <summary>
		/// 采购单详情列表
		/// </summary>
		[XmlElement("poItem")]
		public List<PoItem>  PoItem{ get; set; }
	}
}
