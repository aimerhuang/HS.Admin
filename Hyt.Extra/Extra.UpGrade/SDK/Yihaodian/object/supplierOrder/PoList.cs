using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierOrder
{
	[Serializable]
	public class PoList 
	{	
		/// <summary>
		/// 返回结果
		/// </summary>
		[XmlElement("po")]
		public List<Po>  Po{ get; set; }
	}
}
