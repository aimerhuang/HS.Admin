using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取单笔交易的部分信息(性能高)（兼容淘宝）
	/// </summary>
	public class TradeGetRequest 
		: IYhdRequest<TradeGetResponse> 
	{
		/**Trade中可以指定返回的fields。 */
			public string  Fields{ get; set; }

		/**交易编号  */
			public long?  Tid{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.trade.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("fields", this.Fields);
			parameters.Add("tid", this.Tid);
			return parameters;
		}
		#endregion
	}
}
