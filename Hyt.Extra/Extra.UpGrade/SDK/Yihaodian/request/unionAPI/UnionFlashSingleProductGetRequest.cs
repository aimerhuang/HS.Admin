using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询名品商品详情信息
	/// </summary>
	public class UnionFlashSingleProductGetRequest 
		: IYhdRequest<UnionFlashSingleProductGetResponse> 
	{
		/**联盟用户ID */
			public long?  TrackerU{ get; set; }

		/**省份id */
			public long?  ProvinceId{ get; set; }

		/**卖场id */
			public long?  ActivityId{ get; set; }

		/**产品id */
			public long?  ProductId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.flash.single.product.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("trackerU", this.TrackerU);
			parameters.Add("provinceId", this.ProvinceId);
			parameters.Add("activityId", this.ActivityId);
			parameters.Add("productId", this.ProductId);
			return parameters;
		}
		#endregion
	}
}
