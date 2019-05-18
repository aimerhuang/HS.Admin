using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 更新订单运费
	/// </summary>
	public class OrderDeliveryfeeUpdateRequest 
		: IYhdRequest<OrderDeliveryfeeUpdateResponse> 
	{
		/**订单编码。只有待支付状态（ORDER_WAIT_PAY）的订单，才可以修改运费 */
			public string  OrderCode{ get; set; }

		/**运费。修改的运费值要小于原有运费值。 */
			public double?  OrderDeliveryFee{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.order.deliveryfee.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("orderCode", this.OrderCode);
			parameters.Add("orderDeliveryFee", this.OrderDeliveryFee);
			return parameters;
		}
		#endregion
	}
}
