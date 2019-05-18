
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
	public partial class SySystemLog
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 日志级别:10:Debug,20:Info,30:Warn,40:Error,50:Fata
		/// </summary>
		[Description("日志级别:10:Debug,20:Info,30:Warn,40:Error,50:Fata")]
		public int LogLevel { get; set; }
 		/// <summary>
		/// 来源:前台(10),后台(20),商城IphoneApp(31),商城Andro
		/// </summary>
		[Description("来源:前台(10),后台(20),商城IphoneApp(31),商城Andro")]
		public int Source { get; set; }
 		/// <summary>
		/// 操作目标类型:枚举值见('系统日志目标类型‘)
		/// </summary>
		[Description("操作目标类型:枚举值见('系统日志目标类型‘)")]
		public int TargetType { get; set; }
 		/// <summary>
		/// 操作目标系统编号
		/// </summary>
		[Description("操作目标系统编号")]
		public int TargetSysNo { get; set; }
 		/// <summary>
		/// 异常信息
		/// </summary>
		[Description("异常信息")]
		public string Exception { get; set; }
 		/// <summary>
		/// 操作人姓名
		/// </summary>
		[Description("操作人姓名")]
		public string OperatorName { get; set; }
 		/// <summary>
		/// 操作人
		/// </summary>
		[Description("操作人")]
		public int Operator { get; set; }
 		/// <summary>
		/// 操作人IP
		/// </summary>
		[Description("操作人IP")]
		public string LogIp { get; set; }
 		/// <summary>
		/// 操作时间
		/// </summary>
		[Description("操作时间")]
		public DateTime LogDate { get; set; }
 		/// <summary>
		/// 日志内容
		/// </summary>
		[Description("日志内容")]
		public string Message { get; set; }
 	}
}

	