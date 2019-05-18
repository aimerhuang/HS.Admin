using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 更新一个产品
	/// </summary>
	public class ProductUpdateRequest 
		: IYhdRequest<ProductUpdateResponse> 
	{
		/**1号店产品ID,与outerId二选一(productId优先) */
			public long?  ProductId{ get; set; }

		/**外部产品ID,与productId二选一 */
			public string  OuterId{ get; set; }

		/**产品类别ID(只有单品能修改,从获取授权产品类别yhd.category.products.get接口中获取叶子ID) */
			public long?  CategoryId{ get; set; }

		/**商家产品类别ID列表(从获取商家产品类别接口中获取叶子ID) */
			public string  MerchantCategoryId{ get; set; }

		/**产品名称(需要在产品名称前面添加品牌名称加空格,不超过100个字符) */
			public string  ProductCname{ get; set; }

		/**产品名称副标题(不超过100个字符或汉字) */
			public string  ProductSubTitle{ get; set; }

		/**产品名称前缀(不超过10个字符) */
			public string  ProductNamePrefix{ get; set; }

		/**产品描述(文描)(不超过300kb) */
			public string  ProductDescription{ get; set; }

		/**节能补贴金额(最多两位小数,50~400) */
			public double?  SubsidyAmount{ get; set; }

		/**产品属性列表(每个属性之间用逗号分隔,属性ID和属性值之间用冒号分隔,其中属性值可以不输入。如属性ID:属性值),属性id不能重复 */
			public string  ProdAttributeInfoList{ get; set; }

		/**属性项列表:
1.每个<b>属性值对</b>以逗号分隔（<b>属性值对</b>：由属性id、选项id、子选项id组成）；
2.属性类型为单选且没有子属性，<b>属性值对</b>以‘属性id：选项id’形式组合；
3.属性类型为单选且有子属性，<b>属性值对</b>以‘属性id：选项id_子选项id’形式组合；
4.属性类型为多选，<b>属性值对</b>以‘属性id：选项id|选项id’形式组合。
<font color="red">备注</font>：属性id不能重复,也不能同prodAttributeInfoList中的属性ID相同,同一个属性ID不能有相同的选项ID */
			public string  ProdAttributeItemInfoList{ get; set; }

		/**产品条形码 */
			public string  Barcode{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.product.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("categoryId", this.CategoryId);
			parameters.Add("merchantCategoryId", this.MerchantCategoryId);
			parameters.Add("productCname", this.ProductCname);
			parameters.Add("productSubTitle", this.ProductSubTitle);
			parameters.Add("productNamePrefix", this.ProductNamePrefix);
			parameters.Add("productDescription", this.ProductDescription);
			parameters.Add("subsidyAmount", this.SubsidyAmount);
			parameters.Add("prodAttributeInfoList", this.ProdAttributeInfoList);
			parameters.Add("prodAttributeItemInfoList", this.ProdAttributeItemInfoList);
			parameters.Add("barcode", this.Barcode);
			return parameters;
		}
		#endregion
	}
}
