
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
	public partial class GsGroupShopping
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
		/// 团购标题
		/// </summary>
		[Description("团购标题")]
		public string Title { get; set; }
 		/// <summary>
		/// 团购标题
		/// </summary>
		[Description("团购标题")]
		public string Subtitle { get; set; }
 		/// <summary>
		/// 团购图片
		/// </summary>
		[Description("团购图片")]
		public string ImageUrl { get; set; }
 		/// <summary>
		/// 团购小图标
		/// </summary>
		[Description("团购小图标")]
		public string IconUrl { get; set; }
 		/// <summary>
		/// 团购描述
		/// </summary>
		[Description("团购描述")]
		public string Description { get; set; }
 		/// <summary>
		/// 团购起始时间
		/// </summary>
		[Description("团购起始时间")]
		public DateTime? StartTime { get; set; }
 		/// <summary>
		/// 团购截止时间
		/// </summary>
		[Description("团购截止时间")]
		public DateTime? EndTime { get; set; }
 		/// <summary>
		/// 团购数量上限
		/// </summary>
		[Description("团购数量上限")]
		public int MaxQuantity { get; set; }
 		/// <summary>
		/// 团购数量下限
		/// </summary>
		[Description("团购数量下限")]
		public int MinQuantity { get; set; }
 		/// <summary>
		/// 已团数量
		/// </summary>
		[Description("已团数量")]
		public int HaveQuantity { get; set; }
 		/// <summary>
		/// 限购数量
		/// </summary>
		[Description("限购数量")]
		public int LimitQuantity { get; set; }
 		/// <summary>
		/// 总价值
		/// </summary>
		[Description("总价值")]
		public decimal TotalPrice { get; set; }
 		/// <summary>
		/// 团购价
		/// </summary>
		[Description("团购价")]
		public decimal GroupPrice { get; set; }
 		/// <summary>
		/// 折扣
		/// </summary>
		[Description("折扣")]
		public decimal Discount { get; set; }
 		/// <summary>
		/// 促销系统编号
		/// </summary>
		[Description("促销系统编号")]
		public int PromotionSysNo { get; set; }
 		/// <summary>
		/// 团购类型:全部(0),商城(10),手机App(20),
		/// </summary>
		[Description("团购类型:全部(0),商城(10),手机App(20),")]
		public int GroupType { get; set; }
 		/// <summary>
		/// 支持区域类型:全国(0),百城当日达范围(10),指定区域(2
		/// </summary>
		[Description("支持区域类型:全国(0),百城当日达范围(10),指定区域(2")]
		public int SupportAreaType { get; set; }
 		/// <summary>
		/// 显示顺序
		/// </summary>
		[Description("显示顺序")]
		public int DisplayOrder { get; set; }
 		/// <summary>
		/// 状态:待审(10),已审(20),作废(-10)
		/// </summary>
		[Description("状态:待审(10),已审(20),作废(-10)")]
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
		/// 审核人
		/// </summary>
		[Description("审核人")]
		public int Auditor { get; set; }
 		/// <summary>
		/// 审核时间
		/// </summary>
		[Description("审核时间")]
		public DateTime AuditDate { get; set; }
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

	