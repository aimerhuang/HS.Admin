using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 修改订单邮费价格（兼容淘宝） 
	/// </summary>
	public class TradePostageUpdateRequest 
		: IYhdRequest<TradePostageUpdateResponse> 
	{
		/**主订单编号 */
			public long?  Tid{ get; set; }

		/**邮费价格(邮费单位是元） */
			public string  PostFee{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.trade.postage.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("tid", this.Tid);
			parameters.Add("postFee", this.PostFee);
			return parameters;
		}
		#endregion
	}
}
