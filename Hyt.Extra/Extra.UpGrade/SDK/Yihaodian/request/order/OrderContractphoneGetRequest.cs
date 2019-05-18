using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询合约机订单查询接口
	/// </summary>
	public class OrderContractphoneGetRequest 
		: IYhdRequest<OrderContractphoneGetResponse> 
	{
		/**合约机创建开始时间 */
			public string  StartTime{ get; set; }

		/**合约机创建结束时间 */
			public string  EndTime{ get; set; }

		/**订单列表。多个订单好用“,”隔开 */
			public string  OrderCodeList{ get; set; }

		/**机主姓名 */
			public string  HostName{ get; set; }

		/**客户选的手机号 */
			public string  MobilePhone{ get; set; }

		/**合约机资料审核状态：0（初始化）， 1（后台确认）， 2（后台取消） ，3（前台取消），4（ 已完成），5（商家确认），6（商家取消） */
			public int?  InformationStatus{ get; set; }

		/**最小50，最大100，默认100 */
			public int?  PageRows{ get; set; }

		/**页码，默认值1 */
			public int?  CurPage{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.order.contractphone.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("startTime", this.StartTime);
			parameters.Add("endTime", this.EndTime);
			parameters.Add("orderCodeList", this.OrderCodeList);
			parameters.Add("hostName", this.HostName);
			parameters.Add("mobilePhone", this.MobilePhone);
			parameters.Add("informationStatus", this.InformationStatus);
			parameters.Add("pageRows", this.PageRows);
			parameters.Add("curPage", this.CurPage);
			return parameters;
		}
		#endregion
	}
}
