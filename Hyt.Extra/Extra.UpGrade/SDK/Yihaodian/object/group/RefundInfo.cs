using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Group
{
	/// <summary>
	/// 受理退款单信息
	/// </summary>
	[Serializable]
	public class RefundInfo 
	{
		/**受理退款单号 */
		[XmlElement("refundCode")]
			public string  RefundCode{ get; set; }

		/**退款金额 */
		[XmlElement("refundAmount")]
			public double?  RefundAmount{ get; set; }

	}
}
