using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 异常订单退款接口
	/// </summary>
	public class JdDealRefundRequest 
		: IYhdRequest<JdDealRefundResponse> 
	{
		/**订单号(YHD的So号) */
			public string  OrderCode{ get; set; }

		/**JD赔付单号，记录在YHD表中 */
			public string  JdRefundCode{ get; set; }

		/**退款金额 */
			public string  RefundAmount{ get; set; }

		/**退款类型
2：商品破损补偿
3：商品质量问题
4：发货延迟补偿
7：商品差价补偿
5：其他返利 */
			public string  RefundType{ get; set; }

		/**退款备注 */
			public string  RefundRemark{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.jd.deal.refund";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("orderCode", this.OrderCode);
			parameters.Add("jdRefundCode", this.JdRefundCode);
			parameters.Add("refundAmount", this.RefundAmount);
			parameters.Add("refundType", this.RefundType);
			parameters.Add("refundRemark", this.RefundRemark);
			return parameters;
		}
		#endregion
	}
}
