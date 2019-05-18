using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Promotion
{
	/// <summary>
	/// 满立减信息
	/// </summary>
	[Serializable]
	public class FullMinusDetail 
	{
		/**促销id */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**开始时间 */
		[XmlElement("startDate")]
			public string  StartDate{ get; set; }

		/**结束时间 */
		[XmlElement("endDate")]
			public string  EndDate{ get; set; }

		/**促销广告语 */
		[XmlElement("promotionTitle")]
			public string  PromotionTitle{ get; set; }

		/**满减类型。1：商品   2:分类   3:分类+品牌,4:全场 */
		[XmlElement("fullMinusType")]
			public int?  FullMinusType{ get; set; }

		/**0:已取消 1:尚未生效 2:生效中 3:已过期  */
		[XmlElement("status")]
			public int?  Status{ get; set; }

		/**促销是否被锁定 */
		[XmlElement("isLocked")]
			public int?  IsLocked{ get; set; }

		/**每个订单是否允许重复参加活动 */
		[XmlElement("repeat")]
			public int?  Repeat{ get; set; }

		/**每个账号可参加活动次数, 0 -- 不限 >0限制次数 */
		[XmlElement("limitNumPerUser")]
			public int?  LimitNumPerUser{ get; set; }

		/**多级优惠类型: 1--只能参加一级 2--可重复参加 */
		[XmlElement("joinLevelType")]
			public int?  JoinLevelType{ get; set; }

		/**满多少减多少串，","、";"分割，500,100;600,110; */
		[XmlElement("conditionContents")]
			public string  ConditionContents{ get; set; }

		/**排除商品ID “,”组合 */
		[XmlElement("excludeProductIds")]
			public int?  ExcludeProductIds{ get; set; }

		/**商品信息串，按商品添加时有值。“productId,subId1,subId2...; */
		[XmlElement("productInfo")]
			public string  ProductInfo{ get; set; }

		/**分类信息串，按分类添加时有值。“categoryId;categoryId;” */
		[XmlElement("categoryInfo")]
			public string  CategoryInfo{ get; set; }

		/**品牌分类信息串，按品牌分类添加时有值。“brandId,categoryId; */
		[XmlElement("brandCategoryInfo")]
			public string  BrandCategoryInfo{ get; set; }

	}
}
