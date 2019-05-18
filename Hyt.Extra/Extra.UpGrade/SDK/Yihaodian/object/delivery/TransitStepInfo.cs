using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Delivery
{
	/// <summary>
	/// 物流跟踪信息
	/// </summary>
	[Serializable]
	public class TransitStepInfo 
	{
		/**状态发生的时间 */
		[XmlElement("status_time")]
			public string  Status_time{ get; set; }

		/**状态描述 */
		[XmlElement("status_desc")]
			public string  Status_desc{ get; set; }

	}
}
