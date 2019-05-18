using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Ydt
{
	/// <summary>
	/// 计划日限额
	/// </summary>
	[Serializable]
	public class CampaignBudget 
	{
		/**计划id */
		[XmlElement("campaign_id")]
			public long?  Campaign_id{ get; set; }

		/**日限额，单位是元，不得小于20;0表示无限额。 */
		[XmlElement("budget")]
			public double?  Budget{ get; set; }

		/**计划创建时间 */
		[XmlElement("create_time")]
			public string  Create_time{ get; set; }

		/**计划更新时间 */
		[XmlElement("modified_time")]
			public string  Modified_time{ get; set; }

	}
}
