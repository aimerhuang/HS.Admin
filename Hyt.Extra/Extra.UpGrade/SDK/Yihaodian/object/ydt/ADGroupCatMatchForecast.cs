using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Ydt
{
	/// <summary>
	/// 出价预估信息
	/// </summary>
	[Serializable]
	public class ADGroupCatMatchForecast 
	{
		/**推广组Id */
		[XmlElement("adgroup_id")]
			public long?  Adgroup_id{ get; set; }

		/**类目出价Id */
		[XmlElement("catmatch_id")]
			public long?  Catmatch_id{ get; set; }

		/**出价排名；11:2-出价11元，排名第二名 */
		[XmlElement("price_rank")]
			public string  Price_rank{ get; set; }

		/**创建时间 */
		[XmlElement("create_time")]
			public string  Create_time{ get; set; }

		/**最后修改时间 */
		[XmlElement("modified_time")]
			public string  Modified_time{ get; set; }

	}
}
