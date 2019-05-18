
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 分销商退换货单
	/// </summary>
    /// <remarks>
    /// 2013-09-10 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsReturn
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
		/// 商城退换货事物编号
		/// </summary>
		[Description("商城退换货事物编号")]
		public string ReturnTransactionSysNo { get; set; }
 		/// <summary>
		/// 商城退换货单号
		/// </summary>
		[Description("商城退换货单号")]
		public int RcReturnSysNo { get; set; }
 		/// <summary>
		/// 商城订单号
		/// </summary>
		[Description("商城订单号")]
		public string MallOrderId { get; set; }
 		/// <summary>
		/// 买家昵称
		/// </summary>
		[Description("买家昵称")]
		public string BuyerNick { get; set; }
 		/// <summary>
		/// RMA类型:售后换货(10),售后退货(20)
		/// </summary>
		[Description("RMA类型:售后换货(10),售后退货(20)")]
		public int RmaType { get; set; }
 		/// <summary>
		/// 商城退款金额
		/// </summary>
		[Description("商城退款金额")]
		public decimal MallReturnAmount { get; set; }
 		/// <summary>
		/// 申请退款时间
		/// </summary>
		[Description("申请退款时间")]
		public DateTime ApplicationTime { get; set; }
 		/// <summary>
		/// 商城退货单号
		/// </summary>
		[Description("商城退货单号")]
		public string MallReturnId { get; set; }
 		/// <summary>
		/// 买家备注
		/// </summary>
		[Description("买家备注")]
		public string BuyerRemark { get; set; }
 		/// <summary>
		/// 退款备注
		/// </summary>
		[Description("退款备注")]
		public string RmaRemark { get; set; }
 	}
}

	