using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询卖家已卖出的交易数据（根据创建时间）（兼容淘宝）
	/// </summary>
	public class TradesSoldGetRequest 
		: IYhdRequest<TradesSoldGetResponse> 
	{
		/**Trade中可以指定返回的fields。 */
			public string  Fields{ get; set; }

		/**查询两个月内交易创建时间开始。格式:yyyy-MM-dd HH:mm:ss  */
			public string  StartCreated{ get; set; }

		/**查询交易创建时间结束。格式:yyyy-MM-dd HH:mm:ss  */
			public string  EndCreated{ get; set; }

		/**订单状态（逗号分隔）: ORDER_WAIT_PAY：已下单（货款未全收）、 ORDER_PAYED：已下单（货款已收）、 ORDER_WAIT_SEND：可以发货（已送仓库）、 ORDER_ON_SENDING：已出库（货在途）、 ORDER_RECEIVED：货物用户已收到、 ORDER_FINISH：订单完成、 ORDER_CANCEL：订单取消 */
			public string  Status{ get; set; }

		/**买家昵称（暂不提供） */
			public string  BuyerNick{ get; set; }

		/**交易类型列表，同时查询多种交易类型可用逗号分隔。（暂不提供） */
			public string  Type{ get; set; }

		/**暂不提供 */
			public string  ExtType{ get; set; }

		/**评价状态，默认查询所有评价状态的数据，除了默认值外每次只能查询一种状态。（暂不提供） */
			public string  RateStatus{ get; set; }

		/**卖家对交易的自定义分组标签，目前可选值为：time_card（点卡软件代充），fee_card（话费软件代充） （暂不提供） */
			public string  Tag{ get; set; }

		/**页码。取值范围:大于零的整数; 默认值:1  */
			public long?  PageNo{ get; set; }

		/**每页条数 */
			public long?  PageSize{ get; set; }

		/**是否启用has_next的分页方式，如果指定true,则返回的结果中不包含总记录数，但是会新增一个是否存在下一页的的字段。 */
			public bool  UseHasNext{ get; set; }

		/**暂不提供 */
			public bool  IsAcookie{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.trades.sold.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("fields", this.Fields);
			parameters.Add("startCreated", this.StartCreated);
			parameters.Add("endCreated", this.EndCreated);
			parameters.Add("status", this.Status);
			parameters.Add("buyerNick", this.BuyerNick);
			parameters.Add("type", this.Type);
			parameters.Add("extType", this.ExtType);
			parameters.Add("rateStatus", this.RateStatus);
			parameters.Add("tag", this.Tag);
			parameters.Add("pageNo", this.PageNo);
			parameters.Add("pageSize", this.PageSize);
			parameters.Add("useHasNext", this.UseHasNext);
			parameters.Add("isAcookie", this.IsAcookie);
			return parameters;
		}
		#endregion
	}
}
