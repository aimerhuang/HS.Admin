using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Category
{
	/// <summary>
	/// 类别基本属性信息
	/// </summary>
	[Serializable]
	public class CategoryAttributeInfo 
	{
		/**属性id */
		[XmlElement("attributeId")]
			public long?  AttributeId{ get; set; }

		/**属性名称 */
		[XmlElement("attributeName")]
			public string  AttributeName{ get; set; }

		/**属性值的类型，0：文本类型1：数字类型 */
		[XmlElement("valueType")]
			public int?  ValueType{ get; set; }

		/**编辑类型，0：文本类型1:下拉列表类型（单选）2：多选框（多选） */
		[XmlElement("editType")]
			public int?  EditType{ get; set; }

		/**属性类型，0：正常属性1：导购属性 */
		[XmlElement("type")]
			public int?  Type{ get; set; }

		/**单位 */
		[XmlElement("unit")]
			public string  Unit{ get; set; }

		/**是否必填，0:是1:不是 */
		[XmlElement("isMustEnter")]
			public int?  IsMustEnter{ get; set; }

		/**是否描述属性，0:不是1:是 */
		[XmlElement("isDescribeAttribute")]
			public int?  IsDescribeAttribute{ get; set; }

		/**是否导购属性，0:不是1:是 */
		[XmlElement("isGuideAttribute")]
			public int?  IsGuideAttribute{ get; set; }

		/**是否系列属性，0:不是1:是 */
		[XmlElement("isSeriesAttribute")]
			public int?  IsSeriesAttribute{ get; set; }

		/**选项属性信息列表 */
		[XmlElement("attributeItemInfoList")]
		public AttributeItemInfoList  AttributeItemInfoList{ get; set; }

	}
}
