using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.MemberAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 批量操作更新sam会员卡状态及sam卡号
	/// </summary>
	public class MembersCardSetResponse 
		: YhdResponse 
	{
		/**更新是否全部成功(1 成功 0 失败) */
		[XmlElement("isSuccess")]
			public string  IsSuccess{ get; set; }

		/**卡信息更新返回结果 */
		[XmlElement("cardsUpdateResultInfoList")]
		public CardsUpdateResultInfoList  CardsUpdateResultInfoList{ get; set; }

		/**成功记录数 */
		[XmlElement("updateCount")]
			public int?  UpdateCount{ get; set; }

		/**失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
