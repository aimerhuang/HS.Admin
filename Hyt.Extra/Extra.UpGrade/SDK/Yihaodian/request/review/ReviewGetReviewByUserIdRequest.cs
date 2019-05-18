using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询用户评论信息
	/// </summary>
	public class ReviewGetReviewByUserIdRequest 
		: IYhdRequest<ReviewGetReviewByUserIdResponse> 
	{
		/**用户id */
			public long?  UserId{ get; set; }

		/**页码。最大限制值200； */
			public int?  PageNo{ get; set; }

		/**每页获取条数；取值范围：大于0的整数；最大限制100； */
			public int?  PageSize{ get; set; }

		/**是否启用has_next的分页方式，如果指定true,则返回的结果中不包含总记录数，但是会新增一个是否存在下一页的的字段,通过此种方式获取评价信息，效率在原有的基础上有大幅的提升。 */
			public bool  UseHasNext{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.review.getReviewByUserId";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("userId", this.UserId);
			parameters.Add("pageNo", this.PageNo);
			parameters.Add("pageSize", this.PageSize);
			parameters.Add("useHasNext", this.UseHasNext);
			return parameters;
		}
		#endregion
	}
}
