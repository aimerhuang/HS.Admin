
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 微信配置
	/// </summary>
    /// <remarks>
    /// 2013-11-14 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class MkWeixinConfig
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 微信Token
		/// </summary>
		[Description("微信Token")]
		public string Token { get; set; }
 		/// <summary>
		/// AppId
		/// </summary>
		[Description("AppId")]
		public string AppId { get; set; }
 		/// <summary>
		/// AppSecret
		/// </summary>
		[Description("AppSecret")]
		public string AppSecret { get; set; }
 		/// <summary>
		/// 关注回复文本
		/// </summary>
		[Description("关注回复文本")]
		public string FollowText { get; set; }
 		/// <summary>
		/// 关注回复图片
		/// </summary>
		[Description("关注回复图片")]
		public string FollowImage { get; set; }
 		/// <summary>
		/// 关注回复类型:文本(10),图片(20)
		/// </summary>
		[Description("关注回复类型:文本(10),图片(20)")]
		public int FollowType { get; set; }
 		/// <summary>
		/// 消息回复文本
		/// </summary>
		[Description("消息回复文本")]
		public string MessageText { get; set; }
 		/// <summary>
		/// 消息回复图文
		/// </summary>
		[Description("消息回复图文")]
		public string MessageImage { get; set; }
 		/// <summary>
		/// 消息回复类型:文本(10),图片(20)
		/// </summary>
		[Description("消息回复类型:文本(10),图片(20)")]
		public int MessageType { get; set; }
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

	