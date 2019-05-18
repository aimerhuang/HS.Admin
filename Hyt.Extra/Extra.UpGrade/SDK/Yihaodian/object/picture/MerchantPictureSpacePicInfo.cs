using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Picture
{
	/// <summary>
	/// 商家图片空间图片信息
	/// </summary>
	[Serializable]
	public class MerchantPictureSpacePicInfo 
	{
		/**图片空间类别id */
		[XmlElement("picCategoryId")]
			public long?  PicCategoryId{ get; set; }

		/**图片空间id */
		[XmlElement("picSpaceId")]
			public long?  PicSpaceId{ get; set; }

		/**图片名称 */
		[XmlElement("picName")]
			public string  PicName{ get; set; }

		/**图片Url */
		[XmlElement("picUrl")]
			public string  PicUrl{ get; set; }

		/**图片id */
		[XmlElement("picId")]
			public long?  PicId{ get; set; }

	}
}
