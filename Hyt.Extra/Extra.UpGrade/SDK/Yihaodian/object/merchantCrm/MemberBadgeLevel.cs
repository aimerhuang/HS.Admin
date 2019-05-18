using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.MerchantCrm
{
	/// <summary>
	/// 会员级别信息
	/// </summary>
	[Serializable]
	public class MemberBadgeLevel 
	{
		/**卖家会员级别 1：青铜会员 2：白银会员 3：黄金会员 */
		[XmlElement("cur_grade")]
			public string  Cur_grade{ get; set; }

		/**当前会员级别名称：青铜、白银、黄金 */
		[XmlElement("cur_grade_name")]
			public string  Cur_grade_name{ get; set; }

		/**会员级别折扣率：90代表9.0折 */
		[XmlElement("discount")]
			public double?  Discount{ get; set; }

		/**升级到下一个级别的需要的交易额：单位（元） */
		[XmlElement("next_upgrade_amount")]
			public double?  Next_upgrade_amount{ get; set; }

		/**升级到下一个级别的需要的交易量：订单数 */
		[XmlElement("next_upgrade_count")]
			public double?  Next_upgrade_count{ get; set; }

		/**下个会员级别名称：青铜会员、白银会员、黄金会员 */
		[XmlElement("next_grade_name")]
			public string  Next_grade_name{ get; set; }

		/**该等级对应的下一等级1：青铜会员 2：白银会员 3：黄金会员 */
		[XmlElement("next_grade")]
			public string  Next_grade{ get; set; }

	}
}
