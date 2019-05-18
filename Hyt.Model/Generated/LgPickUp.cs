
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-11-22 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class LgPickUp
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 取件方式系统编号
		/// </summary>
		[Description("取件方式系统编号")]
		public int PickupTypeSysNo { get; set; }
 		/// <summary>
		/// 仓库编号
		/// </summary>
		[Description("仓库编号")]
		public int WarehouseSysNo { get; set; }
 		/// <summary>
		/// 入库单系统编号
		/// </summary>
		[Description("入库单系统编号")]
		public int StockInSysNo { get; set; }
 		/// <summary>
		/// 取件地址系统编号
		/// </summary>
		[Description("取件地址系统编号")]
		public int PickupAddressSysNo { get; set; }
 		/// <summary>
		/// 总金额(明细信用价之和)
		/// </summary>
		[Description("总金额(明细信用价之和)")]
		public decimal TotalAmount { get; set; }
 		/// <summary>
		/// 状态：待取件（10）、取件中(15)、已取件（20）、已入
		/// </summary>
		[Description("状态：待取件（10）、取件中(15)、已取件（20）、已入")]
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
 		/// <summary>
		/// 时间戳
		/// </summary>
		[Description("时间戳")]
		public DateTime Stamp { get; set; }
 	}
}

	