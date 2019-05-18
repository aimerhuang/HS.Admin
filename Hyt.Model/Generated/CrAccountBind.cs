
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 客户账户绑定
	/// </summary>
    /// <remarks>
    /// 2014-04-14 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class CrAccountBind
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 客户系统编号
		/// </summary>
		[Description("客户系统编号")]
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 绑定类型:路由器(10),微信(50)
		/// </summary>
		[Description("绑定类型:路由器(10),微信(50)")]
		public int BindType { get; set; }
 		/// <summary>
		/// 绑定账号Id
		/// </summary>
		[Description("绑定账号Id")]
		public string BindId { get; set; }
 		/// <summary>
		/// 授权码
		/// </summary>
		[Description("授权码")]
		public string AuthCode { get; set; }
 		/// <summary>
		/// 绑定时间
		/// </summary>
		[Description("绑定时间")]
		public DateTime BindTime { get; set; }
 		/// <summary>
		/// 授权时间
		/// </summary>
		[Description("授权时间")]
		public DateTime AuthTime { get; set; }
 		/// <summary>
		/// 解绑时间
		/// </summary>
		[Description("解绑时间")]
		public DateTime UnbindTime { get; set; }
 		/// <summary>
		/// 状态:绑定(1),解绑(0)
		/// </summary>
		[Description("状态:绑定(1),解绑(0)")]
		public int Status { get; set; }
 	}
}

	