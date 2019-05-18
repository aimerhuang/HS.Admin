using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 评论回复
	/// </summary>
	public class ReviewAddexplainRequest 
		: IYhdRequest<ReviewAddexplainResponse> 
	{
		/**评论id */
			public long?  ReviewId{ get; set; }

		/**评论回复内容，限制字数300 */
			public string  Content{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.review.addexplain";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("reviewId", this.ReviewId);
			parameters.Add("content", this.Content);
			return parameters;
		}
		#endregion
	}
}
