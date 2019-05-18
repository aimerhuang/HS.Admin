using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierProd
{
	/// <summary>
	/// 商家产品价格信息
	/// </summary>
	[Serializable]
	public class MerchantPriceInfo 
	{
		/**商家ID */
		[XmlElement("merchantId")]
			public long?  MerchantId{ get; set; }

		/**商家名称 */
		[XmlElement("merchantName")]
			public string  MerchantName{ get; set; }

		/**1号店价 */
		[XmlElement("nonMemberPrice")]
			public double?  NonMemberPrice{ get; set; }

		/**促销1号店价 */
		[XmlElement("promNonMemberPrice")]
			public double?  PromNonMemberPrice{ get; set; }

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

		/**是否是vip产品（1：是；0：否） */
		[XmlElement("isCanVipDiscount")]
			public int?  IsCanVipDiscount{ get; set; }

	}
}
