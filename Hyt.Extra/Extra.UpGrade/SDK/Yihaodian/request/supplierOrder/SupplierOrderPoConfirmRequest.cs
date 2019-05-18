using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 确认PO单
	/// </summary>
	public class SupplierOrderPoConfirmRequest 
		: IYhdRequest<SupplierOrderPoConfirmResponse> 
	{
		/**被取消的采购单code */
			public string  PoCode{ get; set; }

		/**快递单号 */
			public string  ExpressNo{ get; set; }

		/**递送方式 */
			public int?  DeliveryMethod{ get; set; }

		/**递送人姓名 */
			public string  DeliveryPeople{ get; set; }

		/**递送人电话 */
			public string  DeliveryPeoplePhone{ get; set; }

		/**预计送达时间 */
			public string  ExpectedDeliveryDate{ get; set; }

		/**此值为json数组.其中 id：Long型，必填项，PO详情项ID； shipQty：Integer型，必填项，发货数量； lessRemark：String型，非必填项，包装发生变化的不一致的原因说明； enablePrint：Integer型，非必填项，是否打印（1打印，0不打印）； lessCode：Long型，非必填项，包装发生变化的不一致code； lessCode：不一致的原因code为下列值其一。 1：接受并确认可发货 2：销售区域限制 3：断货已停产 4：库存不足 5：缺货 6：其他 */
			public string  ConfirmitemList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.supplier.order.po.confirm";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("poCode", this.PoCode);
			parameters.Add("expressNo", this.ExpressNo);
			parameters.Add("deliveryMethod", this.DeliveryMethod);
			parameters.Add("deliveryPeople", this.DeliveryPeople);
			parameters.Add("deliveryPeoplePhone", this.DeliveryPeoplePhone);
			parameters.Add("expectedDeliveryDate", this.ExpectedDeliveryDate);
			parameters.Add("confirmitemList", this.ConfirmitemList);
			return parameters;
		}
		#endregion
	}
}
