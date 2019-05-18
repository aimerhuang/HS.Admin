using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查找单个满就送赠品详情促销
	/// </summary>
	public class PromotionFullgiftSingleGetRequest 
		: IYhdRequest<PromotionFullgiftSingleGetResponse> 
	{
		/**促销的id */
			public long?  Id{ get; set; }

		/**满送类型。1：商品   2:分类    3:分类 品牌,4:全场 */
			public int?  FullGiftType{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.promotion.fullgift.single.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("id", this.Id);
			parameters.Add("fullGiftType", this.FullGiftType);
			return parameters;
		}
		#endregion
	}
}
