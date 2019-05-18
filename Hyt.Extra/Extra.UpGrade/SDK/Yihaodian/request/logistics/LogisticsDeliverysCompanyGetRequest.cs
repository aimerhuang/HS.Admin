using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取1mall合作物流公司信息
	/// </summary>
	public class LogisticsDeliverysCompanyGetRequest 
		: IYhdRequest<LogisticsDeliverysCompanyGetResponse> 
	{

		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.logistics.deliverys.company.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			return parameters;
		}
		#endregion
	}
}
