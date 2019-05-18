using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 商家获取收藏列表
	/// </summary>
	public class FavoriteSearchRequest 
		: IYhdRequest<FavoriteSearchResponse> 
	{
		/**0、商品收藏 1、店铺收藏 */
			public long?  FavoriteType{ get; set; }

		/**页码 */
			public int?  PageNo{ get; set; }

		/**每页条数不传或者大于20 默认20条查询 */
			public int?  PageSize{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.favorite.search";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("favoriteType", this.FavoriteType);
			parameters.Add("pageNo", this.PageNo);
			parameters.Add("pageSize", this.PageSize);
			return parameters;
		}
		#endregion
	}
}
