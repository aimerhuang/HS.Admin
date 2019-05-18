using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取供应商Po单数据
	/// </summary>
	public class SupplierOrderPoGetRequest 
		: IYhdRequest<SupplierOrderPoGetResponse> 
	{
		/**PO单类型0:采购1：退货 */
			public long?  PoType{ get; set; }

		/**PO单ID */
			public long?  Id{ get; set; }

		/**下单开始日期 */
			public string  StartDate{ get; set; }

		/**下单结束日期 */
			public string  EndDate{ get; set; }

		/**PO状态： 0待批准，1批准，2待收货，3部分收货，4拒绝收货，5完成，6 终止，7等待取消，8退货完成，9作废 */
			public long?  PoStatus{ get; set; }

		/**查询页大小，不能大于100 */
			public int?  PageRows{ get; set; }

		/**查询页数 */
			public int?  CurPage{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.supplier.order.po.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("poType", this.PoType);
			parameters.Add("id", this.Id);
			parameters.Add("startDate", this.StartDate);
			parameters.Add("endDate", this.EndDate);
			parameters.Add("poStatus", this.PoStatus);
			parameters.Add("pageRows", this.PageRows);
			parameters.Add("curPage", this.CurPage);
			return parameters;
		}
		#endregion
	}
}
