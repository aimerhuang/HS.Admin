using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Logistics
{
	/// <summary>
	/// 箱子
	/// </summary>
	[Serializable]
	public class CartonInfo 
	{
		/**包箱编码，非空 */
		[XmlElement("cartonCode")]
			public string  CartonCode{ get; set; }

		/**快递公司面单号，非3PL快递配送可为空 */
		[XmlElement("shipmentNo")]
			public string  ShipmentNo{ get; set; }

		/**重量，单位：千克（KG），非空 */
		[XmlElement("weight")]
			public double?  Weight{ get; set; }

		/**长，单位：厘米 */
		[XmlElement("length")]
			public long?  Length{ get; set; }

		/**宽，单位：厘米 */
		[XmlElement("width")]
			public long?  Width{ get; set; }

		/**高，单位：厘米 */
		[XmlElement("height")]
			public long?  Height{ get; set; }

	}
}
