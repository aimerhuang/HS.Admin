using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Logistics
{
	[Serializable]
	public class WarehouseList 
	{	
		/// <summary>
		/// 仓库信息列表
		/// </summary>
		[XmlElement("warehouse")]
		public List<Warehouse>  Warehouse{ get; set; }
	}
}
