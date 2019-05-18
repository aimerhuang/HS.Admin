using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	/// <summary>
	/// 区域商家商品信息临时表
	/// </summary>
	[Serializable]
	public class HoldPmInfoExtHedwigVo 
	{
		/**记录的主键 */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**集团商家id */
		[XmlElement("merchantParentId")]
			public long?  MerchantParentId{ get; set; }

		/**区域商家id */
		[XmlElement("merchantId")]
			public long?  MerchantId{ get; set; }

		/**区域商家名称 */
		[XmlElement("merchantName")]
			public string  MerchantName{ get; set; }

		/**集团商家的临时产品id */
		[XmlElement("holdProductId")]
			public long?  HoldProductId{ get; set; }

		/**0普通单品  1系列品   2系列子品 */
		[XmlElement("productType")]
			public int?  ProductType{ get; set; }

		/**区域商家商品的商家编码 */
		[XmlElement("outerId")]
			public string  OuterId{ get; set; }

		/**市场价 */
		[XmlElement("productListPrice")]
			public double?  ProductListPrice{ get; set; }

		/**1号店价 */
		[XmlElement("yhdPrice")]
			public double?  YhdPrice{ get; set; }

		/**运费模板id */
		[XmlElement("feeTemplateId")]
			public long?  FeeTemplateId{ get; set; }

		/**库存 */
		[XmlElement("realStockNum")]
			public double?  RealStockNum{ get; set; }

		/**可见，0：不可见  1：可见 */
		[XmlElement("canShow")]
			public int?  CanShow{ get; set; }

		/**可销，0：不可销  1：可销 */
		[XmlElement("canSale")]
			public int?  CanSale{ get; set; }

		/**下架原因类型 */
		[XmlElement("underCarriageReason")]
			public int?  UnderCarriageReason{ get; set; }

		/**下架原因描述 */
		[XmlElement("underCarriageRemark")]
			public string  UnderCarriageRemark{ get; set; }

		/**商家编码 */
		[XmlElement("productCode")]
			public string  ProductCode{ get; set; }

		/**区域商家商品的正式产品Id */
		[XmlElement("fromalProductId")]
			public long?  FromalProductId{ get; set; }

		/**区域商家商品的正式pmInfoId */
		[XmlElement("fromalPmInfoId")]
			public long?  FromalPmInfoId{ get; set; }

		/**是否已删除，0：未删除  1：已删除 */
		[XmlElement("isDeleted")]
			public int?  IsDeleted{ get; set; }

		/**创建时间 */
		[XmlElement("createTime")]
			public string  CreateTime{ get; set; }

		/**创建人id */
		[XmlElement("createOperatorId")]
			public long?  CreateOperatorId{ get; set; }

		/**更新时间 */
		[XmlElement("updateTime")]
			public string  UpdateTime{ get; set; }

		/**更新人id */
		[XmlElement("updateOperatorId")]
			public long?  UpdateOperatorId{ get; set; }

		/**集团商家商品holdPmInfoId */
		[XmlElement("holdPmInfoId")]
			public long?  HoldPmInfoId{ get; set; }

	}
}
