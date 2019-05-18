using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 更新单个产品外部编码
	/// </summary>
	public class ProductOuteridUpdateRequest 
		: IYhdRequest<ProductOuteridUpdateResponse> 
	{
		/**1号店产品ID */
			public long?  ProductId{ get; set; }

		/**外部产品编码id,最大长度为100 */
			public string  OuterId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.product.outerid.update";
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
