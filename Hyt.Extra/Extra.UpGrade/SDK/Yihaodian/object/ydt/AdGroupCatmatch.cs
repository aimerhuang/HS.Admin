using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Ydt
{
	/// <summary>
	/// 推广组类目出价
	/// </summary>
	[Serializable]
	public class AdGroupCatmatch 
	{
		/**推广组主人昵称 */
		[XmlElement("nick")]
			public string  Nick{ get; set; }

		/**推广计划Id */
		[XmlElement("campaign_id")]
			public long?  Campaign_id{ get; set; }

		/**推广组id */
		[XmlElement("adgroup_id")]
			public long?  Adgroup_id{ get; set; }

		/**类目出价Id */
		[XmlElement("catmatch_id")]
			public long?  Catmatch_id{ get; set; }

		/**类目出价 */
		[XmlElement("max_price")]
			public double?  Max_price{ get; set; }

		/**是否使用推广组默认出价，false-不使用默认出价 true-使用默认出价；默认true */
		[XmlElement("is_default_price")]
			public bool  Is_default_price{ get; set; }

		/**是否启用类目出价；offline-不启用；online-启用；默认启用 */
		[XmlElement("online_status")]
			public string  Online_status{ get; set; }

		/**创建时间 */
		[XmlElement("create_time")]
			public string  Create_time{ get; set; }

		/**最后修改时间 */
		[XmlElement("modified_time")]
			public string  Modified_time{ get; set; }

		/**类目出价质量得分 */
		[XmlElement("qscore")]
			public string  Qscore{ get; set; }

	}
}
