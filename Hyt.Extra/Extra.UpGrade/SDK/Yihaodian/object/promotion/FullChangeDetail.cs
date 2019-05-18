using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Promotion
{
	/// <summary>
	/// 满换购促销信息
	/// </summary>
	[Serializable]
	public class FullChangeDetail 
	{
		/**促销id */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**促销开始时间，格式如示例，开始时间必须是当前日期之前 */
		[XmlElement("startDate")]
			public string  StartDate{ get; set; }

		/**促销结束时间，格式如示例， 产品促销时间不能超过一个月！ */
		[XmlElement("endDate")]
			public string  EndDate{ get; set; }

		/**促销活动广告语 */
		[XmlElement("promotionTitle")]
			public string  PromotionTitle{ get; set; }

		/**0:已取消 1:尚未生效 2:生效中 3:已过期 */
		[XmlElement("status")]
			public int?  Status{ get; set; }

		/**满减类型。1：商品   2:分类   3:分类+品牌,4:全场 */
		[XmlElement("fullGiftType")]
			public int?  FullGiftType{ get; set; }

		/**促销是否被锁定 */
		[XmlElement("isLocked")]
			public int?  IsLocked{ get; set; }

		/**取消原因 */
		[XmlElement("reason")]
			public string  Reason{ get; set; }

	}
}
