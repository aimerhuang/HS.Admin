using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierProd
{
	/// <summary>
	/// 库存信息
	/// </summary>
	[Serializable]
	public class StockInfo 
	{
		/**是否支持虚拟库存(1：是；0：否) */
		[XmlElement("isSupportVS")]
			public int?  IsSupportVS{ get; set; }

		/**实际库存 */
		[XmlElement("vs")]
			public int?  Vs{ get; set; }

		/**冻结库存(下单冻结,付款释放) */
		[XmlElement("vsf")]
			public int?  Vsf{ get; set; }

		/**仓库ID */
		[XmlElement("warehouseId")]
			public long?  WarehouseId{ get; set; }

		/**仓库名称 */
		[XmlElement("warehouseName")]
			public string  WarehouseName{ get; set; }

	}
}
