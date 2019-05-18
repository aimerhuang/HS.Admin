using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Picture
{
	/// <summary>
	/// 图片空间分类
	/// </summary>
	[Serializable]
	public class PicCategoryInfo 
	{
		/**图片空间分类ID */
		[XmlElement("picCategoryId")]
			public long?  PicCategoryId{ get; set; }

		/**图片空间分类名称 */
		[XmlElement("picCategoryName")]
			public string  PicCategoryName{ get; set; }

	}
}
