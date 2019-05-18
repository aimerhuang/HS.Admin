using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 无需物流（虚拟）发货处理（兼容淘宝）
	/// </summary>
	public class LogisticsDummySendRequest 
		: IYhdRequest<LogisticsDummySendResponse> 
	{
		/**交易ID  */
			public long?  Tid{ get; set; }

		/**feature参数格式 范例: identCode=tid1:识别码1,识别码2|tid2:识别码3;machineCode=tid3:3C机器号A,3C机器号B identCode为识别码的KEY,machineCode为3C的KEY,多个key之间用”;”分隔(暂不提供) */
			public string  Feature{ get; set; }

		/**商家的IP地址 （暂不提供） */
			public string  SellerIp { get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.logistics.dummy.send";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("tid", this.Tid);
			parameters.Add("feature", this.Feature);
			parameters.Add("sellerIp ", this.SellerIp );
			return parameters;
		}
		#endregion
	}
}
