using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierProd
{
	/// <summary>
	/// 产品价格
	/// </summary>
	[Serializable]
	public class PmPrice 
	{
		/**产品ID */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**进价 */
		[XmlElement("inPrice")]
			public double?  InPrice{ get; set; }

		/**市场价 */
		[XmlElement("marketPrice")]
			public double?  MarketPrice{ get; set; }

		/**商家产品价格信息列表 */
		[XmlElement("merchantPriceInfoList")]
		public MerchantPriceInfoList  MerchantPriceInfoList{ get; set; }

	}
}
