
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 分销商升舱订单
	/// </summary>
    /// <remarks>
    /// 2013-09-10 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsOrder
	{
	    /// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 分销商商城系统编号
		/// </summary>
		[Description("分销商商城系统编号")]
		public int DealerMallSysNo { get; set; }
 		/// <summary>
		/// 商城订单号
        /// 淘宝分销存入分销流水号
		/// </summary>
		[Description("商城订单号")]
		public string MallOrderId { get; set; }
 		/// <summary>
		/// 商城订单事物编号
		/// </summary>
		[Description("商城订单事物编号")]
		public string OrderTransactionSysNo { get; set; }
 		/// <summary>
		/// 商城退换货事物编号
		/// </summary>
		[Description("商城退换货事物编号")]
		public string ReturnTransactionSysNo { get; set; }
 		/// <summary>
		/// 卖家昵称
		/// </summary>
		[Description("卖家昵称")]
		public string SellerNick { get; set; }
 		/// <summary>
		/// 买家昵称
		/// </summary>
		[Description("买家昵称")]
		public string BuyerNick { get; set; }
 		/// <summary>
		/// 省名称
		/// </summary>
		[Description("省名称")]
		public string Province { get; set; }
 		/// <summary>
		/// 市名称
		/// </summary>
		[Description("市名称")]
		public string City { get; set; }
 		/// <summary>
		/// 区名称
		/// </summary>
		[Description("区名称")]
		public string County { get; set; }
 		/// <summary>
		/// 详细地址
		/// </summary>
		[Description("详细地址")]
		public string StreetAddress { get; set; }
 		/// <summary>
		/// 邮费
		/// </summary>
		[Description("邮费")]
		public decimal PostFee { get; set; }
 		/// <summary>
		/// 升舱服务费
		/// </summary>
		[Description("升舱服务费")]
		public decimal ServiceFee { get; set; }
 		/// <summary>
		/// 优惠总金额
		/// </summary>
		[Description("优惠总金额")]
		public decimal DiscountAmount { get; set; }
 		/// <summary>
		/// 订单金额
		/// </summary>
		[Description("订单金额")]
		public decimal Payment { get; set; }
 		/// <summary>
		/// 付款时间
		/// </summary>
		[Description("付款时间")]
		public DateTime PayTime { get; set; }
 		/// <summary>
		/// 升舱时间
		/// </summary>
		[Description("升舱时间")]
		public DateTime UpgradeTime { get; set; }
 		/// <summary>
		/// 发货时间
		/// </summary>
		[Description("发货时间")]
		public DateTime DeliveryTime { get; set; }
 		/// <summary>
		/// 签收时间
		/// </summary>
		[Description("签收时间")]
		public DateTime SignTime { get; set; }
 		/// <summary>
		/// 是否需要回调:是(1),否(0)
		/// </summary>
		[Description("是否需要回调:是(1),否(0)")]
		public int IsCallback { get; set; }
 		/// <summary>
		/// 最后回调时间
		/// </summary>
		[Description("最后回调时间")]
		public DateTime LastCallbackTime { get; set; }
 		/// <summary>
		/// 状态:升舱中(10),已发货(20),已完成(30),失败(-10)
		/// </summary>
		[Description("状态:升舱中(10),已发货(20),已完成(30),失败(-10)")]
		public int Status { get; set; }
 	}
}

	