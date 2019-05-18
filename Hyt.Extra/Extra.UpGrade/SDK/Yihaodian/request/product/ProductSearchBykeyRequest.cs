using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 根据关键字或类目ID查询商品
	/// </summary>
	public class ProductSearchBykeyRequest 
		: IYhdRequest<ProductSearchBykeyResponse> 
	{
		/**商品标题中包含的关键字. 注意:查询时keyword,cid至少选择其中一个参数 */
			public string  Keyword{ get; set; }

		/**标准商品后台类目id。该ID可以通过yhd.itemcats.get接口获取到。 注意:keyword,cid至少选择其中一个参数 */
			public long?  Cid{ get; set; }

		/**商品所在地，省、直辖市名称 */
			public string  Area{ get; set; }

		/**结果当前页数 */
			public long?  PageNo{ get; set; }

		/**每页返回结果数 */
			public long?  PageSize{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.product.search.bykey";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("keyword", this.Keyword);
			parameters.Add("cid", this.Cid);
			parameters.Add("area", this.Area);
			parameters.Add("pageNo", this.PageNo);
			parameters.Add("pageSize", this.PageSize);
			return parameters;
		}
		#endregion
	}
}
