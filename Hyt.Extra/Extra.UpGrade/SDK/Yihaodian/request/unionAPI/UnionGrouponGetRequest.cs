using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询网盟团购信息
	/// </summary>
	public class UnionGrouponGetRequest 
		: IYhdRequest<UnionGrouponGetResponse> 
	{
		/**联盟用户id */
			public long?  TrackerU{ get; set; }

		/**网站id，预留字段，暂时不使用 */
			public string  WebsiteId{ get; set; }

		/**用户id，预留字段，暂时不使用 */
			public long?  Uid{ get; set; }

		/**省份id */
			public long?  ProvinceId{ get; set; }

		/**模糊查询名称 */
			public string  Keyword{ get; set; }

		/**团购商品所属类目 */
			public long?  CategoryId{ get; set; }

		/**站点，1：1号店自营商品，2：1号店商城商品 */
			public int?  SiteType{ get; set; }

		/**折后价区间价格最小值 */
			public double?  DiscountPriceMin{ get; set; }

		/**折后价区间价格最大值 */
			public double?  DiscountPriceMax{ get; set; }

		/**店铺佣金比例查询最小值。单位：万分之（如：填20，代表0.02%） */
			public double?  CommissionRateMin{ get; set; }

		/**店铺佣金比例查询最大值。单位：万分之（如：填30，代表0.03%） */
			public double?  CommissionRateMax{ get; set; }

		/**排序字段。commision_rate：佣金率。groupon_id：团购ID；people_number：参团人数；discount_price：折后价格 */
			public string  Sort{ get; set; }

		/**排序方式。desc：降序排列；asc：升序排列 */
			public string  SortType{ get; set; }

		/**页码。结果页1-99（默认为1） */
			public int?  CurPage{ get; set; }

		/**每页最大条数（每页显示记录数，默认50，最大50） */
			public int?  PageRows{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.groupon.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("trackerU", this.TrackerU);
			parameters.Add("websiteId", this.WebsiteId);
			parameters.Add("uid", this.Uid);
			parameters.Add("provinceId", this.ProvinceId);
			parameters.Add("keyword", this.Keyword);
			parameters.Add("categoryId", this.CategoryId);
			parameters.Add("siteType", this.SiteType);
			parameters.Add("discountPriceMin", this.DiscountPriceMin);
			parameters.Add("discountPriceMax", this.DiscountPriceMax);
			parameters.Add("commissionRateMin", this.CommissionRateMin);
			parameters.Add("commissionRateMax", this.CommissionRateMax);
			parameters.Add("sort", this.Sort);
			parameters.Add("sortType", this.SortType);
			parameters.Add("curPage", this.CurPage);
			parameters.Add("pageRows", this.PageRows);
			return parameters;
		}
		#endregion
	}
}
