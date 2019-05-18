using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// JD售后处理通知
	/// </summary>
	public class RefundJdAuditOperateNoticeRequest 
		: IYhdRequest<RefundJdAuditOperateNoticeResponse> 
	{
		/**1号店退换货ID。 */
			public int?  ApplyIdYhd{ get; set; }

		/**处理结果或换新结果。1.换新2.原反3.退款4.关单 41.换新成功 42.换新失败 */
			public int?  OperateType{ get; set; }

		/**处理意见或备注 */
			public string  Remark{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.refund.jd.audit.operate.notice";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("applyIdYhd", this.ApplyIdYhd);
			parameters.Add("operateType", this.OperateType);
			parameters.Add("remark", this.Remark);
			return parameters;
		}
		#endregion
	}
}
