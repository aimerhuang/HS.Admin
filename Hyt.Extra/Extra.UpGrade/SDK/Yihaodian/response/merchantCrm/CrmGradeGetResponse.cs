using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.MerchantCrm;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 商家店铺的会员等级规则查询
	/// </summary>
	public class CrmGradeGetResponse 
		: YhdResponse 
	{
		/**商家设置的会员等级信息 */
		[XmlElement("member_badge_level_list")]
		public MemberBadgeLevelList  Member_badge_level_list{ get; set; }

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
