using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	/// <summary>
	/// 集团区域商家商品信息
	/// </summary>
	[Serializable]
	public class HoldPmInfoExtInfo 
	{
		/**集团商家ID */
		[XmlElement("merchantParentId")]
			public long?  MerchantParentId{ get; set; }

		/**区域商家ID */
		[XmlElement("merchantId")]
			public long?  MerchantId{ get; set; }

		/**临时产品ID */
		[XmlElement("holdProductId")]
			public long?  HoldProductId{ get; set; }

		/**商家编码 */
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

		/**实际库存 */
		[XmlElement("realStockNum")]
			public double?  RealStockNum{ get; set; }

		/**上下架状态0：下架，1：上架 */
		[XmlElement("canShow")]
			public int?  CanShow{ get; set; }

		/**是否可见1：是，0：否 */
		[XmlElement("canSale")]
			public int?  CanSale{ get; set; }

		/**下架原因类型 */
		[XmlElement("underCarriageReason")]
			public int?  UnderCarriageReason{ get; set; }

		/**下架原因描述 */
		[XmlElement("underCarriageRemark")]
			public string  UnderCarriageRemark{ get; set; }

		/**商品编码 */
		[XmlElement("productCode")]
			public string  ProductCode{ get; set; }

		/**正式产品ID */
		[XmlElement("fromalProductId")]
			public long?  FromalProductId{ get; set; }

		/**正式pminfoID */
		[XmlElement("fromalPmInfoId")]
			public long?  FromalPmInfoId{ get; set; }

		/**是否删除1：是，0：否 */
		[XmlElement("isDeleted")]
			public int?  IsDeleted{ get; set; }

	}
}
