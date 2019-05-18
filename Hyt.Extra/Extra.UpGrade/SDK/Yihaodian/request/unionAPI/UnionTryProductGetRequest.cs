using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询试用中心商品信息
	/// </summary>
	public class UnionTryProductGetRequest 
		: IYhdRequest<UnionTryProductGetResponse> 
	{
		/**联盟用户ID */
			public long?  TrackerU{ get; set; }

		/**返回字段，可选 */
			public string  Fields{ get; set; }

		/**网站ID，预留字段，暂时不使用 */
			public string  WebsiteId{ get; set; }

		/**用户ID，预留字段 */
			public string  Uid{ get; set; }

		/**省份ID，当为空时，默认上海Id */
			public long?  ProvinceId{ get; set; }

		/**模糊查询名称，只能为一个关键字，匹配0元购商品名称 */
			public string  Keyword{ get; set; }

		/**0元购商品所属的末级类目 */
			public long?  CategoryId{ get; set; }

		/**1：1号店自营商品，2：1号店商城商品，默认混合在一起。 */
			public int?  SiteType{ get; set; }

		/**1：付邮试用。2：免费试用。默认混合在一起。 */
			public int?  SiteTryType{ get; set; }

		/**页码。结果页1-99（默认为1） */
			public int?  PageNo{ get; set; }

		/**每页最大条数（每页显示记录数，默认50，最大50） */
			public int?  PageSize{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.try.product.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("trackerU", this.TrackerU);
			parameters.Add("fields", this.Fields);
			parameters.Add("websiteId", this.WebsiteId);
			parameters.Add("uid", this.Uid);
			parameters.Add("provinceId", this.ProvinceId);
			parameters.Add("keyword", this.Keyword);
			parameters.Add("categoryId", this.CategoryId);
			parameters.Add("siteType", this.SiteType);
			parameters.Add("siteTryType", this.SiteTryType);
			parameters.Add("pageNo", this.PageNo);
			parameters.Add("pageSize", this.PageSize);
			return parameters;
		}
		#endregion
	}
}
