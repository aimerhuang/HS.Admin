using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 1号店火车票退款接口
	/// </summary>
	public class VirtualTrainRefundGetRequest 
		: IYhdRequest<VirtualTrainRefundGetResponse> 
	{
		/**京东订单号 */
			public long?  JdOrderId{ get; set; }

		/**京东退款流水号 */
			public long?  JdRefundId{ get; set; }

		/**实际退款给用户金额（可退金额-扣款项-抵用券） */
			public double?  RefundAmount{ get; set; }

		/**商品退款金额（总付款项 – JD 分摊单件优惠券金额 –
 YHD 分摊单件优惠券金额 – 折扣金额） */
			public double?  ItemReceiveAmount{ get; set; }

		/**YHD优惠券退款金额（分摊单件） */
			public double?  YhdCouponAmount{ get; set; }

		/**JD 优惠券退款金额（分摊单件） */
			public double?  JdCouponAmount{ get; set; }

		/**扣款项（分摊单件商品手续费等） */
			public double?  Deduction{ get; set; }

		/**处理意见 */
			public string  Remark{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.virtual.train.refund.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("jdOrderId", this.JdOrderId);
			parameters.Add("jdRefundId", this.JdRefundId);
			parameters.Add("refundAmount", this.RefundAmount);
			parameters.Add("itemReceiveAmount", this.ItemReceiveAmount);
			parameters.Add("yhdCouponAmount", this.YhdCouponAmount);
			parameters.Add("jdCouponAmount", this.JdCouponAmount);
			parameters.Add("deduction", this.Deduction);
			parameters.Add("remark", this.Remark);
			return parameters;
		}
		#endregion
	}
}
