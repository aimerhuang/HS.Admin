using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 修改一笔交易备注（兼容淘宝） 
	/// </summary>
	public class TradeMemoUpdateRequest 
		: IYhdRequest<TradeMemoUpdateResponse> 
	{
		/** 交易编号 */
			public long?  Tid{ get; set; }

		/**交易备注。最大长度: 1000个字节 */
			public string  Memo{ get; set; }

		/**	 交易备注旗帜，可选值为：0(灰色), 1(红色), 2(黄色), 3(绿色), 4(蓝色), 5(粉红色)，默认值为0(暂不提供) */
			public long?  Flag{ get; set; }

		/**是否对memo的值置空 若为true，则不管传入的memo字段的值是否为空，都将会对已有的memo值清空，慎用； 若用false，则会根据memo是否为空来修改memo的值：若memo为空则忽略对已有memo字段的修改，若memo非空，则使用新传入的memo覆盖已有的memo的值（暂不提供） */
			public bool  Reset{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.trade.memo.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("tid", this.Tid);
			parameters.Add("memo", this.Memo);
			parameters.Add("flag", this.Flag);
			parameters.Add("reset", this.Reset);
			return parameters;
		}
		#endregion
	}
}
