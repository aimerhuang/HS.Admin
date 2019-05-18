using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierProd
{
	/// <summary>
	/// 商家产品库存信息
	/// </summary>
	[Serializable]
	public class MerchantStockInfo 
	{
		/**商家id */
		[XmlElement("merchantId")]
			public int?  MerchantId{ get; set; }

		/**商家名称 */
		[XmlElement("merchantName")]
			public string  MerchantName{ get; set; }

		/**产品库存信息列表 */
		[XmlElement("stockInfoList")]
		public StockInfoList  StockInfoList{ get; set; }

	}
}
