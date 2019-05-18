
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
	public partial class FnDaybook
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 来源单据:收款单(10),付款单(20)
		/// </summary>
		[Description("来源单据:收款单(10),付款单(20)")]
		public int Source { get; set; }
 		/// <summary>
		/// 来源单据编号
		/// </summary>
		[Description("来源单据编号")]
		public int SourceSysNo { get; set; }
 		/// <summary>
		/// 交易金额
		/// </summary>
		[Description("交易金额")]
		public decimal Amount { get; set; }
 		/// <summary>
		/// 交易凭证号
		/// </summary>
		[Description("交易凭证号")]
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

	