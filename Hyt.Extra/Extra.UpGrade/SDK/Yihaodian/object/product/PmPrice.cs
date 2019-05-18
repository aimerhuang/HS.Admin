using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
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

		/**外部产品ID */
		[XmlElement("outerId")]
			public string  OuterId{ get; set; }

		/**进价 */
		[XmlElement("inPrice")]
			public double?  InPrice{ get; set; }

		/**1号店价 */
		[XmlElement("nonMemberPrice")]
			public double?  NonMemberPrice{ get; set; }

		/**促销1号店价 */
		[XmlElement("promNonMemberPrice")]
			public double?  PromNonMemberPrice{ get; set; }

		/**市场价 */
		[XmlElement("productListPrice")]
			public double?  ProductListPrice{ get; set; }

		/**促销开始时间 */
		[XmlElement("promStartTime")]
			public string  PromStartTime{ get; set; }

		/**促销结束时间 */
		[XmlElement("promEndTime")]
			public string  PromEndTime{ get; set; }

		/**特价限制总数量:0表示无限制 */
		[XmlElement("specialPriceLimitNumber")]
			public int?  SpecialPriceLimitNumber{ get; set; }

		/**每人特价限制数量:0表示无限制 */
		[XmlElement("userPriceLimitNumber")]
			public int?  UserPriceLimitNumber{ get; set; }

		/**是否是vip产品（1：是、0：否） */
		[XmlElement("isCanVipDiscount")]
			public int?  IsCanVipDiscount{ get; set; }

		/**商品ID（废弃字段，不要使用） */
		[XmlElement("pmInfoId")]
			public long?  PmInfoId{ get; set; }

		/**区域商家商品信息，限集团商家 */
		[XmlElement("areaMerchantProductList")]
		public HoldPmInfoExtHedwigVoList  AreaMerchantProductList{ get; set; }

	}
}
