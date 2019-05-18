using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询订单详情
	/// </summary>
	public class SupplierOrderDetailGetRequest 
		: IYhdRequest<SupplierOrderDetailGetResponse> 
	{
		/**订单Id */
			public long?  OrderId{ get; set; }

		/**是否为历史订单。true：是；false：不是。 */
			public bool  IsHistory{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.supplier.order.detail.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("orderId", this.OrderId);
			parameters.Add("isHistory", this.IsHistory);
			return parameters;
		}
		#endregion
	}
}
