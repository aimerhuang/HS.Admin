
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
	public partial class PdBrand
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 品牌名称
		/// </summary>
		[Description("品牌名称")]
		public string Name { get; set; }
 		/// <summary>
		/// 品牌图标
		/// </summary>
		[Description("品牌图标")]
		public string Logo { get; set; }
 		/// <summary>
		/// 排序
		/// </summary>
		[Description("排序")]
		public int DisplayOrder { get; set; }
 		/// <summary>
		/// 状态：启用（1）、禁用（0）
		/// </summary>
		[Description("状态：启用（1）、禁用（0）")]
		public int Status { get; set; }
 	}
}

	