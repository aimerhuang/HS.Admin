using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.MerchantSettleAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 此接口用来查询商家指定日期当日账单明细，查询指定页数，一页1000笔
	/// </summary>
	public class SettleBillDetailGetResponse 
		: YhdResponse 
	{
		/**账单明细 */
		[XmlElement("resultList")]
		public MerchantSettleBillList  ResultList{ get; set; }

		/**错误码 */
		[XmlElement("errorCode")]
			public string  ErrorCode{ get; set; }

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
