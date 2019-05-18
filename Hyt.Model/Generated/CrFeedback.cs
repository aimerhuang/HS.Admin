
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
	public partial class CrFeedback
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 意见反馈类型系统编号
		/// </summary>
		[Description("意见反馈类型系统编号")]
		public int FeedbackTypeSysNo { get; set; }
 		/// <summary>
		/// 来源:商城(10),IphoneApp(20),AndroidApp(30)
		/// </summary>
		[Description("来源:商城(10),IphoneApp(20),AndroidApp(30)")]
		public int Source { get; set; }
 		/// <summary>
		/// 客户系统编号
		/// </summary>
		[Description("客户系统编号")]
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 姓名
		/// </summary>
		[Description("姓名")]
		public string Name { get; set; }
 		/// <summary>
		/// 电话
		/// </summary>
		[Description("电话")]
		public string Phone { get; set; }
 		/// <summary>
		/// 邮箱
		/// </summary>
		[Description("邮箱")]
		public string Email { get; set; }
 		/// <summary>
		/// 反馈内容
		/// </summary>
		[Description("反馈内容")]
		public string Content { get; set; }
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
 	}
}

	