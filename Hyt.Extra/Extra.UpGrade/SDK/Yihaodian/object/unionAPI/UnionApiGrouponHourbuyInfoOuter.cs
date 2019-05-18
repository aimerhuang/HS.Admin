using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 金牌秒杀团购
	/// </summary>
	[Serializable]
	public class UnionApiGrouponHourbuyInfoOuter 
	{
		/**团购ID */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**商品ID */
		[XmlElement("pm_info_id")]
			public long?  Pm_info_id{ get; set; }

		/**销售平台，0：全部 1：PC 2：无线 */
		[XmlElement("client_type")]
			public int?  Client_type{ get; set; }

		/**团购短标题 */
		[XmlElement("short_name")]
			public string  Short_name{ get; set; }

		/**开团时间 */
		[XmlElement("start_time")]
			public string  Start_time{ get; set; }

		/**结束时间 */
		[XmlElement("end_time")]
			public string  End_time{ get; set; }

		/**团购价格 */
		[XmlElement("price")]
			public double?  Price{ get; set; }

		/**参考价格 */
		[XmlElement("original_price")]
			public double?  Original_price{ get; set; }

		/**团购图片地址 */
		[XmlElement("image_detail")]
			public string  Image_detail{ get; set; }

		/**H5团购详情页 */
		[XmlElement("h5_detail_url")]
			public string  H5_detail_url{ get; set; }

		/**PC团购详情页 */
		[XmlElement("pc_detail_url")]
			public string  Pc_detail_url{ get; set; }

		/**销售省份Id集合 */
		[XmlElement("province_ids")]
			public string  Province_ids{ get; set; }

		/**团购状态，201为下线状态(团购结束或者无库存等),非201可以对外展示 */
		[XmlElement("status")]
			public int?  Status{ get; set; }

		/**已售百分比 */
		[XmlElement("sold_per")]
			public double?  Sold_per{ get; set; }

	}
}
