using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	/// <summary>
	/// 产品属性项信息
	/// </summary>
	[Serializable]
	public class ProdAttributeItemInfo 
	{
		/**属性ID(从获取类别属性(yhd.category.attribute.get)接口中获取) */
		[XmlElement("attributeId")]
			public long?  AttributeId{ get; set; }

		/**选项ID列表(竖杠分隔。如选项ID1|选项ID2,选项ID不能重复)(从yhd.category.attribute.get接口中获取) */
		[XmlElement("itemId")]
			public long?  ItemId{ get; set; }

	}
}
