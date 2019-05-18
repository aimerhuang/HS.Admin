using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询单个满减促销详情
	/// </summary>
	public class PromotionFullminusDetailGetRequest 
		: IYhdRequest<PromotionFullminusDetailGetResponse> 
	{
		/**促销的id */
			public long?  Id{ get; set; }

		/**		满减类型1：商品   2:分类   3:分类 品牌,4:全场	 */
			public int?  FullMinusType{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.promotion.fullminus.detail.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("id", this.Id);
			parameters.Add("fullMinusType", this.FullMinusType);
			return parameters;
		}
		#endregion
	}
}
