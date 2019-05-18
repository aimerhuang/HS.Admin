using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询金牌秒杀列表信息
	/// </summary>
	public class UnionGrouponHourbuyGetRequest 
		: IYhdRequest<UnionGrouponHourbuyGetResponse> 
	{
		/**查询开始日期 */
			public string  StartDate{ get; set; }

		/**查询区间（最大为3） */
			public int?  DateCount{ get; set; }

		/**当前页码 */
			public int?  CurrentPage{ get; set; }

		/**每页查询数量，最大为50 */
			public int?  PageSize{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.groupon.hourbuy.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("startDate", this.StartDate);
			parameters.Add("dateCount", this.DateCount);
			parameters.Add("currentPage", this.CurrentPage);
			parameters.Add("pageSize", this.PageSize);
			return parameters;
		}
		#endregion
	}
}
