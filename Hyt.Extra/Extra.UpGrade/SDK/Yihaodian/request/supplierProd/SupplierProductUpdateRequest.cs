using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 更新供应商产品信息
	/// </summary>
	public class SupplierProductUpdateRequest 
		: IYhdRequest<SupplierProductUpdateResponse> 
	{
		/**1号店产品ID */
			public long?  ProductId{ get; set; }

		/**产品类别ID(只有单品能修改） */
			public long?  CategoryId{ get; set; }

		/**产品名称(需要在产品名称前面添加品牌名称加空格,不超过100个字符) */
			public string  ProductCname{ get; set; }

		/**产品名称副标题(不超过100个字符) */
			public string  ProductSubTitle{ get; set; }

		/**产品名称前缀(不超过10个字符) */
			public string  ProductNamePrefix{ get; set; }

		/**产品描述(文描)(不超过300kb) */
			public string  ProductDescription{ get; set; }

		/**产品属性列表(每个属性之间用逗号分隔,属性ID和属性值之间用冒号分隔,其中属性值可以不输入。如属性ID:属性值),属性id不能重复 */
			public string  ProdAttributeInfoList{ get; set; }

		/**属性项列表(每个属性之间用逗号分隔,属性ID和选项ID之间用冒号分隔,选项ID之间用竖线分隔,其中选项ID可以不输入。如属性ID:选项ID1|选项ID2),属性id不能重复,也不能同上面的属性ID相同,同一个属性ID不能有相同的选项ID */
			public string  ProdAttributeItemInfoList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.supplier.product.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("categoryId", this.CategoryId);
			parameters.Add("productCname", this.ProductCname);
			parameters.Add("productSubTitle", this.ProductSubTitle);
			parameters.Add("productNamePrefix", this.ProductNamePrefix);
			parameters.Add("productDescription", this.ProductDescription);
			parameters.Add("prodAttributeInfoList", this.ProdAttributeInfoList);
			parameters.Add("prodAttributeItemInfoList", this.ProdAttributeItemInfoList);
			return parameters;
		}
		#endregion
	}
}
