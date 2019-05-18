using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// JD售后客服审核
	/// </summary>
	public class RefundJdAuditUpdateRequest 
		: IYhdRequest<RefundJdAuditUpdateResponse> 
	{
		/**1号店退换货ID。 */
			public int?  ApplyIdYhd{ get; set; }

		/**审核结果。
1 上门取件 2 顾客寄回 3 拒绝 4 取消 5 直赔商品 6 直赔余额。 */
			public int?  AuditResult{ get; set; }

		/**审核备注信息。审核失败或取消时，必须填写。 */
			public string  Remark{ get; set; }

		/**联系人姓名。顾客回寄时必须填写。 */
			public string  ContactName{ get; set; }

		/**联系人电话。顾客回寄时必须填写。 */
			public string  ContactPhone{ get; set; }

		/**联系人地址。顾客回寄时必须填写。 */
			public string  SendBackAddress{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.refund.jd.audit.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("applyIdYhd", this.ApplyIdYhd);
			parameters.Add("auditResult", this.AuditResult);
			parameters.Add("remark", this.Remark);
			parameters.Add("contactName", this.ContactName);
			parameters.Add("contactPhone", this.ContactPhone);
			parameters.Add("sendBackAddress", this.SendBackAddress);
			return parameters;
		}
		#endregion
	}
}
