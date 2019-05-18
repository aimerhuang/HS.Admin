
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 事物处理级别
	/// </summary>
    /// <remarks>
    /// 2013-08-27 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class BsProcessLevel
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 来源:销售单(10),
		/// </summary>
		[Description("来源:销售单(10),")]
		public int Source { get; set; }
 		/// <summary>
		/// 来源编号
		/// </summary>
		[Description("来源编号")]
		public int SourceSysNo { get; set; }
 		/// <summary>
		/// 事物编号
		/// </summary>
		[Description("事物编号")]
		public string TransactionSysNo { get; set; }
 		/// <summary>
		/// 事物处理级别(0-5级,级别越高,处理优先级越高)
		/// </summary>
		[Description("事物处理级别(0-5级,级别越高,处理优先级越高)")]
		public int ProcessLevel { get; set; }
 	}
}

	