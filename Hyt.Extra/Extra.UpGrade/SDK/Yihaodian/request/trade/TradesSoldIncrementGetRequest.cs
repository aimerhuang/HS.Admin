using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询卖家已卖出的增量交易数据（根据修改时间,兼容淘宝接口）
	/// </summary>
	public class TradesSoldIncrementGetRequest 
		: IYhdRequest<TradesSoldIncrementGetResponse> 
	{
		/**需要返回的字段。 */
			public string  Fields{ get; set; }

		/**查询修改开始时间(修改时间跨度不能大于一天)。格式:yyyy-MM-dd HH:mm:ss  */
			public string  StartModified{ get; set; }

		/**查询修改结束时间，必须大于修改开始时间(修改时间跨度不能大于一天)，格式:yyyy-MM-dd HH:mm:ss。 */
			public string  EndModified{ get; set; }

		/**交易状态，默认查询所有交易状态的数据，除了默认值外每次只能查询一种状态订单状态（逗号分隔）: ORDER_WAIT_PAY：已下单（货款未全收）、 ORDER_PAYED：已下单（货款已收）、 ORDER_WAIT_SEND：可以发货（已送仓库）、 ORDER_ON_SENDING：已出库（货在途）、 ORDER_RECEIVED：货物用户已收到、 ORDER_FINISH：订单完成、 ORDER_CANCEL：订单取消 */
			public string  Status{ get; set; }

		/**交易类型列表（暂不支持） */
			public string  Type{ get; set; }

		/** 	可选值有ershou(二手市场的订单（暂不支持） */
			public string  ExtType{ get; set; }

		/**卖家对交易的自定义分组标签（暂不支持） */
			public string  Tag{ get; set; }

		/**页码。取值范围:大于零的整数;默认值:1。 */
			public long?  PageNo{ get; set; }

		/**每页条数。取值范围：1~100，默认值：40 */
			public long?  PageSize{ get; set; }

		/**是否启用has_next的分页方式（暂不支持） */
			public bool  UseHasNext{ get; set; }

		/**默认值为false，表示按正常方式查询订单；(暂不支持) */
			public bool  IsAcookie{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.trades.sold.increment.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("fields", this.Fields);
			parameters.Add("startModified", this.StartModified);
			parameters.Add("endModified", this.EndModified);
			parameters.Add("status", this.Status);
			parameters.Add("type", this.Type);
			parameters.Add("extType", this.ExtType);
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
