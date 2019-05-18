using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询发票信息
	/// </summary>
	public class InvoicesGetRequest 
		: IYhdRequest<InvoicesGetResponse> 
	{
		/**订单编码列表（逗号分隔）,最大长度为100 */
			public string  OrderCodeList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.invoices.get";
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
