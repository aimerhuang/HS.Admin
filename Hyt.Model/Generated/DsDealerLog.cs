
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 分销商升舱错误日志
	/// </summary>
    /// <remarks>
    /// 2014-03-28 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsDealerLog
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 分销商城类型系统编号
		/// </summary>
		[Description("分销商城类型系统编号")]
		public int MallTypeSysNo { get; set; }
        /// <summary>
        /// 分销商城系统编号
        /// </summary>
        public int MallSysNo { get; set; }
 		/// <summary>
		/// 商城订单号
		/// </summary>
		[Description("商城订单号")]
		public string MallOrderId { get; set; }
 		/// <summary>
		/// 事物编号(订单)
		/// </summary>
		[Description("事物编号(订单)")]
		public string OrderTransactionSysNo { get; set; }
 		/// <summary>
		/// 商城订单号
		/// </summary>
		[Description("商城订单号")]
		public int SoOrderSysNo { get; set; }
 		/// <summary>
		/// 日志内容
		/// </summary>
		[Description("日志内容")]
		public string LogContent { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        [Description("错误消息")]
        public string Message { get; set; }
 		/// <summary>
		/// 状态:待解决(10),已解决(20)
		/// </summary>
		[Description("状态:待解决(10),已解决(20)")]
		public int Status { get; set; }
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

	