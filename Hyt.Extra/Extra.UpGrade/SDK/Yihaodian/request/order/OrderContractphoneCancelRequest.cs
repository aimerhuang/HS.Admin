using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 取消合约机订单
	/// </summary>
	public class OrderContractphoneCancelRequest 
		: IYhdRequest<OrderContractphoneCancelResponse> 
	{
		/**订单号，多个订单好用“,”隔开 */
			public string  OrderCodes{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.order.contractphone.cancel";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("orderCodes", this.OrderCodes);
			return parameters;
		}
		#endregion
	}
}
