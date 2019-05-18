
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
	public partial class FeProductGroup
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 平台类型：PC网站(10),手机商城(30),商城Iphone
		/// </summary>
		[Description("平台类型：PC网站(10),手机商城(30),商城Iphone")]
		public int PlatformType { get; set; }
 		/// <summary>
		/// 商品组图标
		/// </summary>
		[Description("商品组图标")]
		public string ProductGroupIcon { get; set; }
 		/// <summary>
		/// 组名称
		/// </summary>
		[Description("组名称")]
		public string Name { get; set; }
 		/// <summary>
		/// 组名称颜色
		/// </summary>
		[Description("组名称颜色")]
		public string NameColor { get; set; }
 		/// <summary>
		/// 代码
		/// </summary>
		[Description("代码")]
		public string Code { get; set; }
 		/// <summary>
		/// 显示顺序
		/// </summary>
		[Description("显示顺序")]
		public int DisplayOrder { get; set; }
 		/// <summary>
		/// 状态：启用（1）、禁用（0）
		/// </summary>
		[Description("状态：启用（1）、禁用（0）")]
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
        /// 前台显示名称
        /// </summary>
        [Description("前台显示名称")]
        public string DisplayName { get; set; }
 	}
}

	