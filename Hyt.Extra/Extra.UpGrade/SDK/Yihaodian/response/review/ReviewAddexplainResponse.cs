using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 评论回复
	/// </summary>
	public class ReviewAddexplainResponse 
		: YhdResponse 
	{
		/**解释成功true；失败false； */
		[XmlElement("is_success")]
			public bool  Is_success{ get; set; }

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
