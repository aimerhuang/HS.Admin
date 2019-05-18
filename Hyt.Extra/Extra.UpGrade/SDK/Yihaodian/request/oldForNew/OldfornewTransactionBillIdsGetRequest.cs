using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量获取用户提交到1号店交易单ids
	/// </summary>
	public class OldfornewTransactionBillIdsGetRequest 
		: IYhdRequest<OldfornewTransactionBillIdsGetResponse> 
	{
		/**合作商id */
			public long?  PartnerBusinessId{ get; set; }

		/**交易单状态集合 */
			public string  BillStatusList{ get; set; }

		/**签名 */
			public string  SignPassport{ get; set; }

		/**更新时间开始 */
			public string  UpdateStartTime{ get; set; }

		/**更新时间结束 */
			public string  UpdateEndTime{ get; set; }

		/**页码 */
			public int?  PageNo{ get; set; }

		/**每页条数 */
			public int?  PageSize{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.oldfornew.transaction.bill.ids.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("partnerBusinessId", this.PartnerBusinessId);
			parameters.Add("billStatusList", this.BillStatusList);
			parameters.Add("signPassport", this.SignPassport);
			parameters.Add("updateStartTime", this.UpdateStartTime);
			parameters.Add("updateEndTime", this.UpdateEndTime);
			parameters.Add("pageNo", this.PageNo);
			parameters.Add("pageSize", this.PageSize);
			return parameters;
		}
		#endregion
	}
}
