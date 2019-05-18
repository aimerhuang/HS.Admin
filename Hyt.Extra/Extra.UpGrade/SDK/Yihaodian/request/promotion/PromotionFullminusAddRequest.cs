using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 新增满减促销
	/// </summary>
	public class PromotionFullminusAddRequest 
		: IYhdRequest<PromotionFullminusAddResponse> 
	{
		/**促销开始时间，格式如示例，产品促销时间不能超过一个月！ */
			public string  StartDate{ get; set; }

		/**促销结束时间，格式如示例，产品促销时间不能超过一个月 */
			public string  EndDate{ get; set; }

		/**满减类型。1：商品   2:分类   3:分类+品牌,4:全场 */
			public int?  FullMinusType{ get; set; }

		/**促销活动广告语,最多只能输入50个字符,不能有特殊字符
['@', '#', '$', '%', '^', '&', '*', '<', '>','\'','\"', '/','＠','＃','￥','％','《','》','＆','‘','’','“','”'] */
			public string  PromotionTitle{ get; set; }

		/**每个订单是否允许重复参加活动，0:否  1:是 */
			public int?  Repeat{ get; set; }

		/**每个账号可参加活动次数, =0:不限 >0限制次数,最大1000，大于1000自动设为1000 */
			public int?  LimitNumPerUser{ get; set; }

		/**多级优惠类型: 1:只能参加一级 2:可重复参加。conditionValues只有1级时必须为1 */
			public int?  JoinLevelType{ get; set; }

		/**优惠条件：每个账号单笔消费满多少元,多级优惠“，”拼接，递增，和contentValues一一对应,且对应的优惠金额必须小于优惠条件的50%。小数点后最多两位 */
			public string  ConditionValues{ get; set; }

		/**设置的促销普通商品id,以“;”分隔（当满减类型为1时 和productSerialIds不能同时为空，两者相加最多100个） */
			public string  ProductIds{ get; set; }

		/**设置的促销系列商品,如果后面没有子品id则默认添加所有子品 （当满减类型为1时和productIds不能同时为空） */
			public string  ProductSerialIds{ get; set; }

		/**类别ID 组合，“;”分隔（当满减类型为2时必须），最多50个 */
			public string  CategoryIds{ get; set; }

		/**品牌id:分类id 组合 ，  “;” “:”分隔   （当满减类型为3时必须），最多50对 */
			public string  BrandCategoryIds{ get; set; }

		/**满减类型为2或3时，需要排除的商品id，以“，”分隔，最多20个 */
			public string  ExcludeProductIds{ get; set; }

		/**优惠金额：减现金XX元  多级优惠  “，”拼接,递增。小数点后最多两位 */
			public string  ContentValues{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.promotion.fullminus.add";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("startDate", this.StartDate);
			parameters.Add("endDate", this.EndDate);
			parameters.Add("fullMinusType", this.FullMinusType);
			parameters.Add("promotionTitle", this.PromotionTitle);
			parameters.Add("repeat", this.Repeat);
			parameters.Add("limitNumPerUser", this.LimitNumPerUser);
			parameters.Add("joinLevelType", this.JoinLevelType);
			parameters.Add("conditionValues", this.ConditionValues);
			parameters.Add("productIds", this.ProductIds);
			parameters.Add("productSerialIds", this.ProductSerialIds);
			parameters.Add("categoryIds", this.CategoryIds);
			parameters.Add("brandCategoryIds", this.BrandCategoryIds);
			parameters.Add("excludeProductIds", this.ExcludeProductIds);
			parameters.Add("contentValues", this.ContentValues);
			return parameters;
		}
		#endregion
	}
}
