
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 微信关键词
	/// </summary>
    /// <remarks>
    /// 2013-11-05 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class MkWeixinKeywords
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 关键词(多个关键词分号分隔)
		/// </summary>
		[Description("关键词(多个关键词分号分隔)")]
		public string Keywords { get; set; }
 		/// <summary>
		/// 回复数量
		/// </summary>
		[Description("回复数量")]
		public int ReplyCount { get; set; }
 		/// <summary>
		/// 状态:启用(1),禁用(0)
		/// </summary>
		[Description("状态:启用(1),禁用(0)")]
		public int Status { get; set; }
        /// <summary>
        /// 代理商系统编号
        /// </summary>
        [Description("代理商系统编号")]
        public int AgentSysNo { get; set; }
        /// <summary>
        /// 经销商系统编号
        /// </summary>
        [Description("经销商系统编号")]
        public int DealerSysNo { get; set; }
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

	