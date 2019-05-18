using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询单品接口商品信息
	/// </summary>
	public class UnionSingleProductGetRequest 
		: IYhdRequest<UnionSingleProductGetResponse> 
	{
		/**联盟用户ID */
			public long?  TrackerU{ get; set; }

		/**返回字段，可选 */
			public string  Fields{ get; set; }

		/**网站ID，预留字段，暂时不使用 */
			public string  WebsiteId{ get; set; }

		/**用户ID，预留字段 */
			public string  Uid{ get; set; }

		/**模糊查询名称，只能为一个关键字，匹配商品名称 */
			public string  KeyWord{ get; set; }

		/**商品所属的类目id */
			public long?  CategoryId{ get; set; }

		/**1：1号店自营商品，2：1号店商城商品，默认混合在一起。 */
			public int?  SiteType{ get; set; }

		/**查询起始价格，需注意start_price <= end_price  */
			public double?  StartPrice{ get; set; }

		/**查询结束价格 */
			public double?  EndPrice{ get; set; }

		/**商品佣金比例查询开始值，注意佣金比例是x10000的整数。50表示0.5% */
			public double?  StartCommissionRate{ get; set; }

		/**商品佣金比例查询结束值，注意佣金比例是x10000的整数。50表示0.5% */
			public double?  EndCommissionRate{ get; set; }

		/**商品质量得分查询开始值，质量得分越大用户成交几率越大 */
			public int?  StartProdQualityScore{ get; set; }

		/**质量得分查询结束值 */
			public int?  EndProdQualityScore{ get; set; }

		/**默认排序:default，质量得分，佣金比例、佣金 */
			public string  Sort{ get; set; }

		/**页码。结果页1-99（默认为1） */
			public int?  PageNo{ get; set; }

		/**每页最大条数（每页显示记录数，默认40，最大40） */
			public int?  PageSize{ get; set; }

		/**产品ID */
			public long?  ProductId{ get; set; }

		/**pminfoid */
			public long?  PmInfoId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.single.product.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("trackerU", this.TrackerU);
			parameters.Add("fields", this.Fields);
			parameters.Add("websiteId", this.WebsiteId);
			parameters.Add("uid", this.Uid);
			parameters.Add("keyWord", this.KeyWord);
			parameters.Add("categoryId", this.CategoryId);
			parameters.Add("siteType", this.SiteType);
			parameters.Add("startPrice", this.StartPrice);
			parameters.Add("endPrice", this.EndPrice);
			parameters.Add("startCommissionRate", this.StartCommissionRate);
			parameters.Add("endCommissionRate", this.EndCommissionRate);
			parameters.Add("startProdQualityScore", this.StartProdQualityScore);
			parameters.Add("endProdQualityScore", this.EndProdQualityScore);
			parameters.Add("sort", this.Sort);
			parameters.Add("pageNo", this.PageNo);
			parameters.Add("pageSize", this.PageSize);
			parameters.Add("productId", this.ProductId);
			parameters.Add("pmInfoId", this.PmInfoId);
			return parameters;
		}
		#endregion
	}
}
