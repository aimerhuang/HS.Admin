
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
	public partial class ApVersion
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// App代码：商城Android(10001),商城Ios(20001),百城通(
		/// </summary>
		[Description("App代码：商城Android(10001),商城Ios(20001),百城通(10101)")]
		public int AppCode { get; set; }
 		/// <summary>
		/// 版本号
		/// </summary>
		[Description("版本号")]
		public string VersionNumber { get; set; }
 		/// <summary>
		/// 版本地址
		/// </summary>
		[Description("版本地址")]
		public string VersionLink { get; set; }
 		/// <summary>
		/// 升级信息
		/// </summary>
		[Description("升级信息")]
		public string UpgradeInfo { get; set; }
 		/// <summary>
		/// 描述
		/// </summary>
		[Description("描述")]
		public string Description { get; set; }
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

	