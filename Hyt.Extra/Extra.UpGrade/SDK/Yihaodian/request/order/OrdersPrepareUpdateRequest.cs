using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// O2O订单备货
	/// </summary>
	public class OrdersPrepareUpdateRequest 
		: IYhdRequest<OrdersPrepareUpdateResponse> 
	{
		/**订单编码列表（逗号分隔）,最大长度为20 */
			public string  OrderCodeList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.orders.prepare.update";
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
