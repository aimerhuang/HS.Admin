
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
	public partial class SyUser
	{
	  	/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 用户账号
		/// </summary>
		[Description("用户账号")]
		public string Account { get; set; }
 		/// <summary>
		/// 用户密码
		/// </summary>
		[Description("用户密码")]
		public string Password { get; set; }
 		/// <summary>
		/// 用户姓名
		/// </summary>
		[Description("用户姓名")]
		public string UserName { get; set; }
 		/// <summary>
		/// 用户手机
		/// </summary>
		[Description("用户手机")]
		public string MobilePhoneNumber { get; set; }
 		/// <summary>
		/// 电子邮箱
		/// </summary>
		[Description("电子邮箱")]
		public string EmailAddress { get; set; }
 		/// <summary>
		/// 状态：有效(1),无效(0)
		/// </summary>
		[Description("状态：有效(1),无效(0)")]
		public int Status { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreatedDate { get; set; }
 		/// <summary>
		/// 创建人
		/// </summary>
		[Description("创建人")]
		public int CreatedBy { get; set; }
 		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Description("最后更新时间")]
		public DateTime LastUpdateDate { get; set; }
 		/// <summary>
		/// 最后更新人
		/// </summary>
		[Description("最后更新人")]
		public int LastUpdateBy { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        [Description("微信公众号Openid")]
        public string OpenId { get; set; }
 	}
}

	