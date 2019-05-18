using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量获取产品库存信息
	/// </summary>
	public class ProductsStockGetRequest 
		: IYhdRequest<ProductsStockGetResponse> 
	{
		/**1号店产品ID列表（逗号分隔）与outerIdList二选一,最大长度为100 */
			public string  ProductIdList{ get; set; }

		/**外部产品ID列表（逗号分隔）与productIdList二选一,最大长度为100 */
			public string  OuterIdList{ get; set; }

		/**仓库id（不传则是默认仓库） */
			public long?  WarehouseId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.products.stock.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productIdList", this.ProductIdList);
			parameters.Add("outerIdList", this.OuterIdList);
			parameters.Add("warehouseId", this.WarehouseId);
			return parameters;
		}
		#endregion
	}
}
