
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 商品信息通知
	/// </summary>
    /// <remarks>
    /// 2013-08-27 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class CrNotice
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
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 商品编号
		/// </summary>
		[Description("商品编号")]
		public int ProductSysNo { get; set; }
 		/// <summary>
		/// 通知类型：降价通知（10）、到货通知（20）
		/// </summary>
		[Description("通知类型：降价通知（10）、到货通知（20）")]
		public int NoticeType { get; set; }
 		/// <summary>
		/// 通知方式：短信（10）、邮件（20）
		/// </summary>
		[Description("通知方式：短信（10）、邮件（20）")]
		public int NoticeWay { get; set; }
 		/// <summary>
		/// 电子邮箱
		/// </summary>
		[Description("电子邮箱")]
		public string EmailAddress { get; set; }
 		/// <summary>
		/// 手机号码
		/// </summary>
		[Description("手机号码")]
		public string MobilePhoneNumber { get; set; }
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

	