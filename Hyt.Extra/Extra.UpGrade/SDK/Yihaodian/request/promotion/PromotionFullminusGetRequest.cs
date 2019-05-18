using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询满减促销列表
	/// </summary>
	public class PromotionFullminusGetRequest 
		: IYhdRequest<PromotionFullminusGetResponse> 
	{
		/**查询的起始时间 */
			public string  StartDate{ get; set; }

		/**查询的结束时间,起始时间必须在结束时间之前 */
			public string  EndDate{ get; set; }

		/**状态值  -1:所有 0:已取消 1:尚未生效 2:生效中 3:已过期  */
			public int?  Status{ get; set; }

		/**分页查询每页的个数（默认50.最大100） */
			public int?  PageRows{ get; set; }

		/**分页查询第几页 */
			public int?  CurPage{ get; set; }

		/**查询的产品id */
			public string  ProductId{ get; set; }

		/**查询的产品名称，模糊查询 */
			public string  ProductCname{ get; set; }

		/**查询的产品类别id(产品名称、id和类别，筛选的都是按商品添加的促销) */
			public long?  CategoryId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.promotion.fullminus.get";
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
