using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 取消PO单
	/// </summary>
	public class SupplierOrderPoCancelRequest 
		: IYhdRequest<SupplierOrderPoCancelResponse> 
	{
		/**被取消的采购单ID */
			public long?  PoId{ get; set; }

		/**备注信息 */
			public string  Remark{ get; set; }

		/**取消的时间 */
			public string  Time{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.supplier.order.po.cancel";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("poId", this.PoId);
			parameters.Add("remark", this.Remark);
			parameters.Add("time", this.Time);
			return parameters;
		}
		#endregion
	}
}
