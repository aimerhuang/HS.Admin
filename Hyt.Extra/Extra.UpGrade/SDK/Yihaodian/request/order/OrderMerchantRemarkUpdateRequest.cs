using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 更新单个订单的卖家备注信息
	/// </summary>
	public class OrderMerchantRemarkUpdateRequest 
		: IYhdRequest<OrderMerchantRemarkUpdateResponse> 
	{
		/**订单编码 */
			public string  OrderCode{ get; set; }

		/**订单卖家备注,最大长度为150个汉字 */
			public string  Remark{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.order.merchant.remark.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("orderCode", this.OrderCode);
			parameters.Add("remark", this.Remark);
			return parameters;
		}
		#endregion
	}
}
