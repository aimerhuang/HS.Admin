
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-11-13 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class FnInvoice
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 发票类型系统编号
		/// </summary>
		[Description("发票类型系统编号")]
		public int InvoiceTypeSysNo { get; set; }
 		/// <summary>
		/// 发票代码
		/// </summary>
		[Description("发票代码")]
		public string InvoiceCode { get; set; }
 		/// <summary>
		/// 发票号码
		/// </summary>
		[Description("发票号码")]
		public string InvoiceNo { get; set; }
 		/// <summary>
		/// 发票金额
		/// </summary>
		[Description("发票金额")]
		public decimal InvoiceAmount { get; set; }
 		/// <summary>
		/// 发票备注
		/// </summary>
		[Description("发票备注")]
		public string InvoiceRemarks { get; set; }
 		/// <summary>
		/// 发票抬头
		/// </summary>
		[Description("发票抬头")]
		public string InvoiceTitle { get; set; }
 		/// <summary>
		/// 状态:待开票(10),已开票(20),已取回(30),作废(-10)
		/// </summary>
		[Description("状态:待开票(10),已开票(20),已取回(30),作废(-10)")]
		public int Status { get; set; }
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
		/// 事务编号
		/// </summary>
		[Description("事务编号")]
		public string TransactionSysNo { get; set; }
 	}
}

	