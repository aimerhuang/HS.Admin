using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierProd
{
	/// <summary>
	/// 产品库存
	/// </summary>
	[Serializable]
	public class PmStock 
	{
		/**产品ID */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**商家产品库存信息列表 */
		[XmlElement("merchantStockInfoList")]
		public MerchantStockInfoList  MerchantStockInfoList{ get; set; }

	}
}
