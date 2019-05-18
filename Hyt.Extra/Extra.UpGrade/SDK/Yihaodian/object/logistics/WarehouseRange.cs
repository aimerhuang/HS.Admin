using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Logistics
{
	/// <summary>
	/// 仓库配送范围信息
	/// </summary>
	[Serializable]
	public class WarehouseRange 
	{
		/**仓库ID */
		[XmlElement("warehouseId")]
			public long?  WarehouseId{ get; set; }

		/**省份ID */
		[XmlElement("provinceId")]
			public long?  ProvinceId{ get; set; }

		/**省份名称 */
		[XmlElement("provinceName")]
			public string  ProvinceName{ get; set; }

	}
}
