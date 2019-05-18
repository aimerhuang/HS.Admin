using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询普通产品信息
	/// </summary>
	public class GeneralProductsSearchRequest 
		: IYhdRequest<GeneralProductsSearchResponse> 
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

		/**产品Id列表(最多productId个数为100,与outerIdList二选一，两个都填，默认为prodcuctIdList) */
			public string  ProductIdList{ get; set; }

		/**outerId列表(最多outerId个数为100，与productIdList二选一，两个都填，默认为prodcuctIdList) */
			public string  OuterIdList{ get; set; }

		/**审核状态[1.未审核(新增未审核,编辑待审核);2.审核通过(审核通过);3.审核失败(审核未通过,图片审核失败,文描审核失败)] */
			public int?  VerifyFlg{ get; set; }

		/**产品类别Id */
			public long?  CategoryId{ get; set; }

		/**产品类别类型（0:1号店类别,1:商家自定义类别,默认为0） */
			public int?  CategoryType{ get; set; }

		/**品牌Id */
			public long?  BrandId{ get; set; }

		/**产品编码列表（逗号分隔）与productIdList、outerIdList三选一,最大长度为100,优先级最低 */
			public string  ProductCodeList{ get; set; }

		/**根据更新时间过滤，起始时间 */
			public string  UpdateStartTime{ get; set; }

		/**根据更新时间过滤，终止时间 */
			public string  UpdateEndTime{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.general.products.search";
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
			parameters.Add("outerIdList", this.OuterIdList);
			parameters.Add("verifyFlg", this.VerifyFlg);
			parameters.Add("categoryId", this.CategoryId);
			parameters.Add("categoryType", this.CategoryType);
			parameters.Add("brandId", this.BrandId);
			parameters.Add("productCodeList", this.ProductCodeList);
			parameters.Add("updateStartTime", this.UpdateStartTime);
			parameters.Add("updateEndTime", this.UpdateEndTime);
			return parameters;
		}
		#endregion
	}
}
