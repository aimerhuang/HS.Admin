using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.OldForNew
{
	/// <summary>
	/// 交易单
	/// </summary>
	[Serializable]
	public class TransactionOrderBillVo 
	{
		/**交易单id */
		[XmlElement("transaction_bill_bd")]
			public long?  Transaction_bill_bd{ get; set; }

		/**第三方询价单id */
		[XmlElement("yhd_inquiry_bill_id")]
			public long?  Yhd_inquiry_bill_id{ get; set; }

		/**第三方检测单id */
		[XmlElement("yhd_inspection_bill_id")]
			public long?  Yhd_inspection_bill_id{ get; set; }

		/**交易状态（0、已提交，5、已寄送，10、已收货，15、已检测 , 20、已完成    25、已取消    30、已关闭） */
		[XmlElement("bill_status")]
			public int?  Bill_status{ get; set; }

		/**交易单创建时间 */
		[XmlElement("create_time")]
			public string  Create_time{ get; set; }

		/**取货方式（0、快递，1、上门取货） */
		[XmlElement("pick_up_type")]
			public int?  Pick_up_type{ get; set; }

		/**联系人姓名 */
		[XmlElement("contact_name")]
			public string  Contact_name{ get; set; }

		/**联系人电话 */
		[XmlElement("contact_phone")]
			public string  Contact_phone{ get; set; }

		/**联系人地址 */
		[XmlElement("contact_address")]
			public string  Contact_address{ get; set; }

		/**快递人姓名 */
		[XmlElement("express_delivery_people")]
			public string  Express_delivery_people{ get; set; }

		/**快递人电话 */
		[XmlElement("express_delivery_phone")]
			public string  Express_delivery_phone{ get; set; }

		/**取件时间 */
		[XmlElement("pickup_time")]
			public string  Pickup_time{ get; set; }

		/**更新时间 */
		[XmlElement("update_time")]
			public string  Update_time{ get; set; }

		/**寄回时间 */
		[XmlElement("send_back_time")]
			public string  Send_back_time{ get; set; }

		/**收货时间 */
		[XmlElement("reciver_time")]
			public string  Reciver_time{ get; set; }

		/**完成时间 */
		[XmlElement("finish_time")]
			public string  Finish_time{ get; set; }

		/**询价单 */
		[XmlElement("inquiry_bill_vo")]
		public InquiryBillVo  Inquiry_bill_vo{ get; set; }

		/**检测单 */
		[XmlElement("inspection_bill_vo")]
		public InspectionBillVo  Inspection_bill_vo{ get; set; }

		/**取消关闭时间 */
		[XmlElement("cancel_time")]
			public string  Cancel_time{ get; set; }

		/**快递单号 */
		[XmlElement("express_number")]
			public string  Express_number{ get; set; }

	}
}
