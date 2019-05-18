using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 用户操作结果回写
	/// </summary>
	public class UserOperateUpdateRequest 
		: IYhdRequest<UserOperateUpdateResponse> 
	{
		/**网站指定Id(10:爱彩网) */
			public int?  SiteId{ get; set; }

		/**Md5字串，由siteId+code+sessionId加密而成，code是为网站分配的密钥 */
			public string  Md5key{ get; set; }

		/**1号店用户的唯一标识 */
			public string  SessionId{ get; set; }

		/**唯一标识 */
			public string  UniqueMark{ get; set; }

		/**请求类型。0：回写购彩结果1：回写冻结状态 */
			public int?  RequestType{ get; set; }

		/**操作结果。0：成功，1：失败。请求类型为0时，为必选参数；请求参数为1时，无需填写。 */
			public long?  Result{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.user.operate.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("siteId", this.SiteId);
			parameters.Add("md5key", this.Md5key);
			parameters.Add("sessionId", this.SessionId);
			parameters.Add("uniqueMark", this.UniqueMark);
			parameters.Add("requestType", this.RequestType);
			parameters.Add("result", this.Result);
			return parameters;
		}
		#endregion
	}
}
