
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 分销商预存款主表
	/// </summary>
    /// <remarks>
    /// 2013-09-10 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsPrePayment
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 分销商系统编号
		/// </summary>
		[Description("分销商系统编号")]
		public int DealerSysNo { get; set; }
 		/// <summary>
		/// 累积预存金额
		/// </summary>
		[Description("累积预存金额")]
		public decimal TotalPrestoreAmount { get; set; }
 		/// <summary>
		/// 预存款可用余额
		/// </summary>
		[Description("预存款可用余额")]
		public decimal AvailableAmount { get; set; }
 		/// <summary>
		/// 预存款冻结金额
		/// </summary>
		[Description("预存款冻结金额")]
		public decimal FrozenAmount { get; set; }
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
        /// <summary>
        /// 余额提示额
        /// </summary>
        [Description("余额提示额")]
        public decimal AlertAmount { get; set; }
	}
}

	