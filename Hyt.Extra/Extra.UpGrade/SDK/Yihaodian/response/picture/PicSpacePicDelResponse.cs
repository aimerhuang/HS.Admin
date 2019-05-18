using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 删除图片空间的图片
	/// </summary>
	public class PicSpacePicDelResponse 
		: YhdResponse 
	{
		/**删除成功记录数 */
		[XmlElement("updateCount")]
			public int?  UpdateCount{ get; set; }

		/**删除失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
