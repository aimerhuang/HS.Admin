using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取加密推广链接
	/// </summary>
	public class UnionClickUrlGetRequest 
		: IYhdRequest<UnionClickUrlGetResponse> 
	{
		/**跟踪码 */
			public long?  TrackerU{ get; set; }

		/**需要加密的落地页链接 */
			public string  LandingPageUrl{ get; set; }

		/**渠道子站ID */
			public string  WebsiteId{ get; set; }

		/**渠道用户ID */
			public string  Uid{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.clickUrl.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("trackerU", this.TrackerU);
			parameters.Add("landingPageUrl", this.LandingPageUrl);
			parameters.Add("websiteId", this.WebsiteId);
			parameters.Add("uid", this.Uid);
			return parameters;
		}
		#endregion
	}
}
