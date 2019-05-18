using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierProd
{
	/// <summary>
	/// 供应商产品信息
	/// </summary>
	[Serializable]
	public class Product 
	{
		/**产品编码 */
		[XmlElement("productCode")]
			public string  ProductCode{ get; set; }

		/**产品中文名称 */
		[XmlElement("productCname")]
			public string  ProductCname{ get; set; }

		/**产品ID(没有通过审核的产品是没有产品ID的) */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**产品条形码 */
		[XmlElement("ean13")]
			public string  Ean13{ get; set; }

		/**产品类目ID */
		[XmlElement("categoryId")]
			public long?  CategoryId{ get; set; }

		/**图片信息列表（逗号分隔，图片id、图片URL、主图标识之间用竖线分隔；其中1：表示主图，0：表示非主图） */
		[XmlElement("prodImg")]
			public string  ProdImg{ get; set; }

		/**品牌Id */
		[XmlElement("brandId")]
			public long?  BrandId{ get; set; }

		/**商家产品信息列表 */
		[XmlElement("merchantProductInfoList")]
		public MerchantProductInfoList  MerchantProductInfoList{ get; set; }

		/**产品类型 0：普通产品 1：主系列产品 2：子系列产品 3：捆绑产品 4：实体礼品卡 5: 虚拟商品 6:增值服务 7:电子礼品卡 */
		[XmlElement("productType")]
			public int?  ProductType{ get; set; }

	}
}
