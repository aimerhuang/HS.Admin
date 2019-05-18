using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询供应商产品信息
	/// </summary>
	public class SupplierProductsGetRequest 
		: IYhdRequest<SupplierProductsGetResponse> 
	{
		/**是否可见(强制上/下架),1是0否 */
			public int?  CanShow{ get; set; }

		/**上下架状态0：下架，1：上架 */
			public int?  CanSale{ get; set; }

		/**当前页数（默认1） */
			public int?  CurPage{ get; set; }

		/**每页显示记录数（默认50、最大限制：100） */
			public int?  PageRows{ get; set; }

		/**商家产品中文名称(支持模糊查询) */
			public string  ProductCname{ get; set; }

		/**产品Id列表(最多productId个数为100) */
			public string  ProductIdList{ get; set; }

		/**产品类别Id（1号店类别） */
			public long?  CategoryId{ get; set; }

		/**品牌Id */
			public long?  BrandId{ get; set; }

		/**产品类型  0：普通产品 1：主系列产品 2：子系列产品 3：捆绑产品 4：实体礼品卡 5: 虚拟商品 6:增值服务 7:电子礼品卡 */
			public int?  ProductType{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.supplier.products.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("canShow", this.CanShow);
			parameters.Add("canSale", this.CanSale);
			parameters.Add("curPage", this.CurPage);
			parameters.Add("pageRows", this.PageRows);
			parameters.Add("productCname", this.ProductCname);
			parameters.Add("productIdList", this.ProductIdList);
			parameters.Add("categoryId", this.CategoryId);
			parameters.Add("brandId", this.BrandId);
			parameters.Add("productType", this.ProductType);
			return parameters;
		}
		#endregion
	}
}
