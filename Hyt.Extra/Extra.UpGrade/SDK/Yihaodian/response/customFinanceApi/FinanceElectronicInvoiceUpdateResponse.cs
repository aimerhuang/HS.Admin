using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// JD回传从航信获得的电子发票信息
	/// </summary>
	public class FinanceElectronicInvoiceUpdateResponse 
		: YhdResponse 
	{
		/**成功与否 */
		[XmlElement("isSuccess")]
			public bool  IsSuccess{ get; set; }

		/**错误的话，返回错误信息 */
		[XmlElement("message")]
			public string  Message{ get; set; }

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
