using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Ydt
{
	/// <summary>
	/// 地域
	/// </summary>
	[Serializable]
	public class AreaOption 
	{
		/**地域id */
		[XmlElement("area_id")]
			public long?  Area_id{ get; set; }

		/**地域名称 */
		[XmlElement("name")]
			public string  Name{ get; set; }

		/**地域级别，目前自治区、省、直辖市是1，其他城市、地区是2 */
		[XmlElement("level")]
			public int?  Level{ get; set; }

		/**父地域id，若该字段为0表示该行政区为顶层，例如像北京，国外等 */
		[XmlElement("parent_id")]
			public int?  Parent_id{ get; set; }

	}
}
