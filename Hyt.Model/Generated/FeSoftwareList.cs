
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 软件列表
	/// </summary>
    /// <remarks>
    /// 2014-01-14 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class FeSoftwareList
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 软件下载系统编号
		/// </summary>
		[Description("软件下载系统编号")]
		public int SoftwareSysNo { get; set; }
 		/// <summary>
		/// 软件名称
		/// </summary>
		[Description("软件名称")]
		public string Name { get; set; }
 		/// <summary>
		/// 下载地址
		/// </summary>
		[Description("下载地址")]
		public string DownloadUrl { get; set; }
 		/// <summary>
		/// 软件图标:普通(10),Iphone(20),Android(30)
		/// </summary>
		[Description("软件图标:普通(10),Iphone(20),Android(30)")]
		public int SoftIcon { get; set; }
 		/// <summary>
		/// 显示序号
		/// </summary>
		[Description("显示序号")]
		public int DisplayOrder { get; set; }
 		/// <summary>
		/// 版本号
		/// </summary>
		[Description("版本号")]
		public string VersionNumber { get; set; }
 	}
}

	