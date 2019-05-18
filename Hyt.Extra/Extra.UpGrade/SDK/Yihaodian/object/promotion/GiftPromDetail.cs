using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Promotion
{
	/// <summary>
	/// 满换购促销详细信息
	/// </summary>
	[Serializable]
	public class GiftPromDetail 
	{
		/**促销id */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**促销开始时间，格式如示例，开始时间必须是当前日期之前 */
		[XmlElement("startDate")]
			public string  StartDate{ get; set; }

		/**促销结束时间，格式如示例， 产品促销时间不能超过一个月！ */
		[XmlElement("endDate")]
			public string  EndDate{ get; set; }

		/**促销广告语 */
		[XmlElement("promotionTitle")]
			public string  PromotionTitle{ get; set; }

		/**满减类型。1：商品   2:分类   3:分类+品牌,4:全场 */
		[XmlElement("fullGiftType")]
			public int?  FullGiftType{ get; set; }

		/**0:已取消 1:尚未生效 2:生效中 3:已过期  */
		[XmlElement("status")]
			public int?  Status{ get; set; }

		/**促销是否被锁定 */
		[XmlElement("isLocked")]
			public int?  IsLocked{ get; set; }

		/**每个订单是否允许重复参加活动 */
		[XmlElement("repeat")]
			public int?  Repeat{ get; set; }

		/**优惠值 */
		[XmlElement("conditionValue")]
			public string  ConditionValue{ get; set; }

		/**排除商品ID “,”组合 */
		[XmlElement("excludeProductIds")]
			public string  ExcludeProductIds{ get; set; }

		/**商品信息串，按商品添加时有值。“productId,subId1,subId2...;productId,subId1,subId2...;” */
		[XmlElement("productInfo")]
			public string  ProductInfo{ get; set; }

		/**分类信息串，按分类添加时有值。“categoryId;categoryId;” */
		[XmlElement("categoryInfo")]
			public string  CategoryInfo{ get; set; }

		/**品牌分类信息串，按品牌分类添加时有值。“brandId,categoryId;brandId,categoryId;” */
		[XmlElement("brandCategoryInfo")]
			public string  BrandCategoryInfo{ get; set; }

		/**0：送完为止  1：限定总换购数量  2：限定每天换购数量 */
		[XmlElement("giftProductInfo")]
			public string  GiftProductInfo{ get; set; }

		/**0：送完为止  1：限定总换购数量  2：限定每天换购数量 */
		[XmlElement("totalLimitType")]
			public int?  TotalLimitType{ get; set; }

		/**优惠类型：1-部分商品购满X元;          2-部分商品购满X件 */
		[XmlElement("conditionType")]
			public int?  ConditionType{ get; set; }

	}
}
