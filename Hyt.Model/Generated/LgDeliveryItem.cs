
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
	public partial class LgDeliveryItem
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
		public int DeliverySysNo { get; set; }
 		/// <summary>
		/// 事务编号
		/// </summary>
		[Description("事务编号")]
		public string TransactionSysNo { get; set; }
 		/// <summary>
		/// 单据类型：出库单（10）、取件单（20）
		/// </summary>
		[Description("单据类型：出库单（10）、取件单（20）")]
		public int NoteType { get; set; }
 		/// <summary>
		/// 单据编号
		/// </summary>
		[Description("单据编号")]
		public int NoteSysNo { get; set; }
 		/// <summary>
		/// 是否到付：是（1）、否（0）
		/// </summary>
		[Description("是否到付：是（1）、否（0）")]
		public int IsCOD { get; set; }
 		/// <summary>
		/// 出库单金额
		/// </summary>
		[Description("出库单金额")]
		public decimal StockOutAmount { get; set; }
 		/// <summary>
		/// 应收金额
		/// </summary>
		[Description("应收金额")]
		public decimal Receivable { get; set; }
 		/// <summary>
		/// 支付类型：预付（10）、到付（20）
		/// </summary>
		[Description("支付类型：预付（10）、到付（20）")]
		public int PaymentType { get; set; }
 		/// <summary>
		/// 支付单号
		/// </summary>
		[Description("支付单号")]
		public string PayNo { get; set; }
 		/// <summary>
		/// 收货地址（SoReceiveAddress）
		/// </summary>
		[Description("收货地址（SoReceiveAddress）")]
		public int AddressSysNo { get; set; }
 		/// <summary>
		/// 快递单号
		/// </summary>
		[Description("快递单号")]
		public string ExpressNo { get; set; }
 		/// <summary>
		/// 状态：待签收（10）、拒收（20）、未送达（30）、已签
		/// </summary>
        [Description("状态：待签收（10）、拒收（20）、未送达/未取件（30）、已签收/已取件（50）、作废（－10）")]
		public int Status { get; set; }
 		/// <summary>
		/// 备注
		/// </summary>
		[Description("备注")]
		public string Remarks { get; set; }
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

	