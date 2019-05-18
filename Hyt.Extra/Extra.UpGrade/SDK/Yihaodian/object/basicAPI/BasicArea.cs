using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.BasicAPI
{
	/// <summary>
	/// 地址信息
	/// </summary>
	[Serializable]
	public class BasicArea 
	{
		/**区域ID */
		[XmlElement("areaId")]
			public long?  AreaId{ get; set; }

		/**父id */
		[XmlElement("parentId")]
			public long?  ParentId{ get; set; }

		/**区域名称 */
		[XmlElement("areaName")]
			public string  AreaName{ get; set; }

		/**1省份2市3区 */
		[XmlElement("level")]
			public string  Level{ get; set; }

		/**邮编 */
		[XmlElement("postCode")]
			public string  PostCode{ get; set; }

		/**区域拼音 */
		[XmlElement("pinyin")]
			public string  Pinyin{ get; set; }

	}
}
