using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Trade
{
	/// <summary>
	/// 订单结构
	/// </summary>
	[Serializable]
	public class Order 
	{
		/**子订单编号(暂不提供) */
		[XmlElement("oid")]
			public long?  Oid{ get; set; }

		/**商品的字符串编号(注意：iid近期即将废弃，请用num_iid参数) */
		[XmlElement("iid")]
			public string  Iid{ get; set; }

		/**商品标题 */
		[XmlElement("title")]
			public string  Title{ get; set; }

		/**商品价格。精确到2位小数;单位:元。如:200.07，表示:200元7分 */
		[XmlElement("price")]
			public string  Price{ get; set; }

		/**商品数字ID */
		[XmlElement("num_iid")]
			public long?  Num_iid{ get; set; }

		/**套餐ID（暂不提供） */
		[XmlElement("item_meal_id")]
			public long?  Item_meal_id{ get; set; }

		/**商品的最小库存单位Sku的id. */
		[XmlElement("sku_id")]
			public string  Sku_id{ get; set; }

		/**购买数量。取值范围:大于零的整数 */
		[XmlElement("num")]
			public long?  Num{ get; set; }

		/**外部网店自己定义的Sku编号 */
		[XmlElement("outer_sku_id")]
			public string  Outer_sku_id{ get; set; }

		/**子订单来源 */
		[XmlElement("order_from")]
			public string  Order_from{ get; set; }

		/**应付金额（商品价格 * 商品数量 + 手工调整金额 - 子订单级订单优惠金额）。精确到2位小数;单位:元。如:200.07，表示:200元7分 */
		[XmlElement("total_fee")]
			public string  Total_fee{ get; set; }

		/**子订单实付金额。精确到2位小数，单位:元。如:200.07，表示:200元7分。对于多子订 单的交易，计算公式如下：payment = price * num + adjust_fee - discount_fee ；单子订单交易，payment与主订单的payment一致，对于退款成功的子订单，由于主订单的优惠分摊金额，会造成该字段可能不为0.00元。建议 使用退款前的实付金额减去退款单中的实际退款金额计算。 */
		[XmlElement("payment")]
			public string  Payment{ get; set; }

		/**子订单级订单优惠金额。精确到2位小数;单位:元。如:200.07，表示:200元7分 */
		[XmlElement("discount_fee")]
			public string  Discount_fee{ get; set; }

		/**手工调整金额.格式为:1.01;单位:元;精确到小数点后两位.（暂不提供） */
		[XmlElement("adjust_fee")]
			public string  Adjust_fee{ get; set; }

		/**订单修改时间 */
		[XmlElement("modified")]
			public string  Modified{ get; set; }

		/**SKU的值。如：机身颜色:黑色;手机套餐:官方标配（暂不提供） */
		[XmlElement("sku_properties_name")]
			public string  Sku_properties_name{ get; set; }

		/**最近退款ID（暂不提供） */
		[XmlElement("refund_id")]
			public long?  Refund_id{ get; set; }

		/**是否超卖 */
		[XmlElement("is_oversold")]
			public bool  Is_oversold{ get; set; }

		/**是否是服务订单，是返回true，否返回false。（暂不提供） */
		[XmlElement("is_service_order")]
			public bool  Is_service_order{ get; set; }

		/**子订单的交易结束时间 说明：子订单有单独的结束时间，与主订单的结束时间可能有所不同，在有退款发起的时候或者是主订单分阶段付款的时候，子订单的结束时间会早于主订单的结束时间，所以开放这个字段便于订单结束状态的判断（暂不提供） */
		[XmlElement("end_time")]
			public string  End_time{ get; set; }

		/**子订单发货时间，当卖家对订单进行了多次发货，子订单的发货时间和主订单的发货时间可能不一样了，那么就需要以子订单的时间为准。（没有进行多次发货的订单，主订单的发货时间和子订单的发货时间都一样）（暂不提供） */
		[XmlElement("consign_time")]
			public string  Consign_time{ get; set; }

		/**子订单的运送方式（卖家对订单进行多次发货之后，一个主订单下的子订单的运送方式可能不同，用order.shipping_type来区分子订单的运送方式）（暂不提供） */
		[XmlElement("shipping_type")]
			public string  Shipping_type{ get; set; }

		/**捆绑的子订单号，表示该子订单要和捆绑的子订单一起发货，用于卖家子订单捆绑发货（暂不提供） */
		[XmlElement("bind_oid")]
			public long?  Bind_oid{ get; set; }

		/**子订单发货的快递公司名称（暂不提供） */
		[XmlElement("logistics_company")]
			public string  Logistics_company{ get; set; }

		/**子订单所在包裹的运单号（暂不提供） */
		[XmlElement("invoice_no")]
			public string  Invoice_no{ get; set; }

		/**表示订单交易是否含有对应的代销采购单。 如果该订单中存在一个对应的代销采购单，那么该值为true；反之，该值为false。（暂不提供） */
		[XmlElement("is_daixiao")]
			public bool  Is_daixiao{ get; set; }

		/**分摊之后的实付金额 */
		[XmlElement("divide_order_fee")]
			public string  Divide_order_fee{ get; set; }

		/**优惠分摊 */
		[XmlElement("part_mjz_discount")]
			public string  Part_mjz_discount{ get; set; }

		/**对应门票有效期的外部id（暂不提供） */
		[XmlElement("ticket_outer_id")]
			public string  Ticket_outer_id{ get; set; }

		/**门票有效期的key（暂不提供） */
		[XmlElement("ticket_expdate_key")]
			public string  Ticket_expdate_key{ get; set; }

		/**发货的仓库编码 */
		[XmlElement("store_code")]
			public string  Store_code{ get; set; }

		/**子订单是否是www订单 */
		[XmlElement("is_www")]
			public bool  Is_www{ get; set; }

		/**套餐的值。如：M8原装电池:便携支架:M8专用座充:莫凡保护袋（暂不提供） */
		[XmlElement("item_meal_name")]
			public string  Item_meal_name{ get; set; }

		/**商品图片的绝对路径 */
		[XmlElement("pic_path")]
			public string  Pic_path{ get; set; }

		/**卖家昵称（暂不提供） */
		[XmlElement("seller_nick")]
			public string  Seller_nick{ get; set; }

		/**买家昵称（暂不提供） */
		[XmlElement("buyer_nick")]
			public string  Buyer_nick{ get; set; }

		/**退款状态。退款状态。可选值 WAIT_SELLER_AGREE(买家已经申请退款，等待卖家同意) WAIT_BUYER_RETURN_GOODS(卖家已经同意退款，等待买家退货) WAIT_SELLER_CONFIRM_GOODS(买家已经退货，等待卖家确认收货) SELLER_REFUSE_BUYER(卖家拒绝退款) CLOSED(退款关闭) SUCCESS(退款成功)（暂不提供） */
		[XmlElement("refund_status")]
			public string  Refund_status{ get; set; }

		/**商家外部编码(可与商家外部系统对接)。外部商家自己定义的系列商品的id */
		[XmlElement("outer_iid")]
			public string  Outer_iid{ get; set; }

		/**订单快照URL */
		[XmlElement("snapshot_url")]
			public string  Snapshot_url{ get; set; }

		/**订单快照详细信息 */
		[XmlElement("snapshot")]
			public string  Snapshot{ get; set; }

		/**订单超时到期时间。格式:yyyy-MM-dd HH:mm:ss */
		[XmlElement("timeout_action_time")]
			public string  Timeout_action_time{ get; set; }

		/**买家是否已评价。可选值：true(已评价)，false(未评价)（暂不提供） */
		[XmlElement("buyer_rate")]
			public bool  Buyer_rate{ get; set; }

		/**卖家是否已评价。可选值：true(已评价)，false(未评价)（暂不提供） */
		[XmlElement("seller_rate")]
			public bool  Seller_rate{ get; set; }

		/**卖家类型，可选值为：B（商城商家），C（普通卖家）（暂不提供） */
		[XmlElement("seller_type")]
			public string  Seller_type{ get; set; }

		/**交易商品对应的类目ID（暂不提供） */
		[XmlElement("cid")]
			public long?  Cid{ get; set; }

		/**订单状态（暂不提供） */
		[XmlElement("status")]
			public string  Status{ get; set; }

		/**单品订金金额 */
		[XmlElement("product_deposit")]
			public double?  Product_deposit{ get; set; }

	}
}
