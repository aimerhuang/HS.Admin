using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.GroupMerchantOrderAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// SAM获取订单接口
	/// </summary>
	public class OrdersStandardGetResponse 
		: YhdResponse 
	{
		/**订单信息列表 */
		[XmlElement("standardOrderList")]
		public StandardOrderInfoList  StandardOrderList{ get; set; }

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
