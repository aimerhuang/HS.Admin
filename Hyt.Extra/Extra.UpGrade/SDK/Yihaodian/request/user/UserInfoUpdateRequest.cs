using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 用户信息回传
	/// </summary>
	public class UserInfoUpdateRequest 
		: IYhdRequest<UserInfoUpdateResponse> 
	{
		/**网站指定Id(10:爱彩网) */
			public int?  SiteId{ get; set; }

		/**Md5字串，由siteId+code+sessionId加密而成，code是为网站分配的密钥 */
			public string  Md5key{ get; set; }

		/**1号店用户的唯一标识 */
			public string  SessionId{ get; set; }

		/**用户真实姓名 */
			public string  RealUserName{ get; set; }

		/**用户身份证号 */
			public string  IdCard{ get; set; }

		/**电话号码 */
			public string  Mobile{ get; set; }

		/**邮箱 */
			public string  Email{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.user.info.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("siteId", this.SiteId);
			parameters.Add("md5key", this.Md5key);
			parameters.Add("sessionId", this.SessionId);
			parameters.Add("realUserName", this.RealUserName);
			parameters.Add("idCard", this.IdCard);
			parameters.Add("mobile", this.Mobile);
			parameters.Add("email", this.Email);
			return parameters;
		}
		#endregion
	}
}
