
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-08-30 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class SpPromotionRule
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 规则名称
		/// </summary>
		[Description("规则名称")]
		public string Name { get; set; }
 		/// <summary>
		/// 规则描述
		/// </summary>
		[Description("规则描述")]
		public string Description { get; set; }
 		/// <summary>
		/// 前台展示文本
		/// </summary>
		[Description("前台展示文本")]
		public string FrontText { get; set; }
 		/// <summary>
		/// 成立时显示文本（显示前添加促销展示前缀,无前缀添加'
		/// </summary>
		[Description("成立时显示文本（显示前添加促销展示前缀,无前缀添加'")]
		public string TrueText { get; set; }
 		/// <summary>
		/// 不成立时显示文本（显示前添加促销展示前缀,无前缀添
		/// </summary>
		[Description("不成立时显示文本（显示前添加促销展示前缀,无前缀添")]
		public string FalseText { get; set; }
 		/// <summary>
		/// 规则脚本
		/// </summary>
		[Description("规则脚本")]
		public string RuleScript { get; set; }
 		/// <summary>
		/// 规则后台管理表单(Html)
		/// </summary>
		[Description("规则后台管理表单(Html)")]
		public string AdminHtml { get; set; }
 		/// <summary>
		/// 规则后台管理表单脚本
		/// </summary>
		[Description("规则后台管理表单脚本")]
		public string AdminScript { get; set; }
 		/// <summary>
		/// 促销应用类型:应用到分类(10);应用到商品(20);应用到
		/// </summary>
		[Description("促销应用类型:应用到分类(10);应用到商品(20);应用到")]
		public int PromotionType { get; set; }
 		/// <summary>
		/// 规则类型
		/// </summary>
		[Description("规则类型")]
		public int RuleType { get; set; }
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

	