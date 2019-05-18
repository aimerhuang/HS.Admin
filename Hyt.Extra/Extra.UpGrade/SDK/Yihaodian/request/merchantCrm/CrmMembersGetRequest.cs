using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取卖家的会员（基本查询）
	/// </summary>
	public class CrmMembersGetRequest 
		: IYhdRequest<CrmMembersGetResponse> 
	{
		/**会员名称,用户在一号店的唯一标识。不支持模糊匹配查询，一旦输入，将优先该条件查询 */
			public string  CustomerPin{ get; set; }

		/**会员等级：0：普通，1：青铜，2：白银，3：黄金 */
			public int?  Grade{ get; set; }

		/**最小交易额，单位为元 */
			public double?  MinTradeAmount{ get; set; }

		/**最大交易额，单位为元 */
			public double?  MaxTradeAmount{ get; set; }

		/**最小交易量 */
			public int?  MinTradeCount{ get; set; }

		/**最大交易量 */
			public int?  MaxTradeCount{ get; set; }

		/**开始时间，精确至年月日 */
			public string  MinLastTradeTime{ get; set; }

		/**截止时间，精确至年月日 */
			public string  MaxLastTradeTime{ get; set; }

		/**当前页码数，如果不如输入，默认为1，如果输入以实际输入为准 */
			public int?  CurrentPage{ get; set; }

		/**每页显示的会员数量,如果不如输入，默认为10，如果输入以实际输入为准，如果大于100，以100为准 */
			public int?  PageSize{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.crm.members.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("customerPin", this.CustomerPin);
			parameters.Add("grade", this.Grade);
			parameters.Add("minTradeAmount", this.MinTradeAmount);
			parameters.Add("maxTradeAmount", this.MaxTradeAmount);
			parameters.Add("minTradeCount", this.MinTradeCount);
			parameters.Add("maxTradeCount", this.MaxTradeCount);
			parameters.Add("minLastTradeTime", this.MinLastTradeTime);
			parameters.Add("maxLastTradeTime", this.MaxLastTradeTime);
			parameters.Add("currentPage", this.CurrentPage);
			parameters.Add("pageSize", this.PageSize);
			return parameters;
		}
		#endregion
	}
}
