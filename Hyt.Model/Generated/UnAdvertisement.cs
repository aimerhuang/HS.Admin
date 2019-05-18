
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 联盟广告
	/// </summary>
    /// <remarks>
    /// 2013-10-14 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class UnAdvertisement
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 联盟网站系统编号
		/// </summary>
		[Description("联盟网站系统编号")]
		public int UnionSiteSysNo { get; set; }
 		/// <summary>
		/// 广告类型,CPC(10)，CPA(20),CPS(30)
		/// </summary>
		[Description("广告类型,CPC(10)，CPA(20),CPS(30)")]
		public int AdvertisementType { get; set; }
 		/// <summary>
		/// 有效访问Url
		/// </summary>
		[Description("有效访问Url")]
		public string AccessUrl { get; set; }
 		/// <summary>
		/// Url正则
		/// </summary>
		[Description("Url正则")]
		public string UrlRegex { get; set; }
 		/// <summary>
		/// 联盟通知代码
		/// </summary>
		[Description("联盟通知代码")]
		public string NotificationCode { get; set; }
 		/// <summary>
		/// 状态:启用(1),禁用(0)
		/// </summary>
		[Description("状态:启用(1),禁用(0)")]
		public int Status { get; set; }
 	}
}

	