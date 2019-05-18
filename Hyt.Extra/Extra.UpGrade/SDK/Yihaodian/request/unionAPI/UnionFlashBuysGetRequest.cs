using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询名品类目下的卖场信息
	/// </summary>
	public class UnionFlashBuysGetRequest 
		: IYhdRequest<UnionFlashBuysGetResponse> 
	{
		/**1号店网盟用户ID */
			public long?  TrackerU{ get; set; }

		/**省份id */
			public long?  ProvinceId{ get; set; }

		/**类目id */
			public long?  CategoryId{ get; set; }

		/**当前页 */
			public int?  CurrentPage{ get; set; }

		/**每页返回数量，默认显示每页最多10条 */
			public int?  PageSize{ get; set; }

		/**在售和预售卖场标志：比如今天8.20号；0标志在售（8.20）； 1表示预售第一天（8.21）；2表示预售第二天（8.22）； 3表示预售第三天（8.23）；4表示预售第三天（8.24）； 获取多天的话传入多个值，以逗号隔开，例如获取8.20和8.21，传“0,1” */
			public string  ActivityType{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.flash.buys.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("trackerU", this.TrackerU);
			parameters.Add("provinceId", this.ProvinceId);
			parameters.Add("categoryId", this.CategoryId);
			parameters.Add("currentPage", this.CurrentPage);
			parameters.Add("pageSize", this.PageSize);
			parameters.Add("activityType", this.ActivityType);
			return parameters;
		}
		#endregion
	}
}
