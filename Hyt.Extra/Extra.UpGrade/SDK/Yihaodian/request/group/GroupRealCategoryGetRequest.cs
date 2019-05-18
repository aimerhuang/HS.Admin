using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 团购类目信息查询
	/// </summary>
	public class GroupRealCategoryGetRequest 
		: IYhdRequest<GroupRealCategoryGetResponse> 
	{
		/**0：商品团；1：生活团 */
			public long?  GrouponType{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.group.real.category.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("grouponType", this.GrouponType);
			return parameters;
		}
		#endregion
	}
}
