using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 订单发货(更新订单物流信息)
	/// </summary>
	public class LogisticsOrderShipmentsUpdateRequest 
		: IYhdRequest<LogisticsOrderShipmentsUpdateResponse> 
	{
		/**订单号(订单编码) */
			public string  OrderCode{ get; set; }

		/**配送商ID(从获取物流信息接口中获取) */
			public long?  DeliverySupplierId{ get; set; }

		/**运单号(快递编号) */
			public string  ExpressNbr{ get; set; }

		/**箱子 CartonCode:ShipmentNo:Weight:Length:Width:Height<br/>示例为2323:34344444:33:44:22:11 如果有一个没有值，如ShipmentNo，格式为2323::33:44:22 */
			public string  CartonList{ get; set; }

		/**实际发货数量 itemId1:number1,itemId2:number2,...示例234343:2,465746:1,123456:0 备注:如果item对应的number为0表示一件不发, 2表示发2件, 如果item:number不传默认该item全发, 如果deliveryItemNumberList不传默认该订单全发 */
			public string  DeliveryItemNumberList{ get; set; }

		/**线下交易ID */
			public string  TransactionId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.logistics.order.shipments.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("orderCode", this.OrderCode);
			parameters.Add("deliverySupplierId", this.DeliverySupplierId);
			parameters.Add("expressNbr", this.ExpressNbr);
			parameters.Add("cartonList", this.CartonList);
			parameters.Add("deliveryItemNumberList", this.DeliveryItemNumberList);
			parameters.Add("transactionId", this.TransactionId);
			return parameters;
		}
		#endregion
	}
}
