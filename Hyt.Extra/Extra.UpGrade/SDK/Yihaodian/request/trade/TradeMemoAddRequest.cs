using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 对一笔交易添加备注(兼容淘宝) 
	/// </summary>
	public class TradeMemoAddRequest 
		: IYhdRequest<TradeMemoAddResponse> 
	{
		/**交易编号 */
			public long?  Tid{ get; set; }

		/**交易备注。最大长度: 1000个字节 */
			public string  Memo{ get; set; }

		/**(暂不提供)交易备注旗帜，可选值为：0(灰色), 1(红色), 2(黄色), 3(绿色), 4(蓝色), 5(粉红色)，默认值为0 */
			public long?  Flag{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.trade.memo.add";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("tid", this.Tid);
			parameters.Add("memo", this.Memo);
			parameters.Add("flag", this.Flag);
			return parameters;
		}
		#endregion
	}
}
