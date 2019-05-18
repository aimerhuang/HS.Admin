using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.OldForNew
{
	/// <summary>
	/// 检测单
	/// </summary>
	[Serializable]
	public class InspectionBillVo 
	{
		/**主键id */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**检测单id */
		[XmlElement("inspection_bill_id")]
			public long?  Inspection_bill_id{ get; set; }

		/**交易单id */
		[XmlElement("transaction_order_bill_id")]
			public long?  Transaction_order_bill_id{ get; set; }

		/**询价单id */
		[XmlElement("inquiry_bill_id")]
			public long?  Inquiry_bill_id{ get; set; }

		/**商品的检测结属性信息 */
		[XmlElement("product_attrs")]
			public string  Product_attrs{ get; set; }

		/**检测时间 */
		[XmlElement("inspection_time")]
			public string  Inspection_time{ get; set; }

		/**兑换内容（现金/品类券） */
		[XmlElement("exchange_content")]
			public string  Exchange_content{ get; set; }

		/**兑换类型（0、分类券，1、品牌券，2产品券，3、全场返利） */
		[XmlElement("exchange_type")]
			public int?  Exchange_type{ get; set; }

		/**成交价 */
		[XmlElement("deal_price")]
			public int?  Deal_price{ get; set; }

		/**检测员（姓名+ID） */
		[XmlElement("inspection_people")]
			public string  Inspection_people{ get; set; }

		/**备注 */
		[XmlElement("remark")]
			public string  Remark{ get; set; }

		/**合作商id */
		[XmlElement("partner_business_id")]
			public int?  Partner_business_id{ get; set; }

		/**用户id */
		[XmlElement("end_user_id")]
			public long?  End_user_id{ get; set; }

		/**合作商确认活动code */
		[XmlElement("active_code")]
			public string  Active_code{ get; set; }

	}
}
