using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取交易单详情
	/// </summary>
	public class OldfornewTransactionBillGetRequest 
		: IYhdRequest<OldfornewTransactionBillGetResponse> 
	{
		/**合作商id */
			public long?  PartnerBusinessId{ get; set; }

		/**签名 */
			public string  SignPassport{ get; set; }

		/**交易单id */
			public long?  TransactionBillId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.oldfornew.transaction.bill.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("partnerBusinessId", this.PartnerBusinessId);
			parameters.Add("signPassport", this.SignPassport);
			parameters.Add("transactionBillId", this.TransactionBillId);
			return parameters;
		}
		#endregion
	}
}
