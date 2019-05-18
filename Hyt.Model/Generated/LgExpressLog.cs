
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 物流配送日志
	/// </summary>
    /// <remarks>
    /// 2014-04-04 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class LgExpressLog
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 物流配送信息系统编号
		/// </summary>
		[Description("物流配送信息系统编号")]
		public int ExpressInfoSysNo { get; set; }
 		/// <summary>
		/// 日志内容
		/// </summary>
		[Description("日志内容")]
		public string LogContext { get; set; }
 		/// <summary>
		/// 日志时间
		/// </summary>
		[Description("日志时间")]
		public DateTime LogTime { get; set; }
 		/// <summary>
		/// 地区代码
		/// </summary>
		[Description("地区代码")]
		public string AreaCode { get; set; }
 		/// <summary>
		/// 地区名称
		/// </summary>
		[Description("地区名称")]
		public string AreaName { get; set; }
 		/// <summary>
		/// 状态:"在途"
		/// </summary>
		[Description("状态:在途")]
		public string Status { get; set; }
 	}
}

	