using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 物流流转信息查询（兼容淘宝）
	/// </summary>
	public class LogisticsTraceSearchRequest 
		: IYhdRequest<LogisticsTraceSearchResponse> 
	{
		/**1号店交易号，请勿传非1号店交易号 */
			public long?  Tid{ get; set; }

		/**卖家昵称(暂不提供) */
			public string  SellerNick{ get; set; }

		/**表明是否是拆单，默认值0，1表示拆单(暂不提供) */
			public int?  IsSplit{ get; set; }

		/**拆单子订单列表，对应的数据是：子订单号的列表。可以不传，但是如果传了则必须符合传递的规则。子订单必须是操作的物流订单的子订单的真子集(暂不提供) */
			public string  SubTid{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.logistics.trace.search";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("tid", this.Tid);
			parameters.Add("sellerNick", this.SellerNick);
			parameters.Add("isSplit", this.IsSplit);
			parameters.Add("subTid", this.SubTid);
			return parameters;
		}
		#endregion
	}
}
