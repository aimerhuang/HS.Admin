using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	/// <summary>
	/// 系列属性值信息
	/// </summary>
	[Serializable]
	public class SerialAttributeInfo 
	{
		/**属性id  */
		[XmlElement("attributeId")]
			public long?  AttributeId{ get; set; }

		/**属性名称 */
		[XmlElement("attributeName")]
			public string  AttributeName{ get; set; }

		/**属性类型(1：颜色 2：非颜色 ) */
		[XmlElement("type")]
			public int?  Type{ get; set; }

		/**选项ID(属性值ID) */
		[XmlElement("itemId")]
			public long?  ItemId{ get; set; }

		/**选项值(属性值) */
		[XmlElement("itemLabel")]
			public string  ItemLabel{ get; set; }

		/**标准值 */
		[XmlElement("standardLabel")]
			public string  StandardLabel{ get; set; }

	}
}
