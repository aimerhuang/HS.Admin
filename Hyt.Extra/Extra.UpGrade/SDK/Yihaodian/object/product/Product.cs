using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	/// <summary>
	/// 商家产品
	/// </summary>
	[Serializable]
	public class Product 
	{
		/**商家产品编码 */
		[XmlElement("productCode")]
			public string  ProductCode{ get; set; }

		/**商家产品中文名称 */
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

		/**上下架状态0：下架，1：上架 */
		[XmlElement("canSale")]
			public int?  CanSale{ get; set; }

		/**产品库存状态 */
		[XmlElement("stockStatus")]
			public int?  StockStatus{ get; set; }

		/**外部产品ID */
		[XmlElement("outerId")]
			public string  OuterId{ get; set; }

		/**是否可见,1是0否 */
		[XmlElement("canShow")]
			public int?  CanShow{ get; set; }

		/**产品审核状态:1.新增未审核;2.编辑待审核;3.审核未通过;4.审核通过;5.图片审核失败;6.文描审核失败;7:生码中(第一次审核中) */
		[XmlElement("verifyFlg")]
			public int?  VerifyFlg{ get; set; }

		/**是否二次审核0：非二次审核；1：是二次审核 */
		[XmlElement("isDupAudit")]
			public int?  IsDupAudit{ get; set; }

		/**图片信息列表（逗号分隔，图片id、图片URL、主图标识之间用竖线分隔；其中1：表示主图，0：表示非主图） */
		[XmlElement("prodImg")]
			public string  ProdImg{ get; set; }

		/**前台商品详情页链接（正式产品才会有） */
		[XmlElement("prodDetailUrl")]
			public string  ProdDetailUrl{ get; set; }

		/**品牌Id */
		[XmlElement("brandId")]
			public long?  BrandId{ get; set; }

		/**商家产品类别。多个类别用逗号分隔 */
		[XmlElement("merchantCategoryId")]
			public string  MerchantCategoryId{ get; set; }

		/**产品条形码 */
		[XmlElement("barcode")]
			public string  Barcode{ get; set; }

		/**是否为虚品(1：是、0：否 )（废弃字段，不要使用） */
		[XmlElement("productType")]
			public int?  ProductType{ get; set; }

		/**商品ID（废弃字段，不要使用） */
		[XmlElement("pmInfoId")]
			public long?  PmInfoId{ get; set; }

		/**产品ID（废弃字段，不要使用） */
		[XmlElement("fromalProductId")]
			public long?  FromalProductId{ get; set; }

		/**集团区域商家商品信息，限集团商家 */
		[XmlElement("holdPmInfoExtInfoList")]
		public HoldPmInfoExtInfoList  HoldPmInfoExtInfoList{ get; set; }

	}
}
