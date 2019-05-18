using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询剁手价列表信息
	/// </summary>
	public class UnionPromotionGetRequest 
		: IYhdRequest<UnionPromotionGetResponse> 
	{
		/**每页查询数量，最大为50 */
			public int?  PageSize{ get; set; }

		/**查询开始日期 */
			public string  PromoStartDate{ get; set; }

		/**查询结束日期 */
			public string  PromoEndDate{ get; set; }

		/**当前页码 */
			public int?  CurrentPage{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.promotion.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("pageSize", this.PageSize);
			parameters.Add("promoStartDate", this.PromoStartDate);
			parameters.Add("promoEndDate", this.PromoEndDate);
			parameters.Add("currentPage", this.CurrentPage);
			return parameters;
		}
		#endregion
	}
}
