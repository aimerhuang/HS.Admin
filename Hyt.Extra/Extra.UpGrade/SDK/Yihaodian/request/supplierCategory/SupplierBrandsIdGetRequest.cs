using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量获取新品品牌名称列表（含ID）
	/// </summary>
	public class SupplierBrandsIdGetRequest 
		: IYhdRequest<SupplierBrandsIdGetResponse> 
	{
		/**当前页，默认为1 */
			public int?  CurPage{ get; set; }

		/**每页数量，默认50，最大100 */
			public int?  PageRows{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.supplier.brands.id.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("curPage", this.CurPage);
			parameters.Add("pageRows", this.PageRows);
			return parameters;
		}
		#endregion
	}
}
