using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Promotion
{
	/// <summary>
	/// 持券商家信息
	/// </summary>
	[Serializable]
	public class CouponMerchant 
	{
		/**抵用券本身关联的活动id */
		[XmlElement("couponActiveId")]
			public long?  CouponActiveId{ get; set; }

		/**抵用券本身关联的活动名称 */
		[XmlElement("activeName")]
			public string  ActiveName{ get; set; }

		/**活动起始时间 */
		[XmlElement("activeBegin")]
			public string  ActiveBegin{ get; set; }

		/**活动结束时间 */
		[XmlElement("activeEnd")]
			public string  ActiveEnd{ get; set; }

		/**活动类型(为8：促销活动发券) */
		[XmlElement("activeType")]
			public int?  ActiveType{ get; set; }

		/**是否有效(固定值) */
		[XmlElement("isAvailable")]
			public int?  IsAvailable{ get; set; }

		/**最大发放张数 */
		[XmlElement("maxSendCount")]
			public int?  MaxSendCount{ get; set; }

		/**抵用券审核状态 */
		[XmlElement("auditStatus")]
			public int?  AuditStatus{ get; set; }

		/**活动范围类型 */
		[XmlElement("definedType")]
			public int?  DefinedType{ get; set; }

		/**商家id */
		[XmlElement("merchantId")]
			public long?  MerchantId{ get; set; }

		/**商家名称(店铺名称) */
		[XmlElement("merchantName")]
			public string  MerchantName{ get; set; }

		/**1：默认支持联合发券；0：默认不支持联合发券 */
		[XmlElement("defaultUseSupport")]
			public int?  DefaultUseSupport{ get; set; }

	}
}
