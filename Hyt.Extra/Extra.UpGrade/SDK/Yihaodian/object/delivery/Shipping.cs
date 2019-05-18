using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Delivery
{
	/// <summary>
	/// 物流订单详情列表
	/// </summary>
	[Serializable]
	public class Shipping 
	{
		/**交易ID */
		[XmlElement("tid")]
			public long?  Tid{ get; set; }

		/**物流订单编号 */
		[XmlElement("order_code")]
			public string  Order_code{ get; set; }

		/**物流订单状态 */
		[XmlElement("status")]
			public string  Status{ get; set; }

		/**标示为是否快捷COD订单（暂不提供） */
		[XmlElement("is_quick_cod_order")]
			public bool  Is_quick_cod_order{ get; set; }

		/**卖家昵称 */
		[XmlElement("seller_nick")]
			public string  Seller_nick{ get; set; }

		/**买家昵称 */
		[XmlElement("buyer_nick")]
			public string  Buyer_nick{ get; set; }

		/**预约取货开始时间(暂不提供) */
		[XmlElement("delivery_start")]
			public string  Delivery_start{ get; set; }

		/**预约取货结束时间(暂不提供) */
		[XmlElement("delivery_end")]
			public string  Delivery_end{ get; set; }

		/** 	运单号.具体一个物流公司的运单号码. */
		[XmlElement("out_sid")]
			public string  Out_sid{ get; set; }

		/**货物名称(暂不提供) */
		[XmlElement("item_title")]
			public string  Item_title{ get; set; }

		/**收件人姓名 */
		[XmlElement("receiver_name")]
			public string  Receiver_name{ get; set; }

		/**收件人电话 */
		[XmlElement("receiver_phone")]
			public string  Receiver_phone{ get; set; }

		/** 收件人手机号码 */
		[XmlElement("receiver_mobile")]
			public string  Receiver_mobile{ get; set; }

		/**收件人地址信息(在传输请求参数Fields字段时，才能返回此字段) */
		[XmlElement("location")]
			public string  Location{ get; set; }

		/**物流方式.可选值:free(卖家包邮),post(平邮),express(快递),ems(EMS).(暂不提供) */
		[XmlElement("type")]
			public string  Type{ get; set; }

		/**谁承担运费.可选值:buyer(买家承担),seller(卖家承担运费).(暂不提供) */
		[XmlElement("freight_payer")]
			public string  Freight_payer{ get; set; }

		/**卖家是否确认发货.可选值:yes(是),no(否). */
		[XmlElement("seller_confirm")]
			public string  Seller_confirm{ get; set; }

		/** 	物流公司名称 */
		[XmlElement("company_name")]
			public string  Company_name{ get; set; }

		/** 返回发货是否成功。 */
		[XmlElement("is_success")]
			public bool  Is_success{ get; set; }

		/**运单创建时间 */
		[XmlElement("created")]
			public string  Created{ get; set; }

		/**运单修改时间 */
		[XmlElement("modified")]
			public string  Modified{ get; set; }

		/**表明是否是拆单，默认值0，1表示拆单 */
		[XmlElement("is_spilt")]
			public long?  Is_spilt{ get; set; }

		/**拆单子订单列表，对应的数据是：该物流订单下的全部子订单(暂不提供) */
		[XmlElement("sub_tids")]
			public string  Sub_tids{ get; set; }

	}
}
