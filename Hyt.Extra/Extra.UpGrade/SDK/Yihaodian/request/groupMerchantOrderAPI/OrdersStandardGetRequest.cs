using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// SAM获取订单接口
	/// </summary>
	public class OrdersStandardGetRequest 
		: IYhdRequest<OrdersStandardGetResponse> 
	{
		/**订单状态（逗号分隔）: 
ORDER_WAIT_PAY：已下单（货款未全收）、 
ORDER_PAYED：已下单（货款已收）、 
ORDER_WAIT_SEND：可以发货（已送仓库）、 
ORDER_ON_SENDING：已出库（货在途）、 
ORDER_RECEIVED：货物用户已收到、 
ORDER_FINISH：订单完成、 
ORDER_CANCEL：订单取消 */
			public string  OrderStatusList{ get; set; }

		/**订单编码列表（逗号分隔）,最大50个订单号；此字段有值的情况下，会忽略其他入参 */
			public string  OrderCodeList{ get; set; }

		/**日期类型(1：订单生成日期，2：订单付款日期，3：订单发货日期，4：订单收货日期，5：订单更新日期，6：SO2DO日期) */
			public int?  DateType{ get; set; }

		/**查询开始时间 */
			public string  StartTime{ get; set; }

		/**查询结束时间(查询开始、结束时间跨度不能超过15天) */
			public string  EndTime{ get; set; }

		/**当前页数 */
			public int?  CurPage{ get; set; }

		/**每页显示记录数，默认50，最大100 */
			public int?  PageRows{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.orders.standard.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("orderStatusList", this.OrderStatusList);
			parameters.Add("orderCodeList", this.OrderCodeList);
			parameters.Add("dateType", this.DateType);
			parameters.Add("startTime", this.StartTime);
			parameters.Add("endTime", this.EndTime);
			parameters.Add("curPage", this.CurPage);
			parameters.Add("pageRows", this.PageRows);
			return parameters;
		}
		#endregion
	}
}
