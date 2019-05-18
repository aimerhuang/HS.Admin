using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量查询物流订单,返回详细信息(兼容淘宝)
	/// </summary>
	public class LogisticsOrdersDetailGetRequest 
		: IYhdRequest<LogisticsOrdersDetailGetResponse> 
	{
		/**需返回的字段列表.可选值:Shipping 物流数据结构中所有字段.fileds中可以指定返回以上任意一个或者多个字段,以","分隔.  */
			public string  Fields{ get; set; }

		/**交易ID.如果加入tid参数的话,不用传其他的参数,但是仅会返回一条物流订单信息.  */
			public long?  Tid{ get; set; }

		/**买家昵称（暂不提供） */
			public string  BuyerNick{ get; set; }

		/**物流状态.可查看数据结构 Shipping 中的status字段.  */
			public string  Status{ get; set; }

		/**卖家是否发货.可选值:yes(是),no(否).如:yes. （暂不提供） */
			public string  SellerConfirm{ get; set; }

		/**收货人姓名  */
			public string  ReceiverName{ get; set; }

		/**创建时间开始.格式:yyyy-MM-dd HH:mm:ss  */
			public string  StartCreated{ get; set; }

		/**创建时间结束.格式:yyyy-MM-dd HH:mm:ss  */
			public string  EndCreated{ get; set; }

		/**谁承担运费.可选值:buyer(买家),seller(卖家).如:buyer（暂不提供）  */
			public string  FreightPayer{ get; set; }

		/**物流方式.可选值:post(平邮),express(快递),ems(EMS).如:post （暂不提供） */
			public string  Type{ get; set; }

		/**页码.该字段没传 或 值<1 ,则默认page_no为1  */
			public long?  PageNo{ get; set; }

		/**每页条数.该字段没传 或 值<1 ，则默认page_size为40 支持最大值为：100  */
			public long?  PageSize{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.logistics.orders.detail.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("fields", this.Fields);
			parameters.Add("tid", this.Tid);
			parameters.Add("buyerNick", this.BuyerNick);
			parameters.Add("status", this.Status);
			parameters.Add("sellerConfirm", this.SellerConfirm);
			parameters.Add("receiverName", this.ReceiverName);
			parameters.Add("startCreated", this.StartCreated);
			parameters.Add("endCreated", this.EndCreated);
			parameters.Add("freightPayer", this.FreightPayer);
			parameters.Add("type", this.Type);
			parameters.Add("pageNo", this.PageNo);
			parameters.Add("pageSize", this.PageSize);
			return parameters;
		}
		#endregion
	}
}