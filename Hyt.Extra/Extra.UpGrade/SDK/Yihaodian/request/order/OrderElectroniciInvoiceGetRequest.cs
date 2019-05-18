using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 海尔电子发票接口
	/// </summary>
	public class OrderElectroniciInvoiceGetRequest 
		: IYhdRequest<OrderElectroniciInvoiceGetResponse> 
	{
		/**发票信息 */
			public string  InvoiceInfoList{ get; set; }

		/**订单code */
			public string  OrderCode{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.order.electronici.invoice.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("invoiceInfoList", this.InvoiceInfoList);
			parameters.Add("orderCode", this.OrderCode);
			return parameters;
		}
		#endregion
	}
}
