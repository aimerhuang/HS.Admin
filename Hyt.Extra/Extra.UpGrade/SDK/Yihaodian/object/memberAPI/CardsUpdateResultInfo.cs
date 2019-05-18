using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.MemberAPI
{
	/// <summary>
	/// 会员卡更新结果信息
	/// </summary>
	[Serializable]
	public class CardsUpdateResultInfo 
	{
		/**更新状态（1 成功 0 失败） */
		[XmlElement("updateStatus")]
			public string  UpdateStatus{ get; set; }

		/**会员卡号 */
		[XmlElement("cardNumber")]
			public string  CardNumber{ get; set; }

		/**用户ID */
		[XmlElement("userId")]
			public string  UserId{ get; set; }

		/**错误码 */
		[XmlElement("errorCode")]
			public string  ErrorCode{ get; set; }

		/**错误描述 */
		[XmlElement("errorDes")]
			public string  ErrorDes{ get; set; }

	}
}
