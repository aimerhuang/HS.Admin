using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierCategory
{
	/// <summary>
	/// 品牌信息
	/// </summary>
	[Serializable]
	public class Brand 
	{
		/**品牌名称 */
		[XmlElement("brandName")]
			public string  BrandName{ get; set; }

		/**品牌id */
		[XmlElement("brandId")]
			public long?  BrandId{ get; set; }

	}
}
