using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Sby;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// ISV流量报表
	/// </summary>
	public class SbyFuwuApiInvokeReportGetResponse 
		: YhdResponse 
	{
		/**调用详情 */
		[XmlElement("appMerchantSummaryCost")]
		public AppMerchantSummaryCost  AppMerchantSummaryCost{ get; set; }

		/**成功记录数 */
		[XmlElement("totalCount")]
			public int?  TotalCount{ get; set; }

		/**失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
