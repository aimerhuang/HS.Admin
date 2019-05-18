using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Category
{
	/// <summary>
	/// 品牌信息
	/// </summary>
	[Serializable]
	public class BrandInfo 
	{
		/**品牌名称 */
		[XmlElement("brandName")]
			public string  BrandName{ get; set; }

		/**品牌ID */
		[XmlElement("brandId")]
			public long?  BrandId{ get; set; }

	}
}
