using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量更新系列产品库存信息
	/// </summary>
	public class SerialProductsStockUpdateRequest 
		: IYhdRequest<SerialProductsStockUpdateResponse> 
	{
		/**系列产品Id，与outerId二选一 */
			public long?  ProductId{ get; set; }

		/**外部产品Id，与productId二选一 */
			public string  OuterId{ get; set; }

		/**更新类型，默认为1（1：全量更新，2：增量更新） */
			public int?  UpdateType{ get; set; }

		/**产品库存信息列表(产品库存信息之间逗号分隔,产品Id、仓库Id和库存数之间冒号分隔)与outerStockList二选一;warehouseId从<a href="http://open.yhd.com/doc2/apiDetail.do?apiName=yhd.logistics.warehouse.info.get" target="_blank">yhd.logistics.warehouse.info.get获取;</a>库存值必须小于对应类别库存最大值，请从<a href="http://open.yhd.com/doc2/apiDetail.do?apiName=yhd.category.products.get" target="_blank">yhd.category.products.get获取</a>中，查询对应类别的库存最大值 */
			public string  ProductStockList{ get; set; }

		/**产品库存信息列表(产品库存信息之间逗号分隔,外部产品Id、仓库Id和库存数之间冒号分隔)与productStockList二选一;warehouseId从<a href="http://open.yhd.com/doc2/apiDetail.do?apiName=yhd.logistics.warehouse.info.get" target="_blank">yhd.logistics.warehouse.info.get获取;</a>库存值必须小于对应类别库存最大值，请从<a href="http://open.yhd.com/doc2/apiDetail.do?apiName=yhd.category.products.get" target="_blank">yhd.category.products.get获取</a>中，查询对应类别的库存最大值 */
			public string  OuterStockList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.serial.products.stock.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("updateType", this.UpdateType);
			parameters.Add("productStockList", this.ProductStockList);
			parameters.Add("outerStockList", this.OuterStockList);
			return parameters;
		}
		#endregion
	}
}
