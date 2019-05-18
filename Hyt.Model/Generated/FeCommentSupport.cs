
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
	public partial class FeCommentSupport
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 商品评论系统编号
		/// </summary>
		[Description("商品评论系统编号")]
		public int ProductCommentSysNo { get; set; }
 		/// <summary>
		/// 会员编号
		/// </summary>
		[Description("会员编号")]
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 有用
		/// </summary>
		[Description("有用")]
		public int SupportCount { get; set; }
 		/// <summary>
		/// 没用
		/// </summary>
		[Description("没用")]
		public int UnSupportCount { get; set; }
 		/// <summary>
		/// 创建人
		/// </summary>
		[Description("创建人")]
		public int CreatedBy { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreateDate { get; set; }
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

	