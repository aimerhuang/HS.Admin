using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	/// <summary>
	/// 用户身份信息
	/// </summary>
	[Serializable]
	public class UserInfo 
	{
		/**用户id */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**用户名 */
		[XmlElement("endUserName")]
			public string  EndUserName{ get; set; }

		/**邮箱 */
		[XmlElement("endUserEmail")]
			public string  EndUserEmail{ get; set; }

		/**手机号 */
		[XmlElement("mobile")]
			public string  Mobile{ get; set; }

		/**电话 */
		[XmlElement("phone")]
			public string  Phone{ get; set; }

		/**用户真实姓名 */
		[XmlElement("endUserRealName")]
			public string  EndUserRealName{ get; set; }

		/**身份证号 */
		[XmlElement("identityCard")]
			public string  IdentityCard{ get; set; }

		/**身份证正面照片url */
		[XmlElement("identityCardFrontPic")]
			public string  IdentityCardFrontPic{ get; set; }

		/**身份证反面照片url */
		[XmlElement("identityCardBackPic")]
			public string  IdentityCardBackPic{ get; set; }

	}
}
