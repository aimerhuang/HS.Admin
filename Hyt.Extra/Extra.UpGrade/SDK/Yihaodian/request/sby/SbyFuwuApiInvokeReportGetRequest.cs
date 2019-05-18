using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// ISV流量报表
	/// </summary>
	public class SbyFuwuApiInvokeReportGetRequest 
		: IYhdRequest<SbyFuwuApiInvokeReportGetResponse> 
	{
		/**页码。默认值1 */
			public int?  PageNo{ get; set; }

		/**每页获取条数；取值范围：大于0的整数；最大限制100；默认值50 */
			public int?  PageSize{ get; set; }

		/**按月份查询,与yearMonthDay互斥,此参数输入，不必再输入yearMonthDay */
			public string  YearMonth{ get; set; }

		/**按日期查询,与yearMonth互斥,此参数输入，不必再输入yearMonth */
			public string  YearMonthDay{ get; set; }

		/**商家id */
			public long?  MerchantId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.sby.fuwu.api.invoke.report.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("pageNo", this.PageNo);
			parameters.Add("pageSize", this.PageSize);
			parameters.Add("yearMonth", this.YearMonth);
			parameters.Add("yearMonthDay", this.YearMonthDay);
			parameters.Add("merchantId", this.MerchantId);
			return parameters;
		}
		#endregion
	}
}
