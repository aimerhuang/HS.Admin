
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2014-01-06 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class SpPromotion
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 促销名称
		/// </summary>
		[Description("促销名称")]
		public string Name { get; set; }
 		/// <summary>
		/// 促销描述
		/// </summary>
		[Description("促销描述")]
		public string Description { get; set; }
 		/// <summary>
		/// 促销展示前缀
		/// </summary>
		[Description("促销展示前缀")]
		public string DisplayPrefix { get; set; }
 		/// <summary>
		/// 活动主题Url
		/// </summary>
		[Description("活动主题Url")]
		public string SubjectUrl { get; set; }
 		/// <summary>
		/// 促销应用类型:应用到分类(10);应用到商品(20);应用到
		/// </summary>
		[Description("促销应用类型:应用到分类(10);应用到商品(20);应用到")]
		public int PromotionType { get; set; }
 		/// <summary>
		/// 网站使用:是(1),否(0)
		/// </summary>
		[Description("网站使用:是(1),否(0)")]
		public int WebPlatform { get; set; }
 		/// <summary>
		/// 门店使用:是(1),否(0)
		/// </summary>
		[Description("门店使用:是(1),否(0)")]
		public int ShopPlatform { get; set; }
 		/// <summary>
		/// 手机商城使用:是(1),否(0)
		/// </summary>
		[Description("手机商城使用:是(1),否(0)")]
		public int MallAppPlatform { get; set; }
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
		/// 促销码
		/// </summary>
		[Description("促销码")]
		public string PromotionCode { get; set; }
 		/// <summary>
		/// 是否使用促销码:是(1),否(0)
		/// </summary>
		[Description("是否使用促销码:是(1),否(0)")]
		public int IsUsePromotionCode { get; set; }
 		/// <summary>
		/// 允许促销使用次数
		/// </summary>
		[Description("允许促销使用次数")]
		public int PromotionUseQuantity { get; set; }
 		/// <summary>
		/// 促销已使用次数
		/// </summary>
		[Description("促销已使用次数")]
		public int PromotionUsedQuantity { get; set; }
 		/// <summary>
		/// 用户使用次数限制
		/// </summary>
		[Description("用户使用次数限制")]
		public int UserUseQuantity { get; set; }
 		/// <summary>
		/// 促销优先级
		/// </summary>
		[Description("促销优先级")]
		public int Priority { get; set; }
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
        /// <summary>
        /// 物流App使用:是(1),否(0)
        /// </summary>
        [Description("物流App使用")]
        public int LogisticsAppPlatform { get; set; }
 	}
}

	