
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
	public partial class WhStockIn
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
		/// 仓库编号
		/// </summary>
		[Description("仓库编号")]
		public int WarehouseSysNo { get; set; }
 		/// <summary>
		/// 来源单据类型：出库单(10)、RMA单(20)、借货单(30)
		/// </summary>
		[Description("来源单据类型：出库单(10)、RMA单(20)、借货单(30)")]
		public int SourceType { get; set; }
 		/// <summary>
		/// 来源单据编号
		/// </summary>
		[Description("来源单据编号")]
		public int SourceSysNO { get; set; }
 		/// <summary>
		/// 入库物流方式：上门取件/取件单（10）、快递入库（20
		/// </summary>
		[Description("入库物流方式：上门取件/取件单（10）、快递入库（20")]
		public int DeliveryType { get; set; }
 		/// <summary>
		/// 备注
		/// </summary>
		[Description("备注")]
		public string Remarks { get; set; }
 		/// <summary>
		/// 是否已打印
		/// </summary>
		[Description("是否已打印")]
		public int IsPrinted { get; set; }
 		/// <summary>
		/// 状态：待入库（10）、部分入库（20）、已入库（50）、
		/// </summary>
		[Description("状态：待入库（10）、部分入库（20）、已入库（50）、")]
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
 		/// <summary>
		/// 时间戳
		/// </summary>
		[Description("时间戳")]
		public DateTime? Stamp { get; set; }
 	}
}

	