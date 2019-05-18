using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 验证用户购买的服务是否可用
	/// </summary>
	public class SbyUserappCheckResponse 
		: YhdResponse 
	{
		/**验证成功 */
		[XmlElement("totalCount")]
			public int?  TotalCount{ get; set; }

		/**验证失败 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
