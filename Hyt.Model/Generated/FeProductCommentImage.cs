
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
	public partial class FeProductCommentImage
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int CommentSysNo { get; set; }
 		/// <summary>
		/// 会员编号
		/// </summary>
		[Description("会员编号")]
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 图片路径
		/// </summary>
		[Description("图片路径")]
		public string ImagePath { get; set; }
 		/// <summary>
		/// 状态：待审（10）、已审（20）、作废（－10）
		/// </summary>
		[Description("状态：待审（10）、已审（20）、作废（－10）")]
		public int Status { get; set; }
 	}
}

	