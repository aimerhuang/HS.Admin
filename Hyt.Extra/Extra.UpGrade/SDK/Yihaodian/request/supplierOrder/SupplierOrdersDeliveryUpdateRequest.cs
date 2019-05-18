using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量订单发货
	/// </summary>
	public class SupplierOrdersDeliveryUpdateRequest 
		: IYhdRequest<SupplierOrdersDeliveryUpdateResponse> 
	{
		/**订单发货信息（配送公司:配送单号:订单code，配送公司1:配送单号1:订单code1:配送公司Id，配送公司:配送单号_配送单号_配送单号:订单code1:配送公司Id:箱数）                                                   注意： 箱数大于1的时候，配送公司，配送单号，订单号，物流公司id,箱数都必传。 */
			public string  DeliveryOrderInfoList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.supplier.orders.delivery.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("deliveryOrderInfoList", this.DeliveryOrderInfoList);
			return parameters;
		}
		#endregion
	}
}
