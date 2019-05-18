using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取退货列表
	/// </summary>
	public class RefundGetRequest 
		: IYhdRequest<RefundGetResponse> 
	{
		/**订单code */
			public string  OrderCode{ get; set; }

		/**产品ID */
			public long?  ProductId{ get; set; }

		/**退货状态。(1000:全部；10:待审核；30:待退货；40:审批通过；110:待顾客寄回；120:待确认退款或确认换货；140:待完成换货；70:退换货完成；100:已关闭；130:客服仲裁) */
			public int?  RefundStatus{ get; set; }

		/**查询开始时间 */
			public string  StartTime{ get; set; }

		/**查询结束时间 */
			public string  EndTime{ get; set; }

		/**当前页数（默认为1） */
			public int?  CurPage{ get; set; }

		/**每页显示记录数（默认50.最大100） */
			public int?  PageRows{ get; set; }

		/** 	时间类型 :1、申请时间 2、更新时间 */
			public int?  DateType{ get; set; }

		/**退换货类型（默认查询退货）：0、退货 1、换货 1000：全部 */
			public int?  OperateType{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.refund.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("orderCode", this.OrderCode);
			parameters.Add("productId", this.ProductId);
			parameters.Add("refundStatus", this.RefundStatus);
			parameters.Add("startTime", this.StartTime);
			parameters.Add("endTime", this.EndTime);
			parameters.Add("curPage", this.CurPage);
			parameters.Add("pageRows", this.PageRows);
			parameters.Add("dateType", this.DateType);
			parameters.Add("operateType", this.OperateType);
			return parameters;
		}
		#endregion
	}
}
