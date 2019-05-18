using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 出库/妥投/拒收订单接口
	/// </summary>
	public class JdDealDeliveryorderRequest 
		: IYhdRequest<JdDealDeliveryorderResponse> 
	{
		/**订单号(YHD的So号) */
			public string  OrderCode{ get; set; }

		/**操作类型（1：出库、2：妥投、3：拒收） */
			public string  DeliveryOrderType{ get; set; }

		/**固定写"JD配送"对应的ID，商家后台配置一个 */
			public string  DeliverySupplierID{ get; set; }

		/**固定写YHD的So号，TMS在下单后根据So号去JD拉取物流日志 */
			public string  ExpressNbr{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.jd.deal.deliveryorder";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("orderCode", this.OrderCode);
			parameters.Add("deliveryOrderType", this.DeliveryOrderType);
			parameters.Add("deliverySupplierID", this.DeliverySupplierID);
			parameters.Add("expressNbr", this.ExpressNbr);
			return parameters;
		}
		#endregion
	}
}
