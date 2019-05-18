using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 更新单个产品价格信息
	/// </summary>
	public class ProductPriceUpdateRequest 
		: IYhdRequest<ProductPriceUpdateResponse> 
	{
		/**1号店产品ID,与outerId二选一 */
			public long?  ProductId{ get; set; }

		/**外部产品ID,与productId二选一 */
			public string  OuterId{ get; set; }

		/**1号店价格。price和marketPrice不能同时为空。 */
			public double?  Price{ get; set; }

		/**市场价。price和marketPrice不能同时为空。 */
			public double?  MarketPrice{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.product.price.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("price", this.Price);
			parameters.Add("marketPrice", this.MarketPrice);
			return parameters;
		}
		#endregion
	}
}
