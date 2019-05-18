using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 新增满就换购促销
	/// </summary>
	public class PromotionFullchangeAddRequest 
		: IYhdRequest<PromotionFullchangeAddResponse> 
	{
		/**促销开始时间，格式：yyyy-MM-dd HH:mm:ss，促销开始时间必须在当前时间之后！ 产品促销时间不能超过一个月！ */
			public string  StartDate{ get; set; }

		/**促销结束时间，格式：yyyy-MM-dd HH:mm:ss，产品促销时间不能超过一个月！ */
			public string  EndDate{ get; set; }

		/**满换购类型。1:商品；2:分类；3:分类 品牌；4:全场（当为4时conditionType必须为1） */
			public int?  FullGiftType{ get; set; }

		/**促销活动广告语,最多只能输入50个字符,不能有特殊字符
<xmp>['@', '#', '$', '%', '^', '&', '*', '<', '>','\'','\"', '/','＠','＃','￥','％','《','》','＆','‘','’','“','”']</xmp> */
			public string  PromotionTitle{ get; set; }

		/**每个订单是否允许重复参加活动，0:否；1:是 */
			public int?  Repeat{ get; set; }

		/**每个账号可参加活动次数。=0:不限；>0限制的次数。最大为1000，大于1000自动设为1000。 */
			public int?  LimitNumPerUser{ get; set; }

		/**优惠条件：每个账号单笔消费满多少元或者多少件可以换购商品。为满多少元时，小数点后最多两位。 */
			public string  ConditionValue{ get; set; }

		/**换购数量限制类型。0：送完为止；1：限定总换购数量；2：限定每天换购数量。为0时，产品串giftProductStrs的“数量限制”必须为0。 */
			public int?  TotalLimitType{ get; set; }

		/**设置的促销普通商品列表。各商品id以分号分隔。当满换购类型fullGiftType为1时，productIds和productSerialIds不能同时为空，并且普通商品id   系列商品id个数最多100个。 */
			public string  ProductIds{ get; set; }

		/**设置的促销系列商品,格式为：系列商品之间用分号分隔，括号内是次系列商品的子品，子品间用逗号分隔。如果后面没有子品id则默认添加所有子品。 当满换购类型为1时，productSerialIds和productIds不能同时为空，并且普通商品id   系列商品id个数最多100个。 */
			public string  ProductSerialIds{ get; set; }

		/**类别id列表。多个类别id之间用分号分隔，最多50个。当满换购类型为2时,此参数必选。 */
			public string  CategoryIds{ get; set; }

		/**品牌id、分类id组合列表。品牌id和分类id之间用冒号分隔，多个（品牌id:分类id）之间用分号分隔，最多50对。当满换购类型为3时，此参数必选。 */
			public string  BrandCategoryIds{ get; set; }

		/**满换购类型为2或3时，需要排除的商品列表，此商品类型为普通商品、系列商品子品。各商品id之间用逗号分隔，最多20个。 */
			public string  ExcludeProductIds{ get; set; }

		/**优惠类型：1-部分商品购满X元；2-部分商品购满X件。 */
			public int?  ConditionType{ get; set; }

		/**换购的产品信息。产品串=“产品id,单用户换购数量,数量限制,换购价”，产品串最多3个。换购产品的产品类型为普通商品、系列商品子品，该产品须可销并可见，单用户换购数量不能大于9个；“数量限制”不能大于1000000，否则自动设为1000000。 */
			public string  GiftProductStrs{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.promotion.fullchange.add";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("startDate", this.StartDate);
			parameters.Add("endDate", this.EndDate);
			parameters.Add("fullGiftType", this.FullGiftType);
			parameters.Add("promotionTitle", this.PromotionTitle);
			parameters.Add("repeat", this.Repeat);
			parameters.Add("limitNumPerUser", this.LimitNumPerUser);
			parameters.Add("conditionValue", this.ConditionValue);
			parameters.Add("totalLimitType", this.TotalLimitType);
			parameters.Add("productIds", this.ProductIds);
			parameters.Add("productSerialIds", this.ProductSerialIds);
			parameters.Add("categoryIds", this.CategoryIds);
			parameters.Add("brandCategoryIds", this.BrandCategoryIds);
			parameters.Add("excludeProductIds", this.ExcludeProductIds);
			parameters.Add("conditionType", this.ConditionType);
			parameters.Add("giftProductStrs", this.GiftProductStrs);
			return parameters;
		}
		#endregion
	}
}
