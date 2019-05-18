
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 联盟网站
	/// </summary>
    /// <remarks>
    /// 2013-10-14 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class UnUnionSite
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 网站
		/// </summary>
		[Description("网站")]
		public string SiteName { get; set; }
 		/// <summary>
		/// 网站Url
		/// </summary>
		[Description("网站Url")]
		public string SiteUrl { get; set; }
 		/// <summary>
		/// 联系人
		/// </summary>
		[Description("联系人")]
		public string Contact { get; set; }
 		/// <summary>
		/// 电话
		/// </summary>
		[Description("电话")]
		public string Telephone { get; set; }
 		/// <summary>
		/// 手机
		/// </summary>
		[Description("手机")]
		public string MobilePhoneNumber { get; set; }
 		/// <summary>
		/// 有效日期起
		/// </summary>
		[Description("有效日期起")]
		public DateTime BeginTime { get; set; }
 		/// <summary>
		/// 有效日期止
		/// </summary>
		[Description("有效日期止")]
		public DateTime EndTime { get; set; }
 		/// <summary>
		/// 状态:启用(1),禁用(0)
		/// </summary>
		[Description("状态:启用(1),禁用(0)")]
		public int Status { get; set; }
 	}
}

	