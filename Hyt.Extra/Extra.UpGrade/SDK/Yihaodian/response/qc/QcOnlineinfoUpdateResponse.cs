using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 反馈质检订单和资质质检结果
	/// </summary>
	public class QcOnlineinfoUpdateResponse 
		: YhdResponse 
	{
		/**处理结果 */
		[XmlElement("countResult")]
			public int?  CountResult{ get; set; }

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
