using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批准退货
	/// </summary>
	public class RefundApproveRequest 
		: IYhdRequest<RefundApproveResponse> 
	{
		/**退货单编码 */
			public string  RefundCode{ get; set; }

		/**申请明细的退换货数量。不填默认顾客申请的退换货数量，选填时应输入所有顾客原先申请的订单明细id和要修改的退换货数量 */
			public string  GrfDetails{ get; set; }

		/**商品退款金额（不含邮费）。退款金额不能大于（退货数量 乘以商品单价） */
			public double?  ProductAmount{ get; set; }

		/**是否退运费。0:不退运费，1：退运费。 */
			public int?  IsDeliveryFee{ get; set; }

		/**是否寄回。0:不寄回，1：寄回。如果不寄回，则直接退款。 */
			public int?  SendBackType{ get; set; }

		/**是否使用默认联系人。0:不使用，1：使用。使用默认联系时，下面的参数联系方式等参数可以不填。否则，必填。 */
			public int?  IsDefaultContactName{ get; set; }

		/**联系人名称 */
			public string  ContactName{ get; set; }

		/**联系人电话 */
			public string  ContactPhone{ get; set; }

		/**联系人地址 */
			public string  SendBackAddress{ get; set; }

		/**审核备注信息 */
			public string  Remark{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.refund.approve";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("refundCode", this.RefundCode);
			parameters.Add("grfDetails", this.GrfDetails);
			parameters.Add("productAmount", this.ProductAmount);
			parameters.Add("isDeliveryFee", this.IsDeliveryFee);
			parameters.Add("sendBackType", this.SendBackType);
			parameters.Add("isDefaultContactName", this.IsDefaultContactName);
			parameters.Add("contactName", this.ContactName);
			parameters.Add("contactPhone", this.ContactPhone);
			parameters.Add("sendBackAddress", this.SendBackAddress);
			parameters.Add("remark", this.Remark);
			return parameters;
		}
		#endregion
	}
}
