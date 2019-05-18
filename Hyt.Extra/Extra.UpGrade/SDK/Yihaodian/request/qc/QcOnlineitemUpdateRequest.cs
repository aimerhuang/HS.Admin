using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 反馈确认质检资质表单
	/// </summary>
	public class QcOnlineitemUpdateRequest 
		: IYhdRequest<QcOnlineitemUpdateResponse> 
	{
		/**质检订单ID */
			public long?  CustomOrderId{ get; set; }

		/**质检资质集合Json */
			public string  QualificationList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.qc.onlineitem.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("customOrderId", this.CustomOrderId);
			parameters.Add("qualificationList", this.QualificationList);
			return parameters;
		}
		#endregion
	}
}
