using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 此接口用来查询商家指定日期当日账单明细，查询指定页数，一页1000笔
	/// </summary>
	public class SettleBillDetailGetRequest 
		: IYhdRequest<SettleBillDetailGetResponse> 
	{
		/**账单日期 */
			public string  Date{ get; set; }

		/**查询页数 */
			public int?  PageNum{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.settle.bill.detail.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("date", this.Date);
			parameters.Add("pageNum", this.PageNum);
			return parameters;
		}
		#endregion
	}
}
