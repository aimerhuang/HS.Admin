using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 商家设置会员等级规则
	/// </summary>
	public class CrmGradeSetRequest 
		: IYhdRequest<CrmGradeSetResponse> 
	{
		/**ruleType用来区分设置等级规则是按照订单金额还是订单数量，ruleType=1表示按照订单金额，ruleType=2表示按照订单数量 */
			public int?  RuleType{ get; set; }

		/**必须要在amount和count参数中选择一个，选择amount则ruleType=1，若此时也填写了count则忽略count
amount参数的填写规范：升级到下一个级别的需要的交易额，单位为分,必须全部填写.例如10000,20000,30000，其中10000表示普通会员升级到青铜的所需的交易额，20000表示青铜升级到白银所需的交易额，层级等级中最高等级的下一个等级默认为0。会员等级越高，所需交易额必须越高。 （最大的交易额度不能超过99999999,交易额度目前只支持整形） */
			public string  Amount{ get; set; }

		/**必须要在amount和count参数中选择一个，选择count则ruleType=2，若此时也填写了amount则忽略amount
count参数的填写规范： 升级到下一个级别的需要的交易量,必须全部填写. 以逗号分隔,例如100,200,300，其中100表示普通会员升级到青铜会员交易量。层级等级中最高等级的下一个等级的交易量默认为0。会员等级越高，交易量必须越高。 （最大的交易量不能超过99999999） */
			public string  Count{ get; set; }

		/**会员级别折扣率。会员等级越高，折扣必须越低。 99即9.9折，88折即8.8折。出于安全原因，折扣现最低只能设置到70即7折。 */
			public string  Discount{ get; set; }

		/**表示对应等级是否设置包邮权益，（true,true,true）表示青铜，白银，黄金都拥有免邮权益，（false，true，true）表示白银，黄金拥有免邮权益，青铜没有.（只能填true小写或者是false小写） */
			public string  FreeShipping{ get; set; }

		/**不设置且设置为空串表示没有特殊权益，设置用#号分割，分别对应的等级的特殊服务.分割后为空串也表示对应的等级不设置特殊服务。示例：赠送水杯#赠送礼物#赠送小浣熊:都有特殊服务
#赠送礼物#赠送小浣熊：青铜没有
赠送礼物#赠送小浣熊#：黄金没有
赠送礼物##赠送小浣熊：白银没有 */
			public string  SpecialService{ get; set; }

		/**层级数量（0< levelCount<=5），默认值为3 */
			public int?  LevelCount{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.crm.grade.set";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("ruleType", this.RuleType);
			parameters.Add("amount", this.Amount);
			parameters.Add("count", this.Count);
			parameters.Add("discount", this.Discount);
			parameters.Add("freeShipping", this.FreeShipping);
			parameters.Add("specialService", this.SpecialService);
			parameters.Add("levelCount", this.LevelCount);
			return parameters;
		}
		#endregion
	}
}
