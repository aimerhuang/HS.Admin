using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询评论信息
	/// </summary>
	public class ReviewGetreviewRequest 
		: IYhdRequest<ReviewGetreviewResponse> 
	{
		/**产品id */
			public long?  ProductId{ get; set; }

		/**订单id（拆单则是子单） */
			public long?  OrderId{ get; set; }

		/**评价创建开始时间 */
			public string  StartTime{ get; set; }

		/**评价创建结束时间 */
			public string  EndTime{ get; set; }

		/**评价结果;可选值:good(好评),neutral(中评),bad(差评) */
			public string  Result{ get; set; }

		/**页码。最大限制值200； */
			public int?  PageNo{ get; set; }

		/**每页获取条数；取值范围：大于0的整数；最大限制100； */
			public int?  PageSize{ get; set; }

		/**是否启用has_next的分页方式，如果指定true,则返回的结果中不包含总记录数，但是会新增一个是否存在下一页的的字段,通过此种方式获取评价信息，效率在原有的基础上有大幅的提升。 */
			public bool  UseHasNext{ get; set; }

		/**以英文逗号分隔,订单id列表，最多100个 */
			public string  OrderIdList{ get; set; }

		/**评价更新开始时间 */
			public string  UpdateStartTime{ get; set; }

		/**评价更新结束时间 */
			public string  UpdateEndTime{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.review.getreview";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("orderId", this.OrderId);
			parameters.Add("startTime", this.StartTime);
			parameters.Add("endTime", this.EndTime);
			parameters.Add("result", this.Result);
			parameters.Add("pageNo", this.PageNo);
			parameters.Add("pageSize", this.PageSize);
			parameters.Add("useHasNext", this.UseHasNext);
			parameters.Add("orderIdList", this.OrderIdList);
			parameters.Add("updateStartTime", this.UpdateStartTime);
			parameters.Add("updateEndTime", this.UpdateEndTime);
			return parameters;
		}
		#endregion
	}
}
