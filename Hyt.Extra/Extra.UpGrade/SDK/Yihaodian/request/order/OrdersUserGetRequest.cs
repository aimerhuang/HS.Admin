using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取用户的订单信息
	/// </summary>
	public class OrdersUserGetRequest 
		: IYhdRequest<OrdersUserGetResponse> 
	{
		/**用户ID */
			public long?  EndUserId{ get; set; }

		/**订单创建开始时间（yyyy-MM-dd HH:mm:ss） */
			public string  StartTime{ get; set; }

		/**订单创建结束时间（yyyy-MM-dd HH:mm:ss） */
			public string  EndTime{ get; set; }

		/**当前页数（默认1） */
			public int?  CurPage{ get; set; }

		/**每页显示记录数，默认50，最大100 */
			public int?  PageRows{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.orders.user.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("endUserId", this.EndUserId);
			parameters.Add("startTime", this.StartTime);
			parameters.Add("endTime", this.EndTime);
			parameters.Add("curPage", this.CurPage);
			parameters.Add("pageRows", this.PageRows);
			return parameters;
		}
		#endregion
	}
}
