using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierProd
{
	/// <summary>
	/// 商家产品信息
	/// </summary>
	[Serializable]
	public class MerchantProductInfo 
	{
		/**商家名称 */
		[XmlElement("merchantName")]
			public string  MerchantName{ get; set; }

		/**商家id */
		[XmlElement("merchantId")]
			public long?  MerchantId{ get; set; }

		/**上下架状态0：下架，1：上架 */
		[XmlElement("canSale")]
			public int?  CanSale{ get; set; }

		/**产品库存状态 */
		[XmlElement("stockStatus")]
			public int?  StockStatus{ get; set; }

		/**是否可见,1是0否 */
		[XmlElement("canShow")]
			public int?  CanShow{ get; set; }

		/**前台商品详情页链接（正式产品才会有） */
		[XmlElement("prodDetailUrl")]
			public string  ProdDetailUrl{ get; set; }

		/**产品商家编码 */
		[XmlElement("outerId")]
			public string  OuterId{ get; set; }

	}
}
