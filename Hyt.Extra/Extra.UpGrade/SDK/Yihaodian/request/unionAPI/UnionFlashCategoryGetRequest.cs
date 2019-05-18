using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询名品类目信息
	/// </summary>
	public class UnionFlashCategoryGetRequest 
		: IYhdRequest<UnionFlashCategoryGetResponse> 
	{
		/**联盟用户ID */
			public long?  TrackerU{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.flash.category.get";
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
