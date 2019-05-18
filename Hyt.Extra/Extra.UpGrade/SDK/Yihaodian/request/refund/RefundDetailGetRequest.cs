using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取单个退货详情
	/// </summary>
	public class RefundDetailGetRequest 
		: IYhdRequest<RefundDetailGetResponse> 
	{
		/**退货单号 */
			public string  RefundCode{ get; set; }

		/**退货Id，refundCode和refundId必须填写一个，如果都传入，取refundCode查询 */
			public long?  RefundId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.refund.detail.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("refundCode", this.RefundCode);
			parameters.Add("refundId", this.RefundId);
			return parameters;
		}
		#endregion
	}
}
