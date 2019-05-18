using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询用户购买的app
	/// </summary>
	public class SbyUserAppGetRequest 
		: IYhdRequest<SbyUserAppGetResponse> 
	{
		/**sby用户id[此参数后面会删除，不要使用] */
			public int?  SbyUserId{ get; set; }

		/**用户类型对应的id(userType=1,传商家id；userType=3,传供应商id) */
			public long?  Id{ get; set; }

		/**用户类型[1:商家 3:供应商 4:服务商] */
			public int?  UserType{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.sby.user.app.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("sbyUserId", this.SbyUserId);
			parameters.Add("id", this.Id);
			parameters.Add("userType", this.UserType);
			return parameters;
		}
		#endregion
	}
}
