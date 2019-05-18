using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询商品状态
	/// </summary>
	public class UnionFullProductsGetRequest 
		: IYhdRequest<UnionFullProductsGetResponse> 
	{
		/**联盟用户ID */
			public long?  TrackerU{ get; set; }

		/**产品Id列表（以逗号分割，最多40个,与productCodes二选一，不能同时为空） */
			public string  ProductIds{ get; set; }

		/**联盟编码列表（以逗号分割，最多40个，与productIds二选一，不能同时为空） */
			public string  ProductCodes{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.full.products.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("trackerU", this.TrackerU);
			parameters.Add("productIds", this.ProductIds);
			parameters.Add("productCodes", this.ProductCodes);
			return parameters;
		}
		#endregion
	}
}
