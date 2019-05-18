using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 异常订单退款查询接口
	/// </summary>
	public class OrdersRefundAbnormalGetRequest 
		: IYhdRequest<OrdersRefundAbnormalGetResponse> 
	{
		/**退款单号 */
			public string  RefundOrderCode{ get; set; }

		/**订单号 */
			public string  OrderCode{ get; set; }

		/**退货单号 */
			public string  RefundCode{ get; set; }

		/**退款单状态 */
			public string  RefundStatus{ get; set; }

		/**收货人手机、电话 */
			public string  ReceiverPhone{ get; set; }

		/**最小50，最大100，默认50 */
			public int?  PageRows{ get; set; }

		/**页码 */
			public int?  CurPage{ get; set; }

		/**开始时间 */
			public string  StartTime{ get; set; }

		/**结束时间 */
			public string  EndTime{ get; set; }

		/**1表示申请，2表示批准（当dateType=1,startTime表示申请开始时间,endTime表示申请结束时间；dateType=2，startTime表示批准开始时间,endTime表示批准结束时间） */
			public int?  DateType{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.orders.refund.abnormal.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("refundOrderCode", this.RefundOrderCode);
			parameters.Add("orderCode", this.OrderCode);
			parameters.Add("refundCode", this.RefundCode);
			parameters.Add("refundStatus", this.RefundStatus);
			parameters.Add("receiverPhone", this.ReceiverPhone);
			parameters.Add("pageRows", this.PageRows);
			parameters.Add("curPage", this.CurPage);
			parameters.Add("startTime", this.StartTime);
			parameters.Add("endTime", this.EndTime);
			parameters.Add("dateType", this.DateType);
			return parameters;
		}
		#endregion
	}
}
