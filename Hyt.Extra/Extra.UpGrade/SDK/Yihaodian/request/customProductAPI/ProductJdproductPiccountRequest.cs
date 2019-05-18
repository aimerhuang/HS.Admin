using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询京东商品的图片数量
	/// </summary>
	public class ProductJdproductPiccountRequest 
		: IYhdRequest<ProductJdproductPiccountResponse> 
	{
		/**京东商品的skuId */
			public int?  OuterId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.product.jdproduct.piccount";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("outerId", this.OuterId);
			return parameters;
		}
		#endregion
	}
}
