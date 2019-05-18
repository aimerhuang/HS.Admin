using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Picture;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询图片空间图片信息
	/// </summary>
	public class PicSpacePicsGetResponse 
		: YhdResponse 
	{
		/**图片空间图片列表信息 */
		[XmlElement("merchantPictureSpacePicList")]
		public MerchantPictureSpacePicInfoList  MerchantPictureSpacePicList{ get; set; }

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
