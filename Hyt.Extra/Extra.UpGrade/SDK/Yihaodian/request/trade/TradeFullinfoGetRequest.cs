using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取单笔交易的详细信息（兼容淘宝）
	/// </summary>
	public class TradeFullinfoGetRequest 
		: IYhdRequest<TradeFullinfoGetResponse> 
	{
		/**Trade中可以指定返回的fields。 */
			public string  Fields{ get; set; }

		/**交易编号  */
			public long?  Tid{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.trade.fullinfo.get";
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
