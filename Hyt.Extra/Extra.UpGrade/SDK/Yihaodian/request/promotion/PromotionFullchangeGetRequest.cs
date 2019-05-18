using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查找满就换购列表促销
	/// </summary>
	public class PromotionFullchangeGetRequest 
		: IYhdRequest<PromotionFullchangeGetResponse> 
	{
		/**促销开始时间，格式如示例，开始时间 */
			public string  StartDate{ get; set; }

		/**状态值  -1:所有 0:已取消 1:尚未生效 2:生效中 3:已过期  */
			public int?  Status{ get; set; }

		/**分页查询第几页 */
			public int?  CurPage{ get; set; }

		/**分页查询每页的个数 */
			public int?  PageRows{ get; set; }

		/**查询的产品类别id(产品名称、id和类别，筛选的都是按商品添加的促销) */
			public long?  CategoryId{ get; set; }

		/**查询的产品名称，模糊查询   */
			public string  ProductCname{ get; set; }

		/**促销结束时间，格式如示例 */
			public string  EndDate{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.promotion.fullchange.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("startDate", this.StartDate);
			parameters.Add("status", this.Status);
			parameters.Add("curPage", this.CurPage);
			parameters.Add("pageRows", this.PageRows);
			parameters.Add("categoryId", this.CategoryId);
			parameters.Add("productCname", this.ProductCname);
			parameters.Add("endDate", this.EndDate);
			return parameters;
		}
		#endregion
	}
}
