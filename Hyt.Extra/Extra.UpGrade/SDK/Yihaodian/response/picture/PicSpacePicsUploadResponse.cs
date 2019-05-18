using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 批量上传图片到图片空间
	/// </summary>
	public class PicSpacePicsUploadResponse 
		: YhdResponse 
	{
		/**图片ID列表(逗号分隔) */
		[XmlElement("picIdList")]
			public string  PicIdList{ get; set; }

		/**图片url地址列表(逗号分隔) */
		[XmlElement("picUrlList")]
			public string  PicUrlList{ get; set; }

		/**图片空间id列表(逗号分隔,图片空间的图片标识,用于图片空间中图片的新增，查询，修改，删除操作) */
		[XmlElement("picSpaceIdList")]
			public string  PicSpaceIdList{ get; set; }

		/**更新成功记录数 */
		[XmlElement("updateCount")]
			public int?  UpdateCount{ get; set; }

		/**查询失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
