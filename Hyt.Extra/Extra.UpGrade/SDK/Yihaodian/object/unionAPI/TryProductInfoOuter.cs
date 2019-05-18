using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 网盟0元购项目产品对象
	/// </summary>
	[Serializable]
	public class TryProductInfoOuter 
	{
		/**试用活动id */
		[XmlElement("active_id")]
			public long?  Active_id{ get; set; }

		/**活动结束时间，该属性只适用于类型为免费试用 */
		[XmlElement("activity_end_date")]
			public string  Activity_end_date{ get; set; }

		/**累计申请人数，该属性只适用于类型为免邮试用 */
		[XmlElement("apply_quantity")]
			public int?  Apply_quantity{ get; set; }

		/**商家id */
		[XmlElement("merchant_id")]
			public long?  Merchant_id{ get; set; }

		/**商品id */
		[XmlElement("pm_info_id")]
			public long?  Pm_info_id{ get; set; }

		/**产品id */
		[XmlElement("product_id")]
			public long?  Product_id{ get; set; }

		/**产品名称 */
		[XmlElement("product_name")]
			public string  Product_name{ get; set; }

		/**还余试用份数，该属性只适用于类型为付邮试用 */
		[XmlElement("remain_quantity")]
			public int?  Remain_quantity{ get; set; }

		/** 试用总份数 */
		[XmlElement("total_quantity")]
			public int?  Total_quantity{ get; set; }

		/**试用类型 1，付邮试用。2：免费试用 */
		[XmlElement("try_type")]
			public int?  Try_type{ get; set; }

		/**试用中心详情页链接 */
		[XmlElement("try_url")]
			public string  Try_url{ get; set; }

		/**1号店价格 */
		[XmlElement("yhd_price")]
			public double?  Yhd_price{ get; set; }

		/**对应试用中心的H5 url */
		[XmlElement("mobile_try_url")]
			public string  Mobile_try_url{ get; set; }

	}
}
