using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	/// <summary>
	/// 单个产品属性信息
	/// </summary>
	[Serializable]
	public class ProductAttributesInfo 
	{
		/**属性ID */
		[XmlElement("attributeId")]
			public long?  AttributeId{ get; set; }

		/**属性值 */
		[XmlElement("attributeValue")]
			public string  AttributeValue{ get; set; }

		/**属性名称 */
		[XmlElement("attributeName")]
			public string  AttributeName{ get; set; }

	}
}
