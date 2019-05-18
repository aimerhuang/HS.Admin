using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取抢购商品
	/// </summary>
	public class UnionQianggouProductGetRequest 
		: IYhdRequest<UnionQianggouProductGetResponse> 
	{
		/**省份ID */
			public long?  ProvinceId{ get; set; }

		/**页码数 */
			public int?  PageNO{ get; set; }

		/**网盟trackerU */
			public long?  TrackerU{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.qianggou.product.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("provinceId", this.ProvinceId);
			parameters.Add("pageNO", this.PageNO);
			parameters.Add("trackerU", this.TrackerU);
			return parameters;
		}
		#endregion
	}
}
