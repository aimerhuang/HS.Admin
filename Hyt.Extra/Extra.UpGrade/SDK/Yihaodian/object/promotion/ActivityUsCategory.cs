using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Promotion
{
	/// <summary>
	/// 活动关联的类目信息
	/// </summary>
	[Serializable]
	public class ActivityUsCategory 
	{
		/**主键（数据库自增) */
		[XmlElement("id")]
			public int?  Id{ get; set; }

		/**关联的活动id */
		[XmlElement("activityId")]
			public int?  ActivityId{ get; set; }

		/**类目id(二级类目) */
		[XmlElement("categoryId")]
			public int?  CategoryId{ get; set; }

		/**类目名称 */
		[XmlElement("categoryName")]
			public string  CategoryName{ get; set; }

	}
}
