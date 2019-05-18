using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Data
{
	/// <summary>
	/// 产品销售统计详细信息
	/// </summary>
	[Serializable]
	public class ProductStatisticsInfo 
	{
		/**订单量 */
		[XmlElement("orderCount")]
			public long?  OrderCount{ get; set; }

		/**订单转化率 */
		[XmlElement("orderRate")]
			public long?  OrderRate{ get; set; }

		/**产品点击量 */
		[XmlElement("productClickCount")]
			public long?  ProductClickCount{ get; set; }

		/**产品销售量 */
		[XmlElement("productSaleCount")]
			public long?  ProductSaleCount{ get; set; }

		/**统计日期 */
		[XmlElement("statisticsDate")]
			public string  StatisticsDate{ get; set; }

	}
}
