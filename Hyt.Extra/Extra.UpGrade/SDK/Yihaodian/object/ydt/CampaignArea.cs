using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Ydt
{
	/// <summary>
	/// 计划地域
	/// </summary>
	[Serializable]
	public class CampaignArea 
	{
		/**计划id */
		[XmlElement("campaign_id")]
			public long?  Campaign_id{ get; set; }

		/**计划定向推广地域，以逗号隔开,全部就是“all” */
		[XmlElement("area")]
			public string  Area{ get; set; }

		/**计划创建时间 */
		[XmlElement("create_time")]
			public string  Create_time{ get; set; }

		/**计划更新时间 */
		[XmlElement("modified_time")]
			public string  Modified_time{ get; set; }

	}
}
