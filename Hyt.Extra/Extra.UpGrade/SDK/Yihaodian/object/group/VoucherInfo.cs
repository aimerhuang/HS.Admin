using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Group
{
	/// <summary>
	/// 消费券信息
	/// </summary>
	[Serializable]
	public class VoucherInfo 
	{
		/**消费券号码 */
		[XmlElement("voucherCode")]
			public string  VoucherCode{ get; set; }

		/**发放时间 */
		[XmlElement("issueTime")]
			public string  IssueTime{ get; set; }

		/**消费时间 */
		[XmlElement("consumptionTime")]
			public string  ConsumptionTime{ get; set; }

		/**消费券有效起始时间 */
		[XmlElement("voucherStartTime")]
			public string  VoucherStartTime{ get; set; }

		/**消费券有效结束时间 */
		[XmlElement("voucherEndTime")]
			public string  VoucherEndTime{ get; set; }

		/**消费券剩余可消费次数 */
		[XmlElement("voucherCount")]
			public int?  VoucherCount{ get; set; }

	}
}
