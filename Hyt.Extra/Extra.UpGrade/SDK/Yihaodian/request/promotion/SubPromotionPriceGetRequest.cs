using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询子品促销信息
	/// </summary>
	public class SubPromotionPriceGetRequest 
		: IYhdRequest<SubPromotionPriceGetResponse> 
	{
		/**系列产品促销id */
			public long?  Id{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.sub.promotion.price.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("id", this.Id);
			return parameters;
		}
		#endregion
	}
}
