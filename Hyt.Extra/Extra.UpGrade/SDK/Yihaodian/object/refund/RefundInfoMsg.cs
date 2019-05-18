using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Refund
{
	/// <summary>
	/// 退货信息
	/// </summary>
	[Serializable]
	public class RefundInfoMsg 
	{
		/**退货详细信息 */
		[XmlElement("refundDetail")]
		public RefundDetail  RefundDetail{ get; set; }

		/**退货明细列表 */
		[XmlElement("refundItemList")]
		public RefundItemList  RefundItemList{ get; set; }

	}
}
