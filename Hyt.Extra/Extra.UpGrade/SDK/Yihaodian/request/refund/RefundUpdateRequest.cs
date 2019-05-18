using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 编辑退货
	/// </summary>
	public class RefundUpdateRequest 
		: IYhdRequest<RefundUpdateResponse> 
	{
		/**退货单编码 */
			public string  RefundCode{ get; set; }

		/**	 申请明细的退换货数量。不填默认不更新退换货数量，选填时应输入所有顾客原先申请的订单明细id和要修改的退换货数量 */
			public string  GrfDetails{ get; set; }

		/**商品退款金额（不含邮费）。退款金额不能大于（退货数量 乘以商品单价） */
			public double?  ProductAmount{ get; set; }

		/**是否退运费。0:不退运费，1：退运费。 */
			public int?  IsDeliveryFee{ get; set; }

		/**备注信息 */
			public string  Remark{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.refund.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("refundCode", this.RefundCode);
			parameters.Add("grfDetails", this.GrfDetails);
			parameters.Add("productAmount", this.ProductAmount);
			parameters.Add("isDeliveryFee", this.IsDeliveryFee);
			parameters.Add("remark", this.Remark);
			return parameters;
		}
		#endregion
	}
}
