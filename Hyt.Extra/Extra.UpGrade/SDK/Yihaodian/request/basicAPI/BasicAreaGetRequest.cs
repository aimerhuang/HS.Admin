using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取省级市级区级地址列表
	/// </summary>
	public class BasicAreaGetRequest 
		: IYhdRequest<BasicAreaGetResponse> 
	{
		/**父id */
			public long?  ParentId{ get; set; }

		/**level传入表示查询所有省份地址；level传入2和parentId换入省id，查询省下所有城市信息；level传入3和parentId换入市id，查询市下所有区域信息 */
			public int?  Level{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.basic.area.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("parentId", this.ParentId);
			parameters.Add("level", this.Level);
			return parameters;
		}
		#endregion
	}
}
