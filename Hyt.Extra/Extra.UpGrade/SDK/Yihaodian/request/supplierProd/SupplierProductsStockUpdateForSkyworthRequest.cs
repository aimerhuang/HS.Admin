using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 更新创维供应商产品库存接口
	/// </summary>
	public class SupplierProductsStockUpdateForSkyworthRequest 
		: IYhdRequest<SupplierProductsStockUpdateForSkyworthResponse> 
	{
		/**外部产品编码与1号店的产品编码有对应关系 */
			public string  CompanyProductCode{ get; set; }

		/**希望调整到的库存数量 */
			public string  TargetStockNum{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.supplier.products.stock.updateForSkyworth";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("companyProductCode", this.CompanyProductCode);
			parameters.Add("targetStockNum", this.TargetStockNum);
			return parameters;
		}
		#endregion
	}
}
