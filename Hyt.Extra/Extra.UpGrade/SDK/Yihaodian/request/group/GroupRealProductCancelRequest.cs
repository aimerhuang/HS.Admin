using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 商城产品团购取消接口
	/// </summary>
	public class GroupRealProductCancelRequest 
		: IYhdRequest<GroupRealProductCancelResponse> 
	{
		/**团购ID */
			public long?  GroupId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.group.real.product.cancel";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("groupId", this.GroupId);
			return parameters;
		}
		#endregion
	}
}
