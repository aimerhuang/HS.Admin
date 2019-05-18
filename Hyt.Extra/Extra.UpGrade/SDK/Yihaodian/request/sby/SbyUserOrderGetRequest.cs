using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询用户历史app订单
	/// </summary>
	public class SbyUserOrderGetRequest 
		: IYhdRequest<SbyUserOrderGetResponse> 
	{
		/**sby用户id */
			public int?  SbyUserId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.sby.user.order.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("sbyUserId", this.SbyUserId);
			return parameters;
		}
		#endregion
	}
}
