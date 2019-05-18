using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Promotion
{
	/// <summary>
	/// 产品促销信息
	/// </summary>
	[Serializable]
	public class ProductPromDetail 
	{
		/**促销id */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**开始时间 */
		[XmlElement("productPromStartDate")]
			public string  ProductPromStartDate{ get; set; }

		/**结束时间 */
		[XmlElement("productPromEndDate")]
			public string  ProductPromEndDate{ get; set; }

		/**1:尚未生效 2:生效中  */
		[XmlElement("status")]
			public int?  Status{ get; set; }

		/**促销是否被锁定 */
		[XmlElement("isLocked")]
			public int?  IsLocked{ get; set; }

		/**取消原因 */
		[XmlElement("reason")]
			public string  Reason{ get; set; }

		/**促销产品id,只会是虚品和普通产品 */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**促销类型，0,商家价格促销2为团购4运营平台 */
		[XmlElement("ruleType")]
			public int?  RuleType{ get; set; }

		/**促销价格 */
		[XmlElement("productPromNonMemPrice")]
			public double?  ProductPromNonMemPrice{ get; set; }

		/**虚品为1，普通产品为0 */
		[XmlElement("isVisualSerial")]
			public int?  IsVisualSerial{ get; set; }

		/**总限制数量 0表示无限制 */
		[XmlElement("specialPriceLimitNumber")]
			public int?  SpecialPriceLimitNumber{ get; set; }

		/**每人特价限制数量 */
		[XmlElement("userPriceLimitNumber")]
			public int?  UserPriceLimitNumber{ get; set; }

		/**是否促销 1：促销 3：特价，针对人  4：特价，针对总数量 */
		[XmlElement("isPromotion")]
			public int?  IsPromotion{ get; set; }

		/**价格促销同时是否生成免邮促销 0：否 1：是  */
		[XmlElement("isAttachShippingProm")]
			public int?  IsAttachShippingProm{ get; set; }

		/**邮费 */
		[XmlElement("postage")]
			public string  Postage{ get; set; }

	}
}
