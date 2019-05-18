using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 更新交易单信息
	/// </summary>
	public class OldfornewTransactionBillUpdateResponse 
		: YhdResponse 
	{
		/**0、失败，1、成功 */
		[XmlElement("result")]
			public int?  Result{ get; set; }

		/**备注信息 */
		[XmlElement("remark")]
			public string  Remark{ get; set; }

		/**错误码 */
		[XmlElement("error_code")]
			public string  Error_code{ get; set; }

		/**错误描述 */
		[XmlElement("msg")]
			public string  Msg{ get; set; }

		/**子错误码 */
		[XmlElement("sub_code")]
			public string  Sub_code{ get; set; }

		/**子错误描述 */
		[XmlElement("sub_msg")]
			public string  Sub_msg{ get; set; }

	}
}
