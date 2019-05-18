using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询订单列表
	/// </summary>
	public class SupplierOrdersGetRequest 
		: IYhdRequest<SupplierOrdersGetResponse> 
	{
		/**订单code */
			public string  OrderCode{ get; set; }

		/**订单创建开始时间（必须与结束时间同时给出或者都不给出，时间间隔不得大于31天） */
			public string  OrderStartTime{ get; set; }

		/**订单创建结束时间(必须与开始时间同时给出或者都不给出，时间间隔不得大于31天) */
			public string  OrderEndTime{ get; set; }

		/**订单状态（1：待发货，2：已发货，3：用户已收到货，4:已完成，5:订单已关闭） */
			public int?  OrderStatus{ get; set; }

		/**收货人名称 */
			public string  GoodReceiverName{ get; set; }

		/**收货人手机号 */
			public string  GoodReceiverMobile{ get; set; }

		/**页面显示记录数 */
			public int?  PageRows{ get; set; }

		/**当前页 */
			public int?  CurPage{ get; set; }

		/**订单更新开始时间（必须与结束时间同时给出或者都不给出，时间间隔不得大于31天） */
			public string  UpdateStartTime{ get; set; }

		/**订单更新结束时间（必须与结束时间同时给出或者都不给出，时间间隔不得大于31天） */
			public string  UpdataEndTime{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.supplier.orders.get";
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
