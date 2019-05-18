using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量获取京东产品价格信息
	/// </summary>
	public class ProductsJdPriceGetRequest 
		: IYhdRequest<ProductsJdPriceGetResponse> 
	{
		/**1号店产品ID列表（逗号分隔）,与outerIdList二选一,最大长度为100 */
			public string  ProductIdList{ get; set; }

		/**外部产品ID列表（逗号分隔）,与productIdList二选一,最大长度为100 */
			public string  OuterIdList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.products.jd.price.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productIdList", this.ProductIdList);
			parameters.Add("outerIdList", this.OuterIdList);
			return parameters;
		}
		#endregion
	}
}
