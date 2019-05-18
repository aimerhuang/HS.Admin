using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// JD错误日志回调插入接口
	/// </summary>
	public class FinanceCallbackErrorlogAddRequest 
		: IYhdRequest<FinanceCallbackErrorlogAddResponse> 
	{
		/**回写信息
此处为申请1号店申请接口时传入的uuid */
			public string  Uuid{ get; set; }

		/**订单号 */
			public string  InvoiceOrderCode{ get; set; }

		/**错误信息 */
			public string  ErrorMsg{ get; set; }

		/**错误类型 */
			public string  ErrorType{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.finance.callback.errorlog.add";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("uuid", this.Uuid);
			parameters.Add("invoiceOrderCode", this.InvoiceOrderCode);
			parameters.Add("errorMsg", this.ErrorMsg);
			parameters.Add("errorType", this.ErrorType);
			return parameters;
		}
		#endregion
	}
}
