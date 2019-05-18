using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 验证用户购买的服务是否可用
	/// </summary>
	public class SbyUserappCheckRequest 
		: IYhdRequest<SbyUserappCheckResponse> 
	{

		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.sby.userapp.check";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			return parameters;
		}
		#endregion
	}
}
