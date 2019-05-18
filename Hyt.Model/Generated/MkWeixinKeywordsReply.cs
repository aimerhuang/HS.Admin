
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 微信关键词回复
	/// </summary>
    /// <remarks>
    /// 2013-11-05 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class MkWeixinKeywordsReply
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 微信关键词系统编号
		/// </summary>
		[Description("微信关键词系统编号")]
		public int WeixinKeywordsSysNo { get; set; }
 		/// <summary>
		/// 回复类型:文本(10),图文(20)
		/// </summary>
		[Description("回复类型:文本(10),图文(20)")]
		public int ReplyType { get; set; }
 		/// <summary>
		/// 标题
		/// </summary>
		[Description("标题")]
		public string Title { get; set; }
 		/// <summary>
		/// 反馈内容
		/// </summary>
		[Description("反馈内容")]
		public string Content { get; set; }
 		/// <summary>
		/// 图片
		/// </summary>
		[Description("图片")]
		public string Image { get; set; }
 		/// <summary>
		/// 链接
		/// </summary>
		[Description("链接")]
		public string Hyperlink { get; set; }
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

	