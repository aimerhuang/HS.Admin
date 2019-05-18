using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.GroupproductAPI
{
	/// <summary>
	/// 区域商家产品库存
	/// </summary>
	[Serializable]
	public class PmStockMerchant 
	{
		/**是否支持虚拟库存(1：是、0：否) */
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

		/**区域商家id */
		[XmlElement("merchantId")]
			public long?  MerchantId{ get; set; }

	}
}
