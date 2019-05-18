using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.OldForNew
{
	/// <summary>
	/// 询价单
	/// </summary>
	[Serializable]
	public class InquiryBillVo 
	{
		/**主键 */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**询价单id */
		[XmlElement("inquiry_bill_id")]
			public long?  Inquiry_bill_id{ get; set; }

		/**商品编码code */
		[XmlElement("product_code")]
			public string  Product_code{ get; set; }

		/**商品名称 */
		[XmlElement("product_name")]
			public string  Product_name{ get; set; }

		/**商品属性 */
		[XmlElement("product_attrs")]
			public string  Product_attrs{ get; set; }

		/**询价单时间 */
		[XmlElement("inquiry_time")]
			public string  Inquiry_time{ get; set; }

		/**兑换类型（0、分类券，1、品牌券，2、产品券，3、全场返利） */
		[XmlElement("exchange_type")]
			public string  Exchange_type{ get; set; }

		/**报价,格式:  活动id:价格;活动id:价格; */
		[XmlElement("quoted_price")]
			public string  Quoted_price{ get; set; }

		/**备注 */
		[XmlElement("remark")]
			public string  Remark{ get; set; }

		/**图片,图片大小尺寸 需要注意 */
		[XmlElement("product_picture")]
			public string  Product_picture{ get; set; }

		/**抵用券 */
		[XmlElement("coupon_amount")]
			public double?  Coupon_amount{ get; set; }

		/**返利金额 */
		[XmlElement("account_amount")]
			public double?  Account_amount{ get; set; }

		/**兑换内容（现金/品类券） */
		[XmlElement("exchange_content")]
			public string  Exchange_content{ get; set; }

		/**活动code */
		[XmlElement("active_code")]
			public string  Active_code{ get; set; }

		/**结算价 */
		[XmlElement("settlement_amount")]
			public double?  Settlement_amount{ get; set; }

	}
}
