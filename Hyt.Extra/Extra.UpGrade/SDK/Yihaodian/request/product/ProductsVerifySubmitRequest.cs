using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量提交商品审核
	/// </summary>
	public class ProductsVerifySubmitRequest 
		: IYhdRequest<ProductsVerifySubmitResponse> 
	{
		/**1号店产品Id列表(逗号分隔),与outerIdList二选一(productIdList优先),相同的产品id做一个产品id处理,最多100条 */
			public string  ProductIdList{ get; set; }

		/**外部产品编码列表(逗号分隔),与productIdList二选一,相同的外部产品编码做1个处理,最多100条 */
			public string  OuterIdList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.products.verify.submit";
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
