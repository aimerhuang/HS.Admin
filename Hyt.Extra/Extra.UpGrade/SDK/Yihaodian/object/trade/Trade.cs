using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Trade
{
	/// <summary>
	/// 交易结构
	/// </summary>
	[Serializable]
	public class Trade 
	{
		/**交易状态: ORDER_WAIT_PAY：已下单（货款未全收） ORDER_PAYED：已下单（货款已收） ORDER_TRUNED_TO_DO：可以发货（已送仓库） ORDER_OUT_OF_WH：已出库（货在途） ORDER_RECEIVED：货物用户已收到 ORDER_FINISH：订单完成 ORDER_CANCEL：订单取消 */
		[XmlElement("status")]
			public string  Status{ get; set; }

		/**交易标题，以店铺名作为此标题的值。 */
		[XmlElement("title")]
			public string  Title{ get; set; }

		/**交易类型列表，同时查询多种交易类型可用逗号分隔。（暂不提供) */
		[XmlElement("type")]
			public string  Type{ get; set; }

		/**商品价格。精确到2位小数；单位：元。如：200.07，表示：200元7分 */
		[XmlElement("price")]
			public string  Price{ get; set; }

		/**卖家货到付款服务费。精确到2位小数;单位:元。如:12.07，表示:12元7分。卖家不承担服务费的订单：未发货的订单获取服务费为0，发货后就能获取到正确值。（暂不提供） */
		[XmlElement("seller_cod_fee")]
			public string  Seller_cod_fee{ get; set; }

		/**满立减金额 */
		[XmlElement("discount_fee")]
			public string  Discount_fee{ get; set; }

		/**买家使用积分,下单时生成，且一直不变。格式:100;单位:个.（暂不提供） */
		[XmlElement("point_fee")]
			public long?  Point_fee{ get; set; }

		/**是否包含邮费。可选值:true(包含),false(不包含) */
		[XmlElement("has_post_fee")]
			public bool  Has_post_fee{ get; set; }

		/**商品金额（商品价格乘以数量的总金额）。精确到2位小数;单位:元。如:200.07，表示:200元7分 */
		[XmlElement("total_fee")]
			public string  Total_fee{ get; set; }

		/**是否保障速递，如果为true，则为保障速递订单，使用线下联系发货接口发货，如果未false，则该订单非保障速递，根据卖家设置的订单流转规则可使用物流宝或者常规物流发货。（暂不提供） */
		[XmlElement("is_lgtype")]
			public bool  Is_lgtype{ get; set; }

		/**表示是否是品牌特卖（常规特卖，不包括特卖惠和特实惠）订单，如果是返回true，如果不是返回false。（暂不提供） */
		[XmlElement("is_brand_sale")]
			public bool  Is_brand_sale{ get; set; }

		/**订单是否强制使用物流宝发货。当此字段与is_brand_sale均为true时，订单强制物流宝发货。此字段为false时，该订单根据流转规则设置可以使用物流宝或者常规方式发货（暂不提供） */
		[XmlElement("is_force_wlb")]
			public bool  Is_force_wlb{ get; set; }

		/**次日达订单送达时间 */
		[XmlElement("lg_aging")]
			public string  Lg_aging{ get; set; }

		/**次日达，三日达等送达类型（暂不提供） */
		[XmlElement("lg_aging_type")]
			public string  Lg_aging_type{ get; set; }

		/**交易创建时间。格式:yyyy-MM-dd HH:mm:ss */
		[XmlElement("created")]
			public string  Created{ get; set; }

		/**付款时间。格式:yyyy-MM-dd HH:mm:ss。订单的付款时间即为物流订单的创建时间。 */
		[XmlElement("pay_time")]
			public string  Pay_time{ get; set; }

		/**交易修改时间(用户对订单的任何修改都会更新此字段)。格式:yyyy-MM-dd HH:mm:ss */
		[XmlElement("modified")]
			public string  Modified{ get; set; }

		/**交易结束时间。交易成功时间(更新交易状态为成功的同时更新)/确认收货时间或者交易关闭时间 。格式:yyyy-MM-dd HH:mm:ss */
		[XmlElement("end_time")]
			public string  End_time{ get; set; }

		/**买家留言 */
		[XmlElement("buyer_message")]
			public string  Buyer_message{ get; set; }

		/**买家的支付宝id号，在UIC中有记录，买家支付宝的唯一标示，不因为买家更换Email账号而改变。（暂不提供） */
		[XmlElement("alipay_id")]
			public long?  Alipay_id{ get; set; }

		/**支付宝交易号，如：2009112081173831（暂不提供） */
		[XmlElement("alipay_no")]
			public string  Alipay_no{ get; set; }

		/**创建交易接口成功后，返回的支付url（暂不提供） */
		[XmlElement("alipay_url")]
			public string  Alipay_url{ get; set; }

		/**买家备注（与1号店上订单的买家备注对应，只有买家才能查看该字段） */
		[XmlElement("buyer_memo")]
			public string  Buyer_memo{ get; set; }

		/**买家备注旗帜（暂不提供） */
		[XmlElement("buyer_flag")]
			public long?  Buyer_flag{ get; set; }

		/**卖家备注 */
		[XmlElement("seller_memo")]
			public string  Seller_memo{ get; set; }

		/**卖家备注旗帜（暂不提供） */
		[XmlElement("seller_flag")]
			public long?  Seller_flag{ get; set; }

		/**发票抬头 */
		[XmlElement("invoice_name")]
			public string  Invoice_name{ get; set; }

		/**发票类型 */
		[XmlElement("invoice_type")]
			public string  Invoice_type{ get; set; }

		/**买家昵称 */
		[XmlElement("buyer_nick")]
			public string  Buyer_nick{ get; set; }

		/**买家下单的地区（暂不提供） */
		[XmlElement("buyer_area")]
			public string  Buyer_area{ get; set; }

		/**买家邮件地址 */
		[XmlElement("buyer_email")]
			public string  Buyer_email{ get; set; }

		/**订单中是否包含运费险订单，如果包含运费险订单返回true，不包含运费险订单，返回false（暂不提供） */
		[XmlElement("has_yfx")]
			public bool  Has_yfx{ get; set; }

		/**订单的运费险，单位为元（暂不提供） */
		[XmlElement("yfx_fee")]
			public string  Yfx_fee{ get; set; }

		/**运费险支付号（暂不提供） */
		[XmlElement("yfx_id")]
			public string  Yfx_id{ get; set; }

		/**运费险类型（暂不提供） */
		[XmlElement("yfx_type")]
			public string  Yfx_type{ get; set; }

		/**判断订单是否有买家留言，有买家留言返回true，否则返回false（暂不提供） */
		[XmlElement("has_buyer_message")]
			public bool  Has_buyer_message{ get; set; }

		/**区域id，代表订单下单的区位码，区位码是通过省市区转换而来，通过区位码能精确到区内的划分，比如310012是杭州市西湖区华星路（暂不提供） */
		[XmlElement("area_id")]
			public string  Area_id{ get; set; }

		/**使用信用卡支付金额数（暂不提供） */
		[XmlElement("credit_card_fee")]
			public string  Credit_card_fee{ get; set; }

		/**卡易售垂直表信息，去除下单ip之后的结果（暂不提供） */
		[XmlElement("nut_feature")]
			public string  Nut_feature{ get; set; }

		/**分阶段付款的订单状态（例如万人团订单等），目前有三返回状态 FRONT_NOPAID_FINAL_NOPAID(定金未付尾款未付)，FRONT_PAID_FINAL_NOPAID(定金已付尾款未付)，FRONT_PAID_FINAL_PAID(定金和尾款都付)（暂不提供） */
		[XmlElement("step_trade_status")]
			public string  Step_trade_status{ get; set; }

		/**分阶段付款的已付金额（万人团订单已付金额）（暂不提供） */
		[XmlElement("step_paid_fee")]
			public string  Step_paid_fee{ get; set; }

		/**订单出现异常问题的时候，给予用户的描述,没有异常的时候，此值为空（暂不提供） */
		[XmlElement("mark_desc")]
			public string  Mark_desc{ get; set; }

		/**电子凭证的垂直信息（暂不提供） */
		[XmlElement("eticket_ext")]
			public string  Eticket_ext{ get; set; }

		/**订单将在此时间前发出，主要用于预售订单（暂不提供） */
		[XmlElement("send_time")]
			public string  Send_time{ get; set; }

		/**创建交易时的物流方式（交易完成前，物流方式有可能改变，但系统里的这个字段一直不变）。可选值：free(卖家包邮),post(平邮),express(快递),ems(EMS),virtual(虚拟发货)，25(次日必达)，26(预约配送)。（暂不提供） */
		[XmlElement("shipping_type")]
			public string  Shipping_type{ get; set; }

		/**买家货到付款服务费。精确到2位小数;单位:元。如:12.07，表示:12元7分（暂不提供） */
		[XmlElement("buyer_cod_fee")]
			public string  Buyer_cod_fee{ get; set; }

		/**快递代收款。精确到2位小数;单位:元。如:212.07，表示:212元7分（暂不提供） */
		[XmlElement("express_agency_fee")]
			public string  Express_agency_fee{ get; set; }

		/**卖家手工调整金额，精确到2位小数，单位：元。如：200.07，表示：200元7分。来源于订单价格修改，如果有多笔子订单的时候，这个为0，单笔的话则跟[order].adjust_fee一样（暂不提供） */
		[XmlElement("adjust_fee")]
			public string  Adjust_fee{ get; set; }

		/**买家获得积分,返点的积分。格式:100;单位:个。返点的积分要交易成功之后才能获得。（暂不提供） */
		[XmlElement("buyer_obtain_point_fee")]
			public long?  Buyer_obtain_point_fee{ get; set; }

		/**货到付款服务费。精确到2位小数;单位:元。如:12.07，表示:12元7分。（暂不提供） */
		[XmlElement("cod_fee")]
			public string  Cod_fee{ get; set; }

		/**交易内部来源。 网上下单，客服代客下单，手机，1mall订单，药网，百度订单，试用中心订单，微便利订单，B2B2C订单，当当网订单 */
		[XmlElement("trade_from")]
			public string  Trade_from{ get; set; }

		/**(暂不提供) */
		[XmlElement("alipay_warn_msg")]
			public string  Alipay_warn_msg{ get; set; }

		/**货到付款物流状态。(暂不提供) */
		[XmlElement("cod_status")]
			public string  Cod_status{ get; set; }

		/**买家可以通过此字段查询是否当前交易可以评论，can_rate=true可以评价，false则不能评价(暂不提供)。 */
		[XmlElement("can_rate")]
			public bool  Can_rate{ get; set; }

		/**交易佣金。精确到2位小数;单位:元。如:200.07，表示:200元7分(暂不提供) */
		[XmlElement("commission_fee")]
			public string  Commission_fee{ get; set; }

		/**交易备注。(暂不提供) */
		[XmlElement("trade_memo")]
			public string  Trade_memo{ get; set; }

		/**买家是否已评价。可选值:true(已评价),false(未评价)。如买家只评价未打分，此字段仍返回false(暂不提供) */
		[XmlElement("buyer_rate")]
			public bool  Buyer_rate{ get; set; }

		/**交易外部来源：ownshop(商家官网)(暂不提供) */
		[XmlElement("trade_source")]
			public string  Trade_source{ get; set; }

		/**卖家是否可以对订单进行评价(暂不提供) */
		[XmlElement("seller_can_rate")]
			public bool  Seller_can_rate{ get; set; }

		/**是否是多次发货的订单 如果卖家对订单进行多次发货，则为true 否则为false(暂不提供) */
		[XmlElement("is_part_consign")]
			public bool  Is_part_consign{ get; set; }

		/**表示订单交易是否含有对应的代销采购单。 如果该订单中存在一个对应的代销采购单，那么该值为true；反之，该值为false。(暂不提供) */
		[XmlElement("is_daixiao")]
			public bool  Is_daixiao{ get; set; }

		/**买家实际使用积分（扣除部分退款使用的积分），交易完成后生成（交易成功或关闭），交易未完成时该字段值为0。格式:100;单位:个(暂不提供) */
		[XmlElement("real_point_fee")]
			public long?  Real_point_fee{ get; set; }

		/**收货人的所在城市 */
		[XmlElement("receiver_city")]
			public string  Receiver_city{ get; set; }

		/**收货人的所在地区 */
		[XmlElement("receiver_district")]
			public string  Receiver_district{ get; set; }

		/**表示订单交易是否网厅订单。 如果该订单是网厅订单，那么该值为true；反之，该值为false。（暂不提供） */
		[XmlElement("is_wt")]
			public bool  Is_wt{ get; set; }

		/**卖家销售公司名称 */
		[XmlElement("seller_nick")]
			public string  Seller_nick{ get; set; }

		/**商品图片绝对途径（暂不提供） */
		[XmlElement("pic_path")]
			public string  Pic_path{ get; set; }

		/**实付金额(实付金额=产品金额-促销活动立减金额+运费-抵用券金额)。精确到2位小数;单位:元。如:200.07，表示:200元7分 */
		[XmlElement("payment")]
			public string  Payment{ get; set; }

		/**交易快照地址（暂不提供） */
		[XmlElement("snapshot_url")]
			public string  Snapshot_url{ get; set; }

		/**交易快照详细信息（暂不提供） */
		[XmlElement("snapshot")]
			public string  Snapshot{ get; set; }

		/**卖家是否已评价。可选值:true(已评价),false(未评价)（暂不提供） */
		[XmlElement("seller_rate")]
			public bool  Seller_rate{ get; set; }

		/**邮费。精确到2位小数;单位:元。如:200.07，表示:200元7分 */
		[XmlElement("post_fee")]
			public string  Post_fee{ get; set; }

		/**买家支付宝账号（暂不提供） */
		[XmlElement("buyer_alipay_no")]
			public string  Buyer_alipay_no{ get; set; }

		/**收货人的姓名 */
		[XmlElement("receiver_name")]
			public string  Receiver_name{ get; set; }

		/**收货人的所在省份 */
		[XmlElement("receiver_state")]
			public string  Receiver_state{ get; set; }

		/**收货人的详细地址 */
		[XmlElement("receiver_address")]
			public string  Receiver_address{ get; set; }

		/**收货人的邮编 */
		[XmlElement("receiver_zip")]
			public string  Receiver_zip{ get; set; }

		/**收货人的手机号码 */
		[XmlElement("receiver_mobile")]
			public string  Receiver_mobile{ get; set; }

		/**收货人的电话号码 */
		[XmlElement("receiver_phone")]
			public string  Receiver_phone{ get; set; }

		/**卖家发货时间。格式:yyyy-MM-dd HH:mm:ss */
		[XmlElement("consign_time")]
			public string  Consign_time{ get; set; }

		/**卖家支付宝账号（暂不提供） */
		[XmlElement("seller_alipay_no")]
			public string  Seller_alipay_no{ get; set; }

		/**卖家手机 */
		[XmlElement("seller_mobile")]
			public string  Seller_mobile{ get; set; }

		/**卖家电话 */
		[XmlElement("seller_phone")]
			public string  Seller_phone{ get; set; }

		/**卖家姓名 */
		[XmlElement("seller_name")]
			public string  Seller_name{ get; set; }

		/**卖家邮件地址 */
		[XmlElement("seller_email")]
			public string  Seller_email{ get; set; }

		/**交易中剩余的确认收货金额（这个金额会随着子订单确认收货而不断减少，交易成功后会变为零）。精确到2位小数;单位:元。如:200.07，表示:200 元7分（暂不提供） */
		[XmlElement("available_confirm_fee")]
			public string  Available_confirm_fee{ get; set; }

		/**实收款（产品金额-促销活动立减金额 +运费 - 商家抵用券） */
		[XmlElement("received_payment")]
			public string  Received_payment{ get; set; }

		/**超时到期时间。格式:yyyy-MM-dd HH:mm:ss。业务规则： 前提条件：只有在买家已付款，卖家已发货的情况下才有效 如果申请了退款，那么超时会落在子订单上；比如说3笔ABC，A申请了，那么返回的是BC的列表, 主定单不存在 如果没有申请过退款，那么超时会挂在主定单上；比如ABC，返回主定单，ABC的超时和主定单相同 */
		[XmlElement("timeout_action_time")]
			public string  Timeout_action_time{ get; set; }

		/**（暂不提供） */
		[XmlElement("is_3_d")]
			public bool  Is_3_d{ get; set; }

		/**交易促销详细信息 */
		[XmlElement("promotion")]
			public string  Promotion{ get; set; }

		/**交易编号 */
		[XmlElement("tid")]
			public long?  Tid{ get; set; }

		/**商品购买数量。 */
		[XmlElement("num")]
			public long?  Num{ get; set; }

		/**商品数字编号 */
		[XmlElement("num_iid")]
			public long?  Num_iid{ get; set; }

		/**订单列表 */
		[XmlElement("orders")]
		public OrderList  Orders{ get; set; }

		/**订单编码 */
		[XmlElement("order_code")]
			public string  Order_code{ get; set; }

		/**发票需要情况：0 不需要 1.普通发票（旧）2.普通发票 3.增值税发票 */
		[XmlElement("order_need_invoice")]
			public int?  Order_need_invoice{ get; set; }

		/**仓库id */
		[XmlElement("warehouse_id")]
			public long?  Warehouse_id{ get; set; }

		/**买家用户id */
		[XmlElement("end_user_id")]
			public long?  End_user_id{ get; set; }

		/**服务子订单列表（暂不提供 ） */
		[XmlElement("service_orders")]
		public ServiceOrderList  Service_orders{ get; set; }

		/**优惠详情（暂不提供） */
		[XmlElement("promotion_details")]
		public PromotionDetailList  Promotion_details{ get; set; }

		/**是否无线端订单(0:PC端订单;1:无线端订单) */
		[XmlElement("is_mobile_order")]
			public int?  Is_mobile_order{ get; set; }

		/**订单订金 */
		[XmlElement("order_deposit")]
			public double?  Order_deposit{ get; set; }

		/**是否为预售订单 */
		[XmlElement("is_deposit_order")]
			public int?  Is_deposit_order{ get; set; }

		/**订金支付时间 */
		[XmlElement("deposit_paid_time")]
			public string  Deposit_paid_time{ get; set; }

		/**购买人税号 */
		[XmlElement("purchaser_tax_code")]
			public string  Purchaser_tax_code{ get; set; }

	}
}
