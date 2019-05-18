using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierCategory
{
	/// <summary>
	/// 类别信息
	/// </summary>
	[Serializable]
	public class Category 
	{
		/**类别名称 */
		[XmlElement("categoryName")]
			public string  CategoryName{ get; set; }

		/**类目ID */
		[XmlElement("categoryId")]
			public long?  CategoryId{ get; set; }

	}
}
