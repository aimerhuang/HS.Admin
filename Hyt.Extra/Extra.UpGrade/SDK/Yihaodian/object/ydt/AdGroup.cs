using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Ydt
{
	/// <summary>
	/// 推广组 
	/// </summary>
	[Serializable]
	public class AdGroup 
	{
		/**推广组主人昵称(暂不支持) */
		[XmlElement("nick")]
			public string  Nick{ get; set; }

		/**推广计划Id */
		[XmlElement("campaign_id")]
			public long?  Campaign_id{ get; set; }

		/**推广组id */
		[XmlElement("adgroup_id")]
			public long?  Adgroup_id{ get; set; }

		/**商品类目id，从根类目到子类目，用空格分割 */
		[XmlElement("category_ids")]
			public string  Category_ids{ get; set; }

		/**商品数字id */
		[XmlElement("num_iid")]
			public long?  Num_iid{ get; set; }

		/**默认出价，单位为元，不能小于5 */
		[XmlElement("default_price")]
			public double?  Default_price{ get; set; }

		/**非搜索出价，单位为元，不能小于5 */
		[XmlElement("nonsearch_max_price")]
			public double?  Nonsearch_max_price{ get; set; }

		/**非搜索是否使用默认出价，false-不用；true-使用；默认为true; */
		[XmlElement("is_nonsearch_default_price")]
			public bool  Is_nonsearch_default_price{ get; set; }

		/**用户设置的上下线状态，offline-下线(暂停竞价)；online-上线；默认为online */
		[XmlElement("online_status")]
			public string  Online_status{ get; set; }

		/**online-上线；audit_offline-审核下线；crm_offline-CRM下线；默认为online */
		[XmlElement("offline_type")]
			public string  Offline_type{ get; set; }

		/**审核下线原因 */
		[XmlElement("reason")]
			public string  Reason{ get; set; }

		/**创建时间 */
		[XmlElement("create_time")]
			public string  Create_time{ get; set; }

		/**最后修改时间 */
		[XmlElement("modified_time")]
			public string  Modified_time{ get; set; }

	}
}
