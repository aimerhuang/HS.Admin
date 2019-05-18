using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 包邮商品信息接口 
	/// </summary>
	public class UnionFreeDeliveryGetRequest 
		: IYhdRequest<UnionFreeDeliveryGetResponse> 
	{
		/**联盟用户ID */
			public long?  TrackU{ get; set; }

		/**返回字段，可选 */
			public string  Fields{ get; set; }

		/**查询关键字 */
			public string  Keyword{ get; set; }

		/**类目id */
			public long?  Cid{ get; set; }

		/**价格下限 */
			public double?  StartPrice{ get; set; }

		/**价格上限 */
			public double?  EndPrice{ get; set; }

		/**页码 */
			public int?  PageNo{ get; set; }

		/**每页条数 */
			public int?  PageSize{ get; set; }

		/**保留字段 */
			public string  Ext1{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.free.delivery.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("trackU", this.TrackU);
			parameters.Add("fields", this.Fields);
			parameters.Add("keyword", this.Keyword);
			parameters.Add("cid", this.Cid);
			parameters.Add("startPrice", this.StartPrice);
			parameters.Add("endPrice", this.EndPrice);
			parameters.Add("pageNo", this.PageNo);
			parameters.Add("pageSize", this.PageSize);
			parameters.Add("ext1", this.Ext1);
			return parameters;
		}
		#endregion
	}
}
