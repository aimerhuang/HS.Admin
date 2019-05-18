using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 取消单个满就送赠品详情促销
	/// </summary>
	public class PromotionFullgiftSingleCancelResponse 
		: YhdResponse 
	{
		/**查询失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

		/**更新成功记录数 */
		[XmlElement("updateCount")]
			public int?  UpdateCount{ get; set; }

	}
}
