using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	/// <summary>
	/// 产品属性信息
	/// </summary>
	[Serializable]
	public class ProdAttributeInfo 
	{
		/**属性ID(从获取类别属性(yhd.category.attribute.get)接口中获取,编辑类型为文本类型) */
		[XmlElement("attributeId")]
			public long?  AttributeId{ get; set; }

		/**属性值(若valueType=1即数字类型的必须填写数字，最多两位小数) */
		[XmlElement("attributeValue")]
			public string  AttributeValue{ get; set; }

	}
}
