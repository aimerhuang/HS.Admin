using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询申请取消订单信息
	/// </summary>
	public class OrderCancelGetRequest 
		: IYhdRequest<OrderCancelGetResponse> 
	{
		/**申请取消开始时间 */
			public string  OrderCancelApplyTimeStart{ get; set; }

		/**申请取消结束时间 */
			public string  OrderCancelApplyTimeEnd{ get; set; }

		/**处理状态 */
			public int?  CancelStatus{ get; set; }

		/**订单编码 */
			public string  OrderCode{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.order.cancel.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("orderCancelApplyTimeStart", this.OrderCancelApplyTimeStart);
			parameters.Add("orderCancelApplyTimeEnd", this.OrderCancelApplyTimeEnd);
			parameters.Add("cancelStatus", this.CancelStatus);
			parameters.Add("orderCode", this.OrderCode);
			return parameters;
		}
		#endregion
	}
}
