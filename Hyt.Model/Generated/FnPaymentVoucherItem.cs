
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
	public partial class FnPaymentVoucherItem
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int PaymentVoucherSysNo { get; set; }
 		/// <summary>
		/// 事物编号
		/// </summary>
		[Description("事物编号")]
		public string TransactionSysNo { get; set; }
 		/// <summary>
		/// 原收款方式系统编号
		/// </summary>
		[Description("原收款方式系统编号")]
		public int OriginalPaymentTypeSysNo { get; set; }
 		/// <summary>
		/// 原交易凭证号
		/// </summary>
		[Description("原交易凭证号")]
		public string OriginalVoucherNo { get; set; }
 		/// <summary>
		/// 付款方式:网银(10),支付宝(20),转账(30),现金(40)
		/// </summary>
		[Description("付款方式:网银(10),支付宝(20),转账(30),现金(40)")]
		public int PaymentType { get; set; }
 		/// <summary>
		/// 金额
		/// </summary>
		[Description("金额")]
		public decimal Amount { get; set; }
 		/// <summary>
		/// 凭证号
		/// </summary>
		[Description("凭证号")]
		public string VoucherNo { get; set; }
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

        ///<summary>
        ///付款方类型:财务中心(10),仓库(20),分销中心(30)
        ///</summary>
        [Description("付款方类型:财务中心(10),仓库(20),分销中心(30)")]
        public int PaymentToType { get; set; }

        ///<summary>
        ///付款方编号
        ///</summary>
        [Description("付款方编号")]
        public int PaymentToSysNo { get; set; }

 		/// <summary>
		/// 状态:待付款(10),已付款(20),作废(-10)
		/// </summary>
		[Description("状态:待付款(10),已付款(20),作废(-10)")]
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

 	}
}

	