using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// JD确认退款通知
	/// </summary>
	public class RefundJdGrfRefundRequest 
		: IYhdRequest<RefundJdGrfRefundResponse> 
	{
		/**1号店退换货ID。 */
			public int?  ApplyIdYhd{ get; set; }

		/**实际退款给用户金额（可退金额-扣款项-抵用券） */
			public double?  RefundAmount{ get; set; }

		/**商品退款金额（总付款项 – JD 分摊单件优惠券金额 – YHD 分摊单件优惠券金额 – 折扣金额） */
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
			return "yhd.refund.jd.grf.refund";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("applyIdYhd", this.ApplyIdYhd);
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
