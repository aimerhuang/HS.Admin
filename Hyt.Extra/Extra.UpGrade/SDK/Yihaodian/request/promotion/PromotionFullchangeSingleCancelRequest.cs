using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 取消单个满就换购详情促销
	/// </summary>
	public class PromotionFullchangeSingleCancelRequest 
		: IYhdRequest<PromotionFullchangeSingleCancelResponse> 
	{
		/**促销的id */
			public long?  CancelId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.promotion.fullchange.single.cancel";
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
