using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询历史订单列表
	/// </summary>
	public class SupplierHistoryOrdersGetRequest 
		: IYhdRequest<SupplierHistoryOrdersGetResponse> 
	{
		/**订单编码 */
			public string  OrderCode{ get; set; }

		/**订单开始时间（必须与结束时间同时给出或者都不给出，时间间隔不得大于31天） */
			public string  OrderStartTime{ get; set; }

		/**订单结束时间（必须与开始时间同时给出或者都不给出，时间间隔不得大于31天） */
			public string  OrderEndTime{ get; set; }

		/**订单状态（1：待发货，2：已发货，3：用户已收到货，4:已完成，5:订单已关闭，6：退换货，7：未支付） */
			public int?  OrderStatus{ get; set; }

		/**收货人名称 */
			public string  GoodReceiverName{ get; set; }

		/**收货人手机号码 */
			public string  GoodReceiverMobile{ get; set; }

		/**分页查询每页的个数 */
			public int?  PageRows{ get; set; }

		/**分页查询第几页 */
			public int?  CurPage{ get; set; }

		/**订单更新开始时间（必须与结束时间同时给出或者都不给出，时间间隔不得大于31天） */
			public string  UpdateStartTime{ get; set; }

		/**订单更新结束时间（必须与开始时间同时给出或者都不给出，时间间隔不得大于31天） */
			public string  UpdataEndTime{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.supplier.history.orders.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("orderCode", this.OrderCode);
			parameters.Add("orderStartTime", this.OrderStartTime);
			parameters.Add("orderEndTime", this.OrderEndTime);
			parameters.Add("orderStatus", this.OrderStatus);
			parameters.Add("goodReceiverName", this.GoodReceiverName);
			parameters.Add("goodReceiverMobile", this.GoodReceiverMobile);
			parameters.Add("pageRows", this.PageRows);
			parameters.Add("curPage", this.CurPage);
			parameters.Add("updateStartTime", this.UpdateStartTime);
			parameters.Add("updataEndTime", this.UpdataEndTime);
			return parameters;
		}
		#endregion
	}
}
