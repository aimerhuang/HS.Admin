using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询水晶商家数据
	/// </summary>
	public class DataCrystalGetRequest 
		: IYhdRequest<DataCrystalGetResponse> 
	{
		/**查询开始日期【最长查询31天】 */
			public string  StartDateStr{ get; set; }

		/**查询结束日期【最长查询31天】 */
			public string  EndDateStr{ get; set; }

		/**是否促销(0：全部，1：促销，2：非促销) */
			public string  IsPromotion{ get; set; }

		/**终端类型(0：全部，1：pc，2：无线) */
			public string  TerminalType{ get; set; }

		/**指标(all：所有，sale：销量，flow：流量，fav：收藏；多个指标以","隔开，比如传入"flow,sale") */
			public string  Target{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.data.crystal.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("startDateStr", this.StartDateStr);
			parameters.Add("endDateStr", this.EndDateStr);
			parameters.Add("isPromotion", this.IsPromotion);
			parameters.Add("terminalType", this.TerminalType);
			parameters.Add("target", this.Target);
			return parameters;
		}
		#endregion
	}
}
