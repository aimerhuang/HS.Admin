
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
	public partial class SpCombo
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
        /// <summary>
        /// 分销商
        /// </summary>
        [Description("分销商")]
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        [Description("仓库")]
        public int WarehouseSysNo { get; set; }
 		/// <summary>
		/// 促销系统编号
		/// </summary>
		[Description("促销系统编号")]
		public int PromotionSysNo { get; set; }
 		/// <summary>
		/// 套餐名称
		/// </summary>
		[Description("套餐名称")]
		public string Title { get; set; }
 		/// <summary>
		/// 开始时间
		/// </summary>
		[Description("开始时间")]
		public DateTime StartTime { get; set; }
 		/// <summary>
		/// 结束时间
		/// </summary>
		[Description("结束时间")]
		public DateTime EndTime { get; set; }
 		/// <summary>
		/// 套餐数
		/// </summary>
		[Description("套餐数")]
		public int ComboQuantity { get; set; }
 		/// <summary>
		/// 已销售数
		/// </summary>
		[Description("已销售数")]
		public int SaleQuantity { get; set; }
 		/// <summary>
		/// 状态:待审(10),已审(20),作废(-10)
		/// </summary>
		[Description("状态:待审(10),已审(20),作废(-10)")]
		public int Status { get; set; }
 		/// <summary>
		/// 审核人
		/// </summary>
		[Description("审核人")]
		public int AuditorSysNo { get; set; }
 		/// <summary>
		/// 审核时间
		/// </summary>
		[Description("审核时间")]
		public DateTime AuditDate { get; set; }
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

	