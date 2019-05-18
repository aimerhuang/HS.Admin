using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Itemcats
{
	/// <summary>
	/// 类目属性
	/// </summary>
	[Serializable]
	public class Feature 
	{
		/**属性键 */
		[XmlElement("attr_key")]
			public string  Attr_key{ get; set; }

		/**属性值 */
		[XmlElement("attr_value")]
			public string  Attr_value{ get; set; }

	}
}
