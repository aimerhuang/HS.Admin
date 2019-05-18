using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 退款确认
	/// </summary>
	public class RefundConfirmRequest 
		: IYhdRequest<RefundConfirmResponse> 
	{
		/**退货单编码 */
			public string  RefundCode{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.refund.confirm";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("refundCode", this.RefundCode);
			return parameters;
		}
		#endregion
	}
}
