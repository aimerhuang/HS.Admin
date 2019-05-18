using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量获取订单详情
	/// </summary>
	public class OrdersDetailGetRequest 
		: IYhdRequest<OrdersDetailGetResponse> 
	{
		/**订单编码列表（逗号分隔）,最大长度为50    */
			public string  OrderCodeList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.orders.detail.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("orderCodeList", this.OrderCodeList);
			return parameters;
		}
		#endregion
	}
}
