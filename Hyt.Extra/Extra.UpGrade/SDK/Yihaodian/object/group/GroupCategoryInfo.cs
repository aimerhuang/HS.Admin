using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Group
{
	/// <summary>
	/// 团购类目信息
	/// </summary>
	[Serializable]
	public class GroupCategoryInfo 
	{
		/**团购类目id */
		[XmlElement("categoryId")]
			public long?  CategoryId{ get; set; }

		/**团购类目名称 */
		[XmlElement("categoryName")]
			public string  CategoryName{ get; set; }

	}
}
