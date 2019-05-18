using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Logistics
{
	[Serializable]
	public class WarehouseRangeList 
	{	
		/// <summary>
		/// 配送范围信息列表
		/// </summary>
		[XmlElement("warehouseRange")]
		public List<WarehouseRange>  WarehouseRange{ get; set; }
	}
}
