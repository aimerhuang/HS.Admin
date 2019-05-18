using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量操作更新sam会员卡状态及sam卡号
	/// </summary>
	public class MembersCardSetRequest 
		: IYhdRequest<MembersCardSetResponse> 
	{
		/**sam会员卡信息 */
			public string  CardInfoListString{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.members.card.set";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("cardInfoListString", this.CardInfoListString);
			return parameters;
		}
		#endregion
	}
}
