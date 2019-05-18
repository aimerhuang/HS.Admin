using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取商品属性
	/// </summary>
	public class ProductAttributeSingleGetRequest 
		: IYhdRequest<ProductAttributeSingleGetResponse> 
	{
		/**1号店产品ID（优先使用） */
			public long?  ProductId{ get; set; }

		/**1号店产品外部编码（与产品ID二选一） */
			public string  OuterId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.product.attribute.single.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			return parameters;
		}
		#endregion
	}
}
