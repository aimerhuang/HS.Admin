using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量查询供应商产品文描信息
	/// </summary>
	public class SupplierProductDescriptionGetRequest 
		: IYhdRequest<SupplierProductDescriptionGetResponse> 
	{
		/**产品id列表 */
			public string  ProductIdList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.supplier.product.description.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productIdList", this.ProductIdList);
			return parameters;
		}
		#endregion
	}
}
