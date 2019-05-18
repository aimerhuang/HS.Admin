using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取用户上架在线销售的全部宝贝
	/// </summary>
	public class YdtAdgroupAdgroupOnlineitemsvonGetRequest 
		: IYhdRequest<YdtAdgroupAdgroupOnlineitemsvonGetResponse> 
	{
		/**返回的每页数据量大小,默认200最大200
支持的最大列表长度为：200 */
			public int?  Page_size{ get; set; }

		/**返回的第几页数据，默认为1 从1开始，最大50 */
			public int?  Page_no{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.ydt.adgroup.adgroup.onlineitemsvon.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("page_size", this.Page_size);
			parameters.Add("page_no", this.Page_no);
			return parameters;
		}
		#endregion
	}
}
