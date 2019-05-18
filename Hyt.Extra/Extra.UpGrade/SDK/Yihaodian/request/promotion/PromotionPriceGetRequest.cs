using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询价格促销列表
	/// </summary>
	public class PromotionPriceGetRequest 
		: IYhdRequest<PromotionPriceGetResponse> 
	{
		/**查询的起始时间 */
			public string  StartDate{ get; set; }

		/**查询的结束时间 */
			public string  EndDate{ get; set; }

		/**1：未生效  2：生效中   3：未生效+生效中 */
			public int?  Status{ get; set; }

		/**分页查询每页的个数 */
			public int?  PageRows{ get; set; }

		/**分页查询第几页 */
			public int?  CurPage{ get; set; }

		/**查询的产品id,只能查普通产品和虚品。 */
			public long?  ProductId{ get; set; }

		/**查询的产品名称，模糊查询 */
			public string  ProductCname{ get; set; }

		/**查询的类别id */
			public long?  CategoryId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.promotion.price.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("startDate", this.StartDate);
			parameters.Add("endDate", this.EndDate);
			parameters.Add("status", this.Status);
			parameters.Add("pageRows", this.PageRows);
			parameters.Add("curPage", this.CurPage);
			parameters.Add("productId", this.ProductId);
			parameters.Add("productCname", this.ProductCname);
			parameters.Add("categoryId", this.CategoryId);
			return parameters;
		}
		#endregion
	}
}
