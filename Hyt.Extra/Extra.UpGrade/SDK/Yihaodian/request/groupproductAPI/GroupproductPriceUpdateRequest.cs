using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 集团商家更新其区域商家1号店价
	/// </summary>
	public class GroupproductPriceUpdateRequest 
		: IYhdRequest<GroupproductPriceUpdateResponse> 
	{
		/**集团商家正式产品id */
			public long?  FormalProductId{ get; set; }

		/**待更新价格的区域商家List 参数,多个区域商家请使用英文逗号分隔。1号店价不能高于市场价 */
			public string  AreaMerchantProductList{ get; set; }

		/**集团商家外部编码，用于查询集团商家商品 */
			public string  OuterId{ get; set; }

		/**类目id */
			public long?  CategoryId{ get; set; }

		/**集团商家id */
			public long?  GroupMerchantId{ get; set; }

		/**市场价 */
			public double?  ProductListPrice{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.groupproduct.price.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("formalProductId", this.FormalProductId);
			parameters.Add("areaMerchantProductList", this.AreaMerchantProductList);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("categoryId", this.CategoryId);
			parameters.Add("groupMerchantId", this.GroupMerchantId);
			parameters.Add("productListPrice", this.ProductListPrice);
			return parameters;
		}
		#endregion
	}
}
