using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 当当物流推送接口
	/// </summary>
	public class OrderLogisticsPushRequest 
		: IYhdRequest<OrderLogisticsPushResponse> 
	{
		/**批量操作支持，最多50。以json格式输入。{"orderLogisticsInfoList": {"orderStepInfo": [{"orderCode": "130918P3SFBT","expressNbr": "9638527415","stepInfoList": {"stepInfo": [{"content": "22222","status": "333","operator": "jxw","operatorDate": "2013-12-26 9:11:34","remark": "333333333"}]}},{"orderCode": "130308W3HN31","expressNbr": "111111111111","stepInfoList": {"stepInfo": [{"content": "22222","status": "333","operator": "jxw","operatorDate": "2013-12-26 9:11:34","remark": "333333333"}]}}]}} */
			public string  OrderLogisticsInfoList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.order.logistics.push";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("orderLogisticsInfoList", this.OrderLogisticsInfoList);
			return parameters;
		}
		#endregion
	}
}
