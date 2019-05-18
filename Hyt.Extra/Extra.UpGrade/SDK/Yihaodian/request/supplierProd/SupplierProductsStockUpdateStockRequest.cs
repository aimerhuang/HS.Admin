using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 更新供应商产品库存接口
	/// </summary>
	public class SupplierProductsStockUpdateStockRequest 
		: IYhdRequest<SupplierProductsStockUpdateStockResponse> 
	{
		/**1号店的产品编码 */
			public string  ProductCode{ get; set; }

		/**希望调整到的库存数量 */
			public string  TargetStockNum{ get; set; }

		/**供应商ID */
			public long?  SupplierId{ get; set; }

		/**YHD仓库ID */
			public string  WarehouseId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.supplier.products.stock.updateStock";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productCode", this.ProductCode);
			parameters.Add("targetStockNum", this.TargetStockNum);
			parameters.Add("supplierId", this.SupplierId);
			parameters.Add("warehouseId", this.WarehouseId);
			return parameters;
		}
		#endregion
	}
}
