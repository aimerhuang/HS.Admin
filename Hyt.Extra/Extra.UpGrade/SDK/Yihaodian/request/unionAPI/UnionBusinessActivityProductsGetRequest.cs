using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询商家活动详情产品信息接口
	/// </summary>
	public class UnionBusinessActivityProductsGetRequest 
		: IYhdRequest<UnionBusinessActivityProductsGetResponse> 
	{
		/**联盟用户ID */
			public long?  TrackerU{ get; set; }

		/**返回字段，可选 */
			public string  Fields{ get; set; }

		/**商家活动id */
			public long?  ActivityId{ get; set; }

		/**模糊匹配商品名称 */
			public string  Keyword{ get; set; }

		/**排序字段只有空、佣金比例(comm_rate)和佣金(comm)个值，默认按照佣金率降序排序 */
			public string  Sort{ get; set; }

		/**当前页，从1开始 */
			public int?  CurPage{ get; set; }

		/**每页数量，1--40之间，默认40 */
			public int?  PageNum{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.business.activity.products.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("trackerU", this.TrackerU);
			parameters.Add("fields", this.Fields);
			parameters.Add("activityId", this.ActivityId);
			parameters.Add("keyword", this.Keyword);
			parameters.Add("sort", this.Sort);
			parameters.Add("curPage", this.CurPage);
			parameters.Add("pageNum", this.PageNum);
			return parameters;
		}
		#endregion
	}
}
