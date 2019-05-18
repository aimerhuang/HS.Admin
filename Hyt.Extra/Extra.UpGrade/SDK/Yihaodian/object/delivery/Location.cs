using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Delivery
{
	/// <summary>
	/// 用户地址 
	/// </summary>
	[Serializable]
	public class Location 
	{
		/**邮政编码 */
		[XmlElement("zip")]
			public string  Zip{ get; set; }

		/**详细地址，最大256个字节（128个中文） */
		[XmlElement("address")]
			public string  Address{ get; set; }

		/**所在城市（中文名称） */
		[XmlElement("city")]
			public string  City{ get; set; }

		/** 	所在省份（中文名称） */
		[XmlElement("state")]
			public string  State{ get; set; }

		/**国家名称 */
		[XmlElement("country ")]
			public string  Country { get; set; }

		/**区/县（只适用于物流API） */
		[XmlElement("district ")]
			public string  District { get; set; }

	}
}
