using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询集团商家下区域商家信息
	/// </summary>
	public class GroupmerchantAreamerchantGetRequest 
		: IYhdRequest<GroupmerchantAreamerchantGetResponse> 
	{

		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.groupmerchant.areamerchant.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			return parameters;
		}
		#endregion
	}
}
