using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 同意/拒绝取消接口
	/// </summary>
	public class JdDealCancelorderRequest 
		: IYhdRequest<JdDealCancelorderResponse> 
	{
		/**订单号(YHD的So号) */
			public string  OrderCode{ get; set; }

		/**操作类型（1：同意、2：拒绝） */
			public string  CancelOrderType{ get; set; }

		/**拒绝理由(如果cancelOrderType为2，拒绝理由必填)
1:订单已出库无法取消
2：已与买家沟通可发货
3：买家取消理由不充分
4：其他 */
			public string  RefuseReason{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.jd.deal.cancelorder";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("orderCode", this.OrderCode);
			parameters.Add("cancelOrderType", this.CancelOrderType);
			parameters.Add("refuseReason", this.RefuseReason);
			return parameters;
		}
		#endregion
	}
}
