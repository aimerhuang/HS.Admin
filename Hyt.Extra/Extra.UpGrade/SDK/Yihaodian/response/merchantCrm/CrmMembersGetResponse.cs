using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.MerchantCrm;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 获取卖家的会员（基本查询）
	/// </summary>
	public class CrmMembersGetResponse 
		: YhdResponse 
	{
		/**记录的总条数 */
		[XmlElement("total_result")]
			public long?  Total_result{ get; set; }

		/**根据一定条件查询的卖家会员 */
		[XmlElement("crm_member_result")]
		public CrmMemberList  Crm_member_result{ get; set; }

		/**错误码 */
		[XmlElement("error_code")]
			public string  Error_code{ get; set; }

		/**子错误描述 */
		[XmlElement("sub_msg")]
			public string  Sub_msg{ get; set; }

		/**错误描述 */
		[XmlElement("msg")]
			public string  Msg{ get; set; }

		/**子错误码 */
		[XmlElement("sub_code")]
			public string  Sub_code{ get; set; }

	}
}
