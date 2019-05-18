using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.User;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 用户登录认证
	/// </summary>
	public class UserAuthGetResponse 
		: YhdResponse 
	{
		/**认证成功记录数 */
		[XmlElement("totalCount")]
			public int?  TotalCount{ get; set; }

		/**认证失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**用户Ticket信息 */
		[XmlElement("ticket")]
		public Ticket  Ticket{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
