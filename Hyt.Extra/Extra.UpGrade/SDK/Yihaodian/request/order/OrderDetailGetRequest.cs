using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取单个订单详情
	/// </summary>
	public class OrderDetailGetRequest 
		: IYhdRequest<OrderDetailGetResponse> 
	{
		/**订单编码 */
			public string  OrderCode{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.order.detail.get";
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
