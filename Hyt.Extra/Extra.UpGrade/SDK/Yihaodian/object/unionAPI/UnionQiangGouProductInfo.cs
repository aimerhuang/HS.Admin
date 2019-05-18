using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 网盟抢购商品信息
	/// </summary>
	[Serializable]
	public class UnionQiangGouProductInfo 
	{
		/**默认pmInfoId */
		[XmlElement("default_pm_info_id")]
			public long?  Default_pm_info_id{ get; set; }

		/**商品一级品类 */
		[XmlElement("category_id")]
			public int?  Category_id{ get; set; }

		/**商品二级品类 */
		[XmlElement("sec_category_id")]
			public int?  Sec_category_id{ get; set; }

		/**状态Status=50预告中，status=101进行中 */
		[XmlElement("status")]
			public int?  Status{ get; set; }

		/**开始时间 */
		[XmlElement("start_time")]
			public string  Start_time{ get; set; }

		/**结束时间 */
		[XmlElement("end_time")]
			public string  End_time{ get; set; }

		/**抢购主标题 */
		[XmlElement("short_name")]
			public string  Short_name{ get; set; }

		/**抢购主图url */
		[XmlElement("image")]
			public string  Image{ get; set; }

		/**参考价 */
		[XmlElement("origional_price")]
			public double?  Origional_price{ get; set; }

		/**抢购价 */
		[XmlElement("price")]
			public double?  Price{ get; set; }

		/**PC商详url */
		[XmlElement("link_url")]
			public string  Link_url{ get; set; }

		/**h5 */
		[XmlElement("link_url_h5")]
			public string  Link_url_h5{ get; set; }

	}
}
