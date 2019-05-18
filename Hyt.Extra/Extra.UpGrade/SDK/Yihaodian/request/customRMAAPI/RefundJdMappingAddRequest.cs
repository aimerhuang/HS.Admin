using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// JD服务回传映射
	/// </summary>
	public class RefundJdMappingAddRequest 
		: IYhdRequest<RefundJdMappingAddResponse> 
	{
		/**YHD退换货ID */
			public int?  ApplyIdYhd{ get; set; }

		/**JD服务单ID */
			public int?  AfsServiceId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.refund.jd.mapping.add";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("applyIdYhd", this.ApplyIdYhd);
			parameters.Add("afsServiceId", this.AfsServiceId);
			return parameters;
		}
		#endregion
	}
}
