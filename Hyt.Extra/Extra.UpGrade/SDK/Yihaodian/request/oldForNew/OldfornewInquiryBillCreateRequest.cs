using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 创建询价单接口
	/// </summary>
	public class OldfornewInquiryBillCreateRequest 
		: IYhdRequest<OldfornewInquiryBillCreateResponse> 
	{
		/**第三方询价单id */
			public long?  InquiryBillId{ get; set; }

		/**第三方商品编码code */
			public string  ProductCode{ get; set; }

		/**第三方商品名称 */
			public string  ProductName{ get; set; }

		/**第三方询价内容，格式 Q:A;Q:A */
			public string  ProductAttrs{ get; set; }

		/**兑换类型（0、分类券，1、品牌券，2、产品券，3、全场返利） */
			public int?  ExchangeType{ get; set; }

		/**备注 */
			public string  Remark{ get; set; }

		/**商品图片 */
			public string  ProductPicture{ get; set; }

		/**活动码 */
			public string  ActiveCode{ get; set; }

		/**合作商id */
			public long?  PartnerBusinessId{ get; set; }

		/**报价(格式: 兑换类型:活动ID:金额;兑换类型:活动ID:金额...) */
			public string  QuotedPrice{ get; set; }

		/**所选抵用券 */
			public double?  ConfirmCouponAmount{ get; set; }

		/**返利金额 */
			public double?  ConfirmRebateAmount{ get; set; }

		/**签名 */
			public string  SignPassport{ get; set; }

		/**结算价 */
			public double?  SettlementAmount{ get; set; }

		/**用户标识 */
			public string  Uec{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.oldfornew.inquiry.bill.create";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("inquiryBillId", this.InquiryBillId);
			parameters.Add("productCode", this.ProductCode);
			parameters.Add("productName", this.ProductName);
			parameters.Add("productAttrs", this.ProductAttrs);
			parameters.Add("exchangeType", this.ExchangeType);
			parameters.Add("remark", this.Remark);
			parameters.Add("productPicture", this.ProductPicture);
			parameters.Add("activeCode", this.ActiveCode);
			parameters.Add("partnerBusinessId", this.PartnerBusinessId);
			parameters.Add("quotedPrice", this.QuotedPrice);
			parameters.Add("confirmCouponAmount", this.ConfirmCouponAmount);
			parameters.Add("confirmRebateAmount", this.ConfirmRebateAmount);
			parameters.Add("signPassport", this.SignPassport);
			parameters.Add("settlementAmount", this.SettlementAmount);
			parameters.Add("uec", this.Uec);
			return parameters;
		}
		#endregion
	}
}
