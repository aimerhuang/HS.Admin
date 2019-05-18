using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 此接口用来查询商家指定日期当日账单明细总条数
	/// </summary>
	public class SettleBillCountGetRequest 
		: IYhdRequest<SettleBillCountGetResponse> 
	{
		/**账单日期 */
			public string  Date{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.settle.bill.count.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("date", this.Date);
			return parameters;
		}
		#endregion
	}
}
