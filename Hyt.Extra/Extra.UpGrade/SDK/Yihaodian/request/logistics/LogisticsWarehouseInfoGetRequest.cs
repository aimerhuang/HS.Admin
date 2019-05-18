using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取仓库信息
	/// </summary>
	public class LogisticsWarehouseInfoGetRequest 
		: IYhdRequest<LogisticsWarehouseInfoGetResponse> 
	{
		/**仓库ID */
			public long?  WarehouseId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.logistics.warehouse.info.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("warehouseId", this.WarehouseId);
			return parameters;
		}
		#endregion
	}
}
