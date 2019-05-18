using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Promotion
{
	/// <summary>
	/// 活动关联的产品信息
	/// </summary>
	[Serializable]
	public class ActivityUsProduct 
	{
		/**主键（数据库自增) */
		[XmlElement("id")]
			public int?  Id{ get; set; }

		/**关联的活动id */
		[XmlElement("activityId")]
			public int?  ActivityId{ get; set; }

		/**产品id */
		[XmlElement("productId")]
			public int?  ProductId{ get; set; }

		/**产品名称 */
		[XmlElement("productName")]
			public string  ProductName{ get; set; }

		/**产品编码 */
		[XmlElement("productCode")]
			public string  ProductCode{ get; set; }

	}
}
