
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-08-27 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class FnPaymentVoucher
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 事物编号
		/// </summary>
		[Description("事物编号")]
		public string TransactionSysNo { get; set; }
 		/// <summary>
        /// 单据来源:销售单(10),退换货(50),提现单(60)
		/// </summary>
        [Description("单据来源:销售单(10),退换货(50),提现单(60)")]
		public int Source { get; set; }
 		/// <summary>
		/// 单据来源编号
		/// </summary>
		[Description("单据来源编号")]
		public int SourceSysNo { get; set; }
 		/// <summary>
		/// 应付金额
		/// </summary>
		[Description("应付金额")]
		public decimal PayableAmount { get; set; }
 		/// <summary>
		/// 已付金额
		/// </summary>
		[Description("已付金额")]
		public decimal PaidAmount { get; set; }
 		/// <summary>
		/// 收款人编号
		/// </summary>
		[Description("收款人编号")]
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 收款人开户行
		/// </summary>
		[Description("收款人开户行")]
		public string RefundBank { get; set; }
 		/// <summary>
		/// 收款人开户姓名
		/// </summary>
		[Description("收款人开户姓名")]
		public string RefundAccountName { get; set; }
 		/// <summary>
		/// 收款人银行账号
		/// </summary>
		[Description("收款人银行账号")]
		public string RefundAccount { get; set; }
 		/// <summary>
		/// 状态:待付款(10),部分付款(15),已付款(20),作废(-10)
		/// </summary>
		[Description("状态:待付款(10),部分付款(15),已付款(20),作废(-10)")]
		public int Status { get; set; }
 		/// <summary>
		/// 备注
		/// </summary>
		[Description("备注")]
		public string Remarks { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreatedDate { get; set; }
 		/// <summary>
		/// 创建人
		/// </summary>
		[Description("创建人")]
		public int CreatedBy { get; set; }
 		/// <summary>
		/// 付款确认人
		/// </summary>
		[Description("付款确认人")]
		public int PayerSysNo { get; set; }
 		/// <summary>
		/// 付款确认时间
		/// </summary>
		[Description("付款确认时间")]
		public DateTime PayDate { get; set; }
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
        /// <summary>
        /// 支付类型
        /// </summary>
        [Description("支付类型")]
        public int PaymentType { get; set; }
 	}
}

	