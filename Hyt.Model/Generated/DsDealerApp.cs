
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 分销商App管理
	/// </summary>
    /// <remarks>
    /// 2014-05-05 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsDealerApp
	{
	  	/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 分销商城类型系统编号
		/// </summary>
		[Description("分销商城类型系统编号")]
		public int MallTypeSysNo { get; set; }
 		/// <summary>
		/// App名称
		/// </summary>
		[Description("App名称")]
		public string AppName { get; set; }
 		/// <summary>
		/// AppKey
		/// </summary>
		[Description("AppKey")]
		public string AppKey { get; set; }
 		/// <summary>
		/// AppSecret
		/// </summary>
		[Description("AppSecret")]
		public string AppSecret { get; set; }
 		/// <summary>
		/// 最大使用数
		/// </summary>
		[Description("最大使用数")]
		public int MaxRelevance { get; set; }
 		/// <summary>
		/// 已使用数
		/// </summary>
		[Description("已使用数")]
		public int HasRelevance { get; set; }
 		/// <summary>
		/// 状态:启用(1),禁用(0)
		/// </summary>
		[Description("状态:启用(1),禁用(0)")]
		public int Status { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
       [Description("扩展字段")]
        public string Extend { get; set; }
 	}
}

	