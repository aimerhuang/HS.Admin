using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierProd
{
	/// <summary>
	/// 供应商系列产品
	/// </summary>
	[Serializable]
	public class SerialProduct 
	{
		/**产品编码 */
		[XmlElement("productCode")]
			public string  ProductCode{ get; set; }

		/**产品中文名称 */
		[XmlElement("productCname")]
			public string  ProductCname{ get; set; }

		/**产品ID */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**商家ID */
		[XmlElement("merchantId")]
			public long?  MerchantId{ get; set; }

		/**是否可见(强制上/下架),1是0否 */
		[XmlElement("canShow")]
			public int?  CanShow{ get; set; }

		/**上下架状态0：下架，1：上架 */
		[XmlElement("canSale")]
			public int?  CanSale{ get; set; }

		/**产品条形码 */
		[XmlElement("ean13")]
			public string  Ean13{ get; set; }

		/**产品类目ID */
		[XmlElement("categoryId")]
			public long?  CategoryId{ get; set; }

		/**前台商品详情页链接（正式产品才会有） */
		[XmlElement("prodDetailUrl")]
			public string  ProdDetailUrl{ get; set; }

		/**品牌Id */
		[XmlElement("brandId")]
			public long?  BrandId{ get; set; }

	}
}
