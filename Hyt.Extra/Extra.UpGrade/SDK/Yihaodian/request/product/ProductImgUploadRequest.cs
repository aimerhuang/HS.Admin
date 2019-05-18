using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 添加商品图片
	/// </summary>
	public class ProductImgUploadRequest 
		: IYhdRequest<ProductImgUploadResponse> 
	{
		/**1号店产品ID,与outerId二选一(productId优先) */
			public long?  ProductId{ get; set; }

		/**外部产品ID,与productId二选一 */
			public string  OuterId{ get; set; }

		/**主图名称 */
			public string  MainImageName{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.product.img.upload";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("mainImageName", this.MainImageName);
			return parameters;
		}
		#endregion
	}
}
