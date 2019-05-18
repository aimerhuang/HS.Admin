
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
	public partial class LgDeliveryPrintTemplate
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 配送方式系统编号
		/// </summary>
		[Description("配送方式系统编号")]
		public int DeliveryTypeSysNo { get; set; }
 		/// <summary>
		/// 模板名称
		/// </summary>
		[Description("模板名称")]
		public string Name { get; set; }
 		/// <summary>
		/// 模板背景图
		/// </summary>
		[Description("模板背景图")]
		public string BackgroundImage { get; set; }
 		/// <summary>
		/// 模板
		/// </summary>
		[Description("模板")]
		public string Template { get; set; }
 		/// <summary>
		/// 状态:启用(1),禁用(0)
		/// </summary>
		[Description("状态:启用(1),禁用(0)")]
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

	