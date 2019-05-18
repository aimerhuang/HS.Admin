using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 取消价格促销
	/// </summary>
	public class PromotionPriceCancelRequest 
		: IYhdRequest<PromotionPriceCancelResponse> 
	{
		/**取消的价格促销id。系列产品的只能取消全部，不能取消单个子品 */
			public long?  CancelId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.promotion.price.cancel";
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
