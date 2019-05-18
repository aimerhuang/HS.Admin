
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
	public partial class WhProductLend
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 仓库编号
		/// </summary>
		[Description("仓库编号")]
		public int WarehouseSysNo { get; set; }
 		/// <summary>
		/// 配送员系统编号
		/// </summary>
		[Description("配送员系统编号")]
		public int DeliveryUserSysNo { get; set; }
 		/// <summary>
		/// 是否在额度不够时强制放行：是（1）、否（0）
		/// </summary>
		[Description("是否在额度不够时强制放行：是（1）、否（0）")]
		public int IsEnforceAllow { get; set; }
 		/// <summary>
		/// 金额(业务员信用价格)
		/// </summary>
		[Description("金额(业务员信用价格)")]
		public decimal Amount { get; set; }
 		/// <summary>
		/// 状态:待出库(10),已出库(20),已完成[已补单已还货](30
		/// </summary>
		[Description("状态:待出库(10),已出库(20),已完成[已补单已还货](30")]
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
		/// 出库人
		/// </summary>
		[Description("出库人")]
		public int StockOutBy { get; set; }
 		/// <summary>
		/// 出库时间
		/// </summary>
		[Description("出库时间")]
		public DateTime StockOutDate { get; set; }
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

	