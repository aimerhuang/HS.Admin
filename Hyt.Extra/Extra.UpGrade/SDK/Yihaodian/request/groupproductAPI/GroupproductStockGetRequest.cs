using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取集团商家产品库存信息
	/// </summary>
	public class GroupproductStockGetRequest 
		: IYhdRequest<GroupproductStockGetResponse> 
	{
		/**1号店产品ID与outerId二选一 */
			public long?  ProductId{ get; set; }

		/**外部产品ID与productId二选一 */
			public string  OuterId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.groupproduct.stock.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			return parameters;
		}
		#endregion
	}
}
