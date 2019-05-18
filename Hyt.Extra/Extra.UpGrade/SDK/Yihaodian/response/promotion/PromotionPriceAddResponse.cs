using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 新增价格促销
	/// </summary>
	public class PromotionPriceAddResponse 
		: YhdResponse 
	{
		/**成功记录数 */
		[XmlElement("updateCount")]
			public int?  UpdateCount{ get; set; }

		/**失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表，存在错误信息时返回 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

		/**价格促销ID */
		[XmlElement("id")]
			public long?  Id{ get; set; }

	}
}
