
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
	public partial class FnReceiptVoucher
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 事务编号
		/// </summary>
		[Description("事务编号")]
		public string TransactionSysNo { get; set; }
 		/// <summary>
		/// 收入类型:预付(10),到付(20)
		/// </summary>
		[Description("收入类型:预付(10),到付(20)")]
		public int IncomeType { get; set; }
 		/// <summary>
		/// 来源单据:销售单(10),退换货单(50)
		/// </summary>
		[Description("来源单据:销售单(10),退换货单(50)")]
		public int Source { get; set; }
 		/// <summary>
		/// 来源单据编号
		/// </summary>
		[Description("来源单据编号")]
		public int SourceSysNo { get; set; }
 		/// <summary>
		/// 应收金额
		/// </summary>
		[Description("应收金额")]
		public decimal IncomeAmount { get; set; }
 		/// <summary>
		/// 实收金额
		/// </summary>
		[Description("实收金额")]
		public decimal ReceivedAmount { get; set; }
 		/// <summary>
		/// 状态:待确认(10),已确认(20),作废(-10)
		/// </summary>
		[Description("状态:待确认(10),已确认(20),作废(-10)")]
		public int Status { get; set; }
 		/// <summary>
		/// 模板备注
		/// </summary>
		[Description("模板备注")]
		public string Remark { get; set; }
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
		/// 确认人
		/// </summary>
		[Description("确认人")]
		public int ConfirmedBy { get; set; }
 		/// <summary>
		/// 确认时间
		/// </summary>
		[Description("确认时间")]
		public DateTime ConfirmedDate { get; set; }
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
        public int? PaymentType { get; set; }
 	}
}

	