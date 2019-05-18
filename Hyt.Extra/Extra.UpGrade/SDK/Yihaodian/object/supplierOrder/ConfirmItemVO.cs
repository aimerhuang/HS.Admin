using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierOrder
{
	/// <summary>
	/// 需要确认的PO单详情信息
	/// </summary>
	[Serializable]
	public class ConfirmItemVO 
	{
		/**PO详情项ID */
		[XmlElement("poItemId")]
			public long?  PoItemId{ get; set; }

		/**发货数量 */
		[XmlElement("shipQty")]
			public int?  ShipQty{ get; set; }

		/**不一致的原因id */
		[XmlElement("lessCode")]
			public int?  LessCode{ get; set; }

		/**不一致的原因说明 */
		[XmlElement("lessRemark")]
			public string  LessRemark{ get; set; }

		/**是否打印 */
		[XmlElement("enablePrint")]
			public int?  EnablePrint{ get; set; }

	}
}
