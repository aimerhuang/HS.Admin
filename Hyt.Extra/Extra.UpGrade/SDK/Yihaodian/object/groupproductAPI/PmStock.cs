using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.GroupproductAPI
{
	/// <summary>
	/// 集团产品库存
	/// </summary>
	[Serializable]
	public class PmStock 
	{
		/**产品ID */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**外部产品ID */
		[XmlElement("outerId")]
			public string  OuterId{ get; set; }

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

		/**仓库名称 */
		[XmlElement("warehouseName")]
			public string  WarehouseName{ get; set; }

		/**区域商家产品库存对象 */
		[XmlElement("pmStockMerchant")]
		public PmStockMerchantList  PmStockMerchant{ get; set; }

	}
}
