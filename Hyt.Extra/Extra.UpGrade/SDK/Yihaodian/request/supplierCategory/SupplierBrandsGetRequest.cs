using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量获取新品品牌列表
	/// </summary>
	public class SupplierBrandsGetRequest 
		: IYhdRequest<SupplierBrandsGetResponse> 
	{

		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.supplier.brands.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			return parameters;
		}
		#endregion
	}
}
