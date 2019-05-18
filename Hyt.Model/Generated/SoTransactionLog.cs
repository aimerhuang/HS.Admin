
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-08-27 杨浩 T4生成
    /// </remarks>
	[Serializable]
    [DataContract]
	public partial class SoTransactionLog
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
        [DataMember]
		public int SysNo { get; set; }
 		/// <summary>
		/// 事务编号
		/// </summary>
		[Description("事务编号")]
		public string TransactionSysNo { get; set; }
 		/// <summary>
		/// 日志内容
		/// </summary>
		[Description("日志内容")]
        [DataMember]
		public string LogContent { get; set; }
 		/// <summary>
		/// 操作人姓名
		/// </summary>
		[Description("操作人姓名")]
		public string Operator { get; set; }
 		/// <summary>
		/// 操作时间
		/// </summary>
		[Description("操作时间")]
        [DataMember]
		public DateTime OperateDate { get; set; }
 	}
}

	