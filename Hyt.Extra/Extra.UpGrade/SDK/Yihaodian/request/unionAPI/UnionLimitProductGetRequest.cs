using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询限时购商品信息接口
	/// </summary>
	public class UnionLimitProductGetRequest 
		: IYhdRequest<UnionLimitProductGetResponse> 
	{
		/**联盟用户ID */
			public string  TrackerU{ get; set; }

		/**返回字段，可选 */
			public string  Fields{ get; set; }

		/**网站ID，预留字段，暂时不使用 */
			public string  WebsiteId{ get; set; }

		/**用户ID，预留字段 */
			public string  Uid{ get; set; }

		/**省份ID，当为空时，默认上海Id 23 */
			public long?  ProvinceId{ get; set; }

		/**产品ID，不填时查询全部 */
			public long?  ProductId{ get; set; }

		/**模糊匹配商品名称 */
			public string  Keyword{ get; set; }

		/**前台一级类目 */
			public long?  CategoryId{ get; set; }

		/**1：1号店自营商品，2：1号店商城商品，默认混合在一起。 */
			public int?  SiteType{ get; set; }

		/**排序字段只有空、抢购价（special_price）、市场价（market_price）、佣金率（commission_rate）和佣金（commission）五个值，默认按照佣金排序 */
			public string  Sort{ get; set; }

		/**排序方式，“asc”和“desc”，默认“desc” */
			public string  SortType{ get; set; }

		/**当前页，从1开始 */
			public int?  CurPage{ get; set; }

		/**每页数量，1--50之间，默认50 */
			public int?  PageNum{ get; set; }

		/**抢购开始时间，格式必须为：2014-08-27 */
			public string  ProdStartTime{ get; set; }

		/**抢购结束时间，格式必须为：2014-09-27 */
			public string  ProdEndTime{ get; set; }

		/**起始佣金率，x10000的值，例如：要查0.05，需要输入500 */
			public string  StartCommissionRate{ get; set; }

		/**截止佣金率，x10000的值，例如：要查0.05，需要输入500 */
			public string  EndCommissionRate{ get; set; }

		/**推荐原因文本格式   0.普通文本 1.富文本 ，默认0 */
			public int?  ContentType{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.limit.product.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("trackerU", this.TrackerU);
			parameters.Add("fields", this.Fields);
			parameters.Add("websiteId", this.WebsiteId);
			parameters.Add("uid", this.Uid);
			parameters.Add("provinceId", this.ProvinceId);
			parameters.Add("productId", this.ProductId);
			parameters.Add("keyword", this.Keyword);
			parameters.Add("categoryId", this.CategoryId);
			parameters.Add("siteType", this.SiteType);
			parameters.Add("sort", this.Sort);
			parameters.Add("sortType", this.SortType);
			parameters.Add("curPage", this.CurPage);
			parameters.Add("pageNum", this.PageNum);
			parameters.Add("prodStartTime", this.ProdStartTime);
			parameters.Add("prodEndTime", this.ProdEndTime);
			parameters.Add("startCommissionRate", this.StartCommissionRate);
			parameters.Add("endCommissionRate", this.EndCommissionRate);
			parameters.Add("contentType", this.ContentType);
			return parameters;
		}
		#endregion
	}
}
