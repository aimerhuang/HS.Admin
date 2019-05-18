using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Logistics
{
	/// <summary>
	/// 仓库信息
	/// </summary>
	[Serializable]
	public class Warehouse 
	{
		/**是否为默认仓库(1表示默认仓库，0表示非默认仓库) */
		[XmlElement("isDefaultWarehouse ")]
			public int?  IsDefaultWarehouse { get; set; }

		/**仓库ID */
		[XmlElement("warehouseId")]
			public long?  WarehouseId{ get; set; }

		/**仓库名称 */
		[XmlElement("warehouseName")]
			public string  WarehouseName{ get; set; }

		/**配送范围信息列表 */
		[XmlElement("warehouseRangeInfoList")]
		public WarehouseRangeList  WarehouseRangeInfoList{ get; set; }

	}
}
