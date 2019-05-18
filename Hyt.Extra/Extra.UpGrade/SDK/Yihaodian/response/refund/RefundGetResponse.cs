using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Refund;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 获取退货列表
	/// </summary>
	public class RefundGetResponse 
		: YhdResponse 
	{
		/** 	退货信息列表 */
		[XmlElement("refundList")]
		public RefundList  RefundList{ get; set; }

		/**查询成功记录数 */
		[XmlElement("totalCount")]
			public int?  TotalCount{ get; set; }

		/**查询失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
