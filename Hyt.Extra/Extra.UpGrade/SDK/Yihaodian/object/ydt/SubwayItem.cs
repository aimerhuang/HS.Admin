using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Ydt
{
	/// <summary>
	/// 主站商品对象
	/// </summary>
	[Serializable]
	public class SubwayItem 
	{
		/**商品价格 */
		[XmlElement("price")]
			public double?  Price{ get; set; }

		/**商品标题 */
		[XmlElement("title")]
			public string  Title{ get; set; }

		/**商品发布时间 */
		[XmlElement("publish_time")]
			public string  Publish_time{ get; set; }

		/**库存数量 */
		[XmlElement("quantity")]
			public int?  Quantity{ get; set; }

		/**上周销量 */
		[XmlElement("sales_count_last_week")]
			public int?  Sales_count_last_week{ get; set; }

	}
}
