using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// JD错误日志回调插入接口
	/// </summary>
	public class FinanceCallbackErrorlogAddResponse 
		: YhdResponse 
	{
		/**成功与否（true 表示成功） */
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
