using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Category
{
	/// <summary>
	/// 选项属性子值信息
	/// </summary>
	[Serializable]
	public class SubAttributeEditItemInfo 
	{
		/**选项id */
		[XmlElement("itemId")]
			public long?  ItemId{ get; set; }

		/**选项标签 */
		[XmlElement("itemLabel")]
			public string  ItemLabel{ get; set; }

	}
}
