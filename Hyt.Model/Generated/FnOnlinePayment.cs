
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// </summary>
    /// <remarks>
    /// 2013-08-27 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class FnOnlinePayment
	{
	  	/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 单据来源:销售单(10)
		/// </summary>
		[Description("单据来源:销售单(10)")]
		public int Source { get; set; }
 		/// <summary>
		/// 来源单据编号
		/// </summary>
		[Description("来源单据编号")]
		public int SourceSysNo { get; set; }
        /// <summary>
        /// 商户订单编号
        /// </summary>
        [Description("商户订单编号")]
        public string BusinessOrderSysNo { get; set; }
 		/// <summary>
		/// 金额
		/// </summary>
		[Description("金额")]
		public decimal Amount { get; set; }
 		/// <summary>
		/// 支付方式编号
		/// </summary>
		[Description("支付方式编号")]
		public int PaymentTypeSysNo { get; set; }
 		/// <summary>
		/// 交易凭证
		/// </summary>
		[Description("交易凭证")]
		public string VoucherNo { get; set; }
 		/// <summary>
		/// 状态:有效(1),无效(0)
		/// </summary>
		[Description("状态:有效(1),无效(0)")]
		public int Status { get; set; }
 		/// <summary>
		/// 创建人
		/// </summary>
		[Description("创建人")]
		public int CreatedBy { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreatedDate { get; set; }
 		/// <summary>
		/// 操作人
		/// </summary>
		[Description("操作人")]
		public int Operator { get; set; }
 		/// <summary>
		/// 操作时间
		/// </summary>
		[Description("操作时间")]
		public DateTime OperatedDate { get; set; }
 		/// <summary>
		/// 最后更新人
		/// </summary>
		[Description("最后更新人")]
		public int LastUpdateBy { get; set; }
 		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Description("最后更新时间")]
		public DateTime LastUpdateDate { get; set; }
 	}
}

	