using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 新增图片空间类别
	/// </summary>
	public class PicSpaceCategorysAddResponse 
		: YhdResponse 
	{
		/**图片类别id */
		[XmlElement("picCategoryId")]
			public long?  PicCategoryId{ get; set; }

		/**新增成功记录数 */
		[XmlElement("updateCount")]
			public int?  UpdateCount{ get; set; }

		/**新增失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
