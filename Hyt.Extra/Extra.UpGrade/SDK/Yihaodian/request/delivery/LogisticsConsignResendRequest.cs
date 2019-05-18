using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 修改物流公司和运单号（兼容淘宝）
	/// </summary>
	public class LogisticsConsignResendRequest 
		: IYhdRequest<LogisticsConsignResendResponse> 
	{
		/**交易ID  */
			public long?  Tid{ get; set; }

		/**子订单号的列表(暂不支持) */
			public long?  SubTid{ get; set; }

		/**表明是否是拆单，默认值0，1表示拆单（暂不支持） */
			public long?  IsSplit{ get; set; }

		/**运单号.具体一个物流公司的真实运单号码。 */
			public string  OutSid{ get; set; }

		/**物流公司ID */
			public string  CompanyCode{ get; set; }

		/**feature参数格式(暂不提供) */
			public string  Feature{ get; set; }

		/**商家的IP地址 (暂不提供) */
			public string  SellerIp { get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.logistics.consign.resend";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("tid", this.Tid);
			parameters.Add("subTid", this.SubTid);
			parameters.Add("isSplit", this.IsSplit);
			parameters.Add("outSid", this.OutSid);
			parameters.Add("companyCode", this.CompanyCode);
			parameters.Add("feature", this.Feature);
			parameters.Add("sellerIp ", this.SellerIp );
			return parameters;
		}
		#endregion
	}
}
