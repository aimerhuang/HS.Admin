
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 邮件表
	/// </summary>
    /// <remarks>
    /// 2013-10-08 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class NcEmail
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 接收邮件
		/// </summary>
		[Description("接收邮件")]
		public string MailAddress { get; set; }
 		/// <summary>
		/// 邮件标题
		/// </summary>
		[Description("邮件标题")]
		public string MailSubject { get; set; }
 		/// <summary>
		/// 邮件内容
		/// </summary>
		[Description("邮件内容")]
		public string MailBody { get; set; }
 		/// <summary>
		/// 邮件类型:通知(10);广告(20);活动(30)
		/// </summary>
		[Description("邮件类型:通知(10);广告(20);活动(30)")]
		public int MailType { get; set; }
 		/// <summary>
		/// 状态:待发(10);已发(20)失败;(-10)
		/// </summary>
		[Description("状态:待发(10);已发(20)失败;(-10)")]
		public int Status { get; set; }
 		/// <summary>
		/// 发送失败次数
		/// </summary>
		[Description("发送失败次数")]
		public int ErrorQuantity { get; set; }
 		/// <summary>
		/// 期望发送时间
		/// </summary>
		[Description("期望发送时间")]
		public DateTime ExpectSendTime { get; set; }
 		/// <summary>
		/// 处理时间
		/// </summary>
		[Description("处理时间")]
		public DateTime HandleTime { get; set; }
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

	