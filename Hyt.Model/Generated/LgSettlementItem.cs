
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-11-14 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class LgSettlementItem
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
		public int SettlementSysNo { get; set; }
 		/// <summary>
		/// 事务编号
		/// </summary>
		[Description("事务编号")]
		public string TransactionSysNo { get; set; }
 		/// <summary>
		/// 配送单系统编号
		/// </summary>
		[Description("配送单系统编号")]
		public int DeliverySysNo { get; set; }
 		/// <summary>
		/// 出库单系统编号
		/// </summary>
		[Description("出库单系统编号")]
		public int StockOutSysNo { get; set; }
 		/// <summary>
		/// 支付方式
		/// </summary>
		[Description("支付方式")]
		public int PayType { get; set; }
 		/// <summary>
		/// 支付金额
		/// </summary>
		[Description("支付金额")]
		public decimal PayAmount { get; set; }
 		/// <summary>
		/// 支付单号
		/// </summary>
		[Description("支付单号")]
		public string PayNo { get; set; }
 		/// <summary>
		/// 状态:待结算（10）、已结算（20）、作废（－10）
		/// </summary>
		[Description("状态:待结算（10）、已结算（20）、作废（－10）")]
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

	