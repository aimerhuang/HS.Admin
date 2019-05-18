using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 更新交易单信息
	/// </summary>
	public class OldfornewTransactionBillUpdateRequest 
		: IYhdRequest<OldfornewTransactionBillUpdateResponse> 
	{
		/**交易单id */
			public long?  TransactionBillId{ get; set; }

		/**当前状态 */
			public int?  CurrentBillStatus{ get; set; }

		/**变更状态 */
			public int?  ChangeBillStatus{ get; set; }

		/**检测单结构 */
			public string  InspectionBillRequest{ get; set; }

		/**交易单备注信息 */
			public string  Remark{ get; set; }

		/**签名 */
			public string  SignPassport{ get; set; }

		/**合作商id */
			public long?  PartnerBusinessId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.oldfornew.transaction.bill.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("transactionBillId", this.TransactionBillId);
			parameters.Add("currentBillStatus", this.CurrentBillStatus);
			parameters.Add("changeBillStatus", this.ChangeBillStatus);
			parameters.Add("inspectionBillRequest", this.InspectionBillRequest);
			parameters.Add("remark", this.Remark);
			parameters.Add("signPassport", this.SignPassport);
			parameters.Add("partnerBusinessId", this.PartnerBusinessId);
			return parameters;
		}
		#endregion
	}
}
