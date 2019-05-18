using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 自己联系物流（线下物流）发货（兼容淘宝）
	/// </summary>
	public class LogisticsOfflineSendRequest 
		: IYhdRequest<LogisticsOfflineSendResponse> 
	{
		/**卖家联系人地址库ID（暂不提供） */
			public long?  CancelId{ get; set; }

		/**物流公司编码 */
			public string  CompanyCode{ get; set; }

		/**feature参数格式<br> 范例: identCode=tid1:识别码1,识别码2|tid2:识别码3;machineCode=tid3:3C机器号A,3C机器号B<br> identCode为识别码的KEY,machineCode为3C的KEY,多个key之间用”;”分隔<br> “tid1:识别码1,识别码2|tid2:识别码3”为identCode对应的value。 "|"不同商品间的分隔符。<br>（暂不提供） */
			public string  Feature{ get; set; }

		/**表明是否是拆单 1表示拆单 0表示不拆单，默认值0（暂不提供） */
			public long?  IsSplit{ get; set; }

		/**运单号 */
			public string  OutSid{ get; set; }

		/**商家的IP地址（暂不提供） */
			public string  SellerIp{ get; set; }

		/**卖家联系人地址库ID（暂不提供） */
			public long?  SenderId{ get; set; }

		/**需要拆单发货的子订单集合，为空表示不做拆单发货（暂不提供） */
			public string  SubTid{ get; set; }

		/**交易ID */
			public long?  Tid{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.logistics.offline.send";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("cancelId", this.CancelId);
			parameters.Add("companyCode", this.CompanyCode);
			parameters.Add("feature", this.Feature);
			parameters.Add("isSplit", this.IsSplit);
			parameters.Add("outSid", this.OutSid);
			parameters.Add("sellerIp", this.SellerIp);
			parameters.Add("senderId", this.SenderId);
			parameters.Add("subTid", this.SubTid);
			parameters.Add("tid", this.Tid);
			return parameters;
		}
		#endregion
	}
}
