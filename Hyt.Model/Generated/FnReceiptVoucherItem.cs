
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
	public partial class FnReceiptVoucherItem
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
		public int ReceiptVoucherSysNo { get; set; }
 		/// <summary>
		/// 事物编号
		/// </summary>
		[Description("事物编号")]
		public string TransactionSysNo { get; set; }
 		/// <summary>
		/// 支付方式系统编号
		/// </summary>
		[Description("支付方式系统编号")]
		public int PaymentTypeSysNo { get; set; }
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
		/// 交易(银行卡,信用卡)卡号
		/// </summary>
		[Description("交易(银行卡,信用卡)卡号")]
		public string CreditCardNumber { get; set; }

        ///<summary>
        ///收款方类型:财务中心(10),仓库(20),分销中心(30)
        ///</summary>
        [Description("收款方类型:财务中心(10),仓库(20),分销中心(30)")]
        public int ReceivablesSideType { get; set; }

        ///<summary>
        ///收款方编号
        ///</summary>
        [Description("收款方编号")]
        public int ReceivablesSideSysNo { get; set; }

        ///<summary>
        ///EAS收款科目编码
        ///</summary>
        [Description("EAS收款科目编码")]
        public string EasReceiptCode { get; set; }
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

	