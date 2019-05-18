using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 根据订单号查询用户身份信息
	/// </summary>
	public class OrderUserinfoGetRequest 
		: IYhdRequest<OrderUserinfoGetResponse> 
	{
		/**订单号 */
			public string  OrderCode{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.order.userinfo.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("orderCode", this.OrderCode);
			return parameters;
		}
		#endregion
	}
}
