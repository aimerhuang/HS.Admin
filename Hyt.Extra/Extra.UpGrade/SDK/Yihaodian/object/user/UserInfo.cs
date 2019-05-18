using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.User
{
	/// <summary>
	/// 买家用户信息
	/// </summary>
	[Serializable]
	public class UserInfo 
	{
		/**用户id */
		[XmlElement("id")]
			public int?  Id{ get; set; }

		/**用户手机号码 */
		[XmlElement("mobile")]
			public string  Mobile{ get; set; }

		/**用户固定电话号码 */
		[XmlElement("phone")]
			public string  Phone{ get; set; }

		/**用户email地址 */
		[XmlElement("endUserEmail")]
			public string  EndUserEmail{ get; set; }

		/**用户昵称 */
		[XmlElement("nickName")]
			public string  NickName{ get; set; }

	}
}
