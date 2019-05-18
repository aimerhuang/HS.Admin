using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 资质审核状态更新
	/// </summary>
	public class QcStatusUpdateRequest 
		: IYhdRequest<QcStatusUpdateResponse> 
	{
		/**商家订单ID */
			public long?  CustomOrderId{ get; set; }

		/**用户名称。用户注册帐号名称 */
			public string  UserName{ get; set; }

		/**审核状态1：审核申请已受理，请保持电话畅通，咨询请致电027-59397322、2：请根据认证邮件的内容，提交审核所需的相关资料、3：资料已提交、资质审核中，请耐心等候、4：资料有缺失，请根据反馈邮件提示尽快提交、注：状态1为派单成功后双方默认状态。状态2、4我们可以添加备注，认证员填写自己的直线电话所以去掉了电话号码。 */
			public int?  Status{ get; set; }

		/**状态备注。最大1024个字符 */
			public string  StatusRemark{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.qc.status.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("customOrderId", this.CustomOrderId);
			parameters.Add("userName", this.UserName);
			parameters.Add("status", this.Status);
			parameters.Add("statusRemark", this.StatusRemark);
			return parameters;
		}
		#endregion
	}
}
