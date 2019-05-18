using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 同意取消订单
	/// </summary>
	public class OrderCancelApproveRequest 
		: IYhdRequest<OrderCancelApproveResponse> 
	{
		/**订单号(订单编码) */
			public string  OrderCode{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.order.cancel.approve";
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
