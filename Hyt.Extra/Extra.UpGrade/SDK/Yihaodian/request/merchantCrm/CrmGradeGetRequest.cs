using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 商家店铺的会员等级规则查询
	/// </summary>
	public class CrmGradeGetRequest 
		: IYhdRequest<CrmGradeGetResponse> 
	{

		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.crm.grade.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			return parameters;
		}
		#endregion
	}
}
