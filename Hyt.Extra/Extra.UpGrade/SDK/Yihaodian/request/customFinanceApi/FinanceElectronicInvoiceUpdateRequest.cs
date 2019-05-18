using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// JD回传从航信获得的电子发票信息
	/// </summary>
	public class FinanceElectronicInvoiceUpdateRequest 
		: IYhdRequest<FinanceElectronicInvoiceUpdateResponse> 
	{
		/**回写信息
此处为申请1号店申请接口时传入的uuid */
			public string  InvoiceCallbackInfo{ get; set; }

		/**订单号 */
			public string  InvoiceOrderCode{ get; set; }

		/**发票编码 */
			public string  InvoiceCode{ get; set; }

		/**发票号码 */
			public string  InvoiceNumber{ get; set; }

		/**Base64转码后pdf文件 */
			public string  InvoicePdfFile{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.finance.electronicInvoice.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("invoiceCallbackInfo", this.InvoiceCallbackInfo);
			parameters.Add("invoiceOrderCode", this.InvoiceOrderCode);
			parameters.Add("invoiceCode", this.InvoiceCode);
			parameters.Add("invoiceNumber", this.InvoiceNumber);
			parameters.Add("invoicePdfFile", this.InvoicePdfFile);
			return parameters;
		}
		#endregion
	}
}
