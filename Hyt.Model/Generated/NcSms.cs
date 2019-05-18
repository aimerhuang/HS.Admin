
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 短信表
	/// </summary>
    /// <remarks>
    /// 2013-10-08 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class      NcSms
	{
	  	/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 手机号码
		/// </summary>
		[Description("手机号码")]
		public string MobilePhoneNumber { get; set; }
 		/// <summary>
		/// 短信内容
		/// </summary>
		[Description("短信内容")]
		public string SmsContent { get; set; }
 		/// <summary>
		/// 优先级
		/// </summary>
		[Description("优先级")]
		public int Priority { get; set; }
 		/// <summary>
		/// 状态待发(10),已发(20),作废(-10)
		/// </summary>
		[Description("状态待发(10),已发(20),作废(-10)")]
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

	