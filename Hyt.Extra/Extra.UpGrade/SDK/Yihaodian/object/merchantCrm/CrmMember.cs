using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.MerchantCrm
{
	/// <summary>
	/// 会员列表
	/// </summary>
	[Serializable]
	public class CrmMember 
	{
		/**会员名称 */
		[XmlElement("customer_pin")]
			public string  Customer_pin{ get; set; }

		/**会员等级 */
		[XmlElement("grade")]
			public string  Grade{ get; set; }

		/**交易成功的金额 */
		[XmlElement("trade_amount")]
			public double?  Trade_amount{ get; set; }

		/**交易成功笔数 */
		[XmlElement("trade_count")]
			public long?  Trade_count{ get; set; }

		/**平均客单价 */
		[XmlElement("avg_price")]
			public double?  Avg_price{ get; set; }

		/**最后交易时间 */
		[XmlElement("last_trade_time")]
			public string  Last_trade_time{ get; set; }

		/**会员ID */
		[XmlElement("buyer_id")]
			public long?  Buyer_id{ get; set; }

	}
}
