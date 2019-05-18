using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 取消单个满就送赠品详情促销
	/// </summary>
	public class PromotionFullgiftSingleCancelRequest 
		: IYhdRequest<PromotionFullgiftSingleCancelResponse> 
	{
		/**促销的id */
			public long?  CancelId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.promotion.fullgift.single.cancel";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("cancelId", this.CancelId);
			return parameters;
		}
		#endregion
	}
}
