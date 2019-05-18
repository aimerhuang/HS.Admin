using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 拒绝退款
	/// </summary>
	public class RefundRejectRequest 
		: IYhdRequest<RefundRejectResponse> 
	{
		/**退换货编码 */
			public string  RefundCode{ get; set; }

		/**拒绝原因 */
			public string  Remark{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.refund.reject";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("refundCode", this.RefundCode);
			parameters.Add("remark", this.Remark);
			return parameters;
		}
		#endregion
	}
}
