using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 用户登录认证
	/// </summary>
	public class UserAuthGetRequest 
		: IYhdRequest<UserAuthGetResponse> 
	{
		/**网站指定Id(10:爱彩网) */
			public int?  SiteId{ get; set; }

		/**Md5字串，由siteId+code+sessionId加密而成，code是为网站分配的密钥 */
			public string  Md5key{ get; set; }

		/**1号店用户的唯一标识 */
			public string  SessionId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.user.auth.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("siteId", this.SiteId);
			parameters.Add("md5key", this.Md5key);
			parameters.Add("sessionId", this.SessionId);
			return parameters;
		}
		#endregion
	}
}
