using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 更新产品中文名称
	/// </summary>
	public class ProductCnameUpdateRequest 
		: IYhdRequest<ProductCnameUpdateResponse> 
	{
		/**1号店产品ID,与outerId二选一(productId优先) */
			public long?  ProductId{ get; set; }

		/**外部产品ID,与productId二选一 */
			public string  OuterId{ get; set; }

		/**产品名称(不超过100个字符) */
			public string  ProductCname{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.product.cname.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("productCname", this.ProductCname);
			return parameters;
		}
		#endregion
	}
}
