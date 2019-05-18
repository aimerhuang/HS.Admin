using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Category
{
	/// <summary>
	/// 类别系列属性信息
	/// </summary>
	[Serializable]
	public class SerialAttributeInfo 
	{
		/**属性id */
		[XmlElement("attributeId")]
			public long?  AttributeId{ get; set; }

		/**属性名称 */
		[XmlElement("attributeName")]
			public string  AttributeName{ get; set; }

		/**属性类型(1：颜色 2：非颜色) */
		[XmlElement("type")]
			public int?  Type{ get; set; }

		/**选项属性信息列表 */
		[XmlElement("attributeItemInfoList")]
		public AttributeItemInfoList  AttributeItemInfoList{ get; set; }

	}
}
