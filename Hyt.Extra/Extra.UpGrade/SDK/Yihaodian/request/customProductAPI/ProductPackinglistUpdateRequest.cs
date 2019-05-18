using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 普通品新增包装清单和售后服务
	/// </summary>
	public class ProductPackinglistUpdateRequest 
		: IYhdRequest<ProductPackinglistUpdateResponse> 
	{
		/**1号店产品ID,与outerId二选一(productId优先) */
			public long?  ProductId{ get; set; }

		/**外部产品ID,与productId二选一 */
			public string  OuterId{ get; set; }

		/**包装清单必填 */
			public string  PackingList{ get; set; }

		/**售后服务必填 */
			public string  AfterSaleService{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.product.packinglist.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("packingList", this.PackingList);
			parameters.Add("afterSaleService", this.AfterSaleService);
			return parameters;
		}
		#endregion
	}
}
