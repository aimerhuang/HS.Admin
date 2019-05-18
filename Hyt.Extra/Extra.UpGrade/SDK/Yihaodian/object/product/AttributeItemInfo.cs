using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
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

	}
}
