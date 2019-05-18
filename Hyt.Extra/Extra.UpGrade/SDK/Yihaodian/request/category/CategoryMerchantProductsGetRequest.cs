using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询商家产品类别列表(店铺类别)
	/// </summary>
	public class CategoryMerchantProductsGetRequest 
		: IYhdRequest<CategoryMerchantProductsGetResponse> 
	{
		/**父类别ID（0：根节点） */
			public long?  CategoryParentId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.category.merchant.products.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("categoryParentId", this.CategoryParentId);
			return parameters;
		}
		#endregion
	}
}
