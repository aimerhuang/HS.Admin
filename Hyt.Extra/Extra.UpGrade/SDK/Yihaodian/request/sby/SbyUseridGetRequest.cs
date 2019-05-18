using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 根据用户类别和ID获取SBY用户ID
	/// </summary>
	public class SbyUseridGetRequest 
		: IYhdRequest<SbyUseridGetResponse> 
	{
		/**用户类型对应的id */
			public long?  Id{ get; set; }

		/**用户类型 1:商家 2：内部用户 3:供应商 */
			public int?  UserType{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.sby.userid.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("id", this.Id);
			parameters.Add("userType", this.UserType);
			return parameters;
		}
		#endregion
	}
}
