using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Category
{
	/// <summary>
	/// 选项属性信息
	/// </summary>
	[Serializable]
	public class AttributeItemInfo 
	{
		/**选项id */
		[XmlElement("itemId")]
			public long?  ItemId{ get; set; }

		/**选项标签 */
		[XmlElement("itemLabel")]
			public string  ItemLabel{ get; set; }

		/**选项属性子值信息列表（只有类别基本属性才有，系列属性忽略此项） */
		[XmlElement("subAttributeEditItemInfoList")]
		public AttributeItemInfoList  SubAttributeEditItemInfoList{ get; set; }

		/**选项属性标准值（只有类别系列属性才有，基本属性忽略此项） */
		[XmlElement("standardLabel")]
			public string  StandardLabel{ get; set; }

	}
}
