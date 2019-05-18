using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取质检资质表单
	/// </summary>
	public class QcOnlineitemQueryRequest 
		: IYhdRequest<QcOnlineitemQueryResponse> 
	{
		/**质检订单ID */
			public long?  CustomOrderId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.qc.onlineitem.query";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("customOrderId", this.CustomOrderId);
			return parameters;
		}
		#endregion
	}
}
