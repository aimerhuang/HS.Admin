using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 取消消息服务
	/// </summary>
	public class MsgUserCancelRequest 
		: IYhdRequest<MsgUserCancelResponse> 
	{
		/**消息主题列表，传多个的话用半角逗号分隔。不设置表示取消应用app所订阅的所有topic，一般情况建议不要设置。 */
			public string  Topics{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.msg.user.cancel";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("topics", this.Topics);
			return parameters;
		}
		#endregion
	}
}
