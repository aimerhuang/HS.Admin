using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取商品文描
	/// </summary>
	public class ProductDescriptionGetRequest 
		: IYhdRequest<ProductDescriptionGetResponse> 
	{
		/**1号店产品ID列表（逗号分隔，优先使用）与outerIdList二选一,最大长度为100 */
			public string  ProductIdList{ get; set; }

		/**外部产品ID列表（逗号分隔）与productIdList二选一,最大长度为100 */
			public string  OuterIdList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.product.description.get";
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
