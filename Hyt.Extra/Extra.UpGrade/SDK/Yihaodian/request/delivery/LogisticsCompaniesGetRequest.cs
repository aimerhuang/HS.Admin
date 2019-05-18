using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询物流公司信息（兼容淘宝）
	/// </summary>
	public class LogisticsCompaniesGetRequest 
		: IYhdRequest<LogisticsCompaniesGetResponse> 
	{
		/**需返回的字段列表。可选值:LogisticCompany 结构中的所有字段;多个字段间用","逗号隔开. 如:id,code,name,reg_mail_no 说明： id：物流公司ID code：物流公司code name：物流公司名称 reg_mail_no：物流公司对应的运单规则 */
			public string  Fields{ get; set; }

		/**是否查询推荐物流公司.可选值:true,false.如果不提供此参数,将会返回所有支持电话联系的物流公司.  */
			public bool  IsRecommended{ get; set; }

		/**推荐物流公司的下单方式. */
			public string  OrderMode{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.logistics.companies.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("fields", this.Fields);
			parameters.Add("isRecommended", this.IsRecommended);
			parameters.Add("orderMode", this.OrderMode);
			return parameters;
		}
		#endregion
	}
}
