using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 更新单个产品库存信息
	/// </summary>
	public class ProductStockUpdateRequest 
		: IYhdRequest<ProductStockUpdateResponse> 
	{
		/**1号店产品ID,与outerId二选一.存在非法字符默认为空(productId优先) */
			public long?  ProductId{ get; set; }

		/**外部产品ID,与productId二选一 */
			public string  OuterId{ get; set; }

		/**虚拟库存。库存值小于对应类别库存最大值。
请从<a href="yhd.category.products.get.html" target="_blank">yhd.category.products.get获取</a>中，查询对应类别的库存最大值 */
			public int?  VirtualStockNum{ get; set; }

		/**仓库ID（不传则是默认仓库） */
			public long?  WarehouseId{ get; set; }

		/**更新类型，（1：全量更新，2：冻结库存上增量更新，3：库存上增量更新） */
			public int?  UpdateType{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.product.stock.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("virtualStockNum", this.VirtualStockNum);
			parameters.Add("warehouseId", this.WarehouseId);
			parameters.Add("updateType", this.UpdateType);
			return parameters;
		}
		#endregion
	}
}
