using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询名品卖场下所有的商品信息
	/// </summary>
	public class UnionFlashProductlistGetRequest 
		: IYhdRequest<UnionFlashProductlistGetResponse> 
	{
		/**联盟用户ID */
			public long?  TrackerU{ get; set; }

		/**省份id */
			public long?  ProvinceId{ get; set; }

		/**卖场id */
			public long?  ActivityId{ get; set; }

		/**结果排序字段：11价格低到高 ，12价格高到低 ，21折扣低到高，22折扣高到低   ，0或者null或者不传  表示后台设置的默认排序 */
			public string  SortType{ get; set; }

		/**当前页 */
			public int?  CurrentPage{ get; set; }

		/**每页返回数量，最多10条 */
			public int?  PageSize{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.flash.productlist.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("trackerU", this.TrackerU);
			parameters.Add("provinceId", this.ProvinceId);
			parameters.Add("activityId", this.ActivityId);
			parameters.Add("sortType", this.SortType);
			parameters.Add("currentPage", this.CurrentPage);
			parameters.Add("pageSize", this.PageSize);
			return parameters;
		}
		#endregion
	}
}
