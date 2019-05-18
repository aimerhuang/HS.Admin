using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取产品的销售统计信息
	/// </summary>
	public class DataProdsaleStatisticsGetRequest 
		: IYhdRequest<DataProdsaleStatisticsGetResponse> 
	{
		/**产品ID */
			public long?  ProductId{ get; set; }

		/**查询开始时间 */
			public string  StartTime{ get; set; }

		/**查询结束时间 */
			public string  EndTime{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.data.prodsale.statistics.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("startTime", this.StartTime);
			parameters.Add("endTime", this.EndTime);
			return parameters;
		}
		#endregion
	}
}
