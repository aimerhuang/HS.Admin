using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询商家活动列表信息
	/// </summary>
	public class UnionBusinessActivityGetRequest 
		: IYhdRequest<UnionBusinessActivityGetResponse> 
	{
		/**联盟用户ID */
			public long?  TrackerU{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.business.activity.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("trackerU", this.TrackerU);
			return parameters;
		}
		#endregion
	}
}
