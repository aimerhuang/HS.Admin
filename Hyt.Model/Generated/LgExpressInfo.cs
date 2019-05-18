
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 物流配送信息
	/// </summary>
    /// <remarks>
    /// 2014-04-04 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class LgExpressInfo
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 事务编号
		/// </summary>
		[Description("事务编号")]
		public string TransactionSysNo { get; set; }
 		/// <summary>
		/// 快递公司名称
		/// </summary>
		[Description("快递公司名称")]
		public string CompanyName { get; set; }
 		/// <summary>
		/// 快递公司的编码
		/// </summary>
		[Description("快递公司的编码")]
		public string CompanyCode { get; set; }
 		/// <summary>
		/// 物流单号
		/// </summary>
		[Description("物流单号")]
		public string ExpressNo { get; set; }
 		/// <summary>
		/// 收货地址（SoReceiveAddress）
		/// </summary>
		[Description("收货地址（SoReceiveAddress）")]
		public int AddressSysNo { get; set; }
 		/// <summary>
		/// 出发城市
		/// </summary>
		[Description("出发城市")]
		public string FromCity { get; set; }
 		/// <summary>
		/// 目的城市
		/// </summary>
		[Description("目的城市")]
		public string ToCity { get; set; }
 		/// <summary>
		/// 快递状态:0:未签收;1:已签收
		/// </summary>
		[Description("快递状态:0:未签收;1:已签收")]
		public int ExpressStatus { get; set; }
 		/// <summary>
		/// 提交返回状态:200: 提交成功；701: 拒绝订阅的快递公
		/// </summary>
		[Description("提交返回状态:200: 提交成功；701: 拒绝订阅的快递公")]
		public int PostResultStatus { get; set; }
 		/// <summary>
		/// 提交次数
		/// </summary>
		[Description("提交次数")]
		public int PostNumber { get; set; }
 		/// <summary>
		/// 提交时间
		/// </summary>
		[Description("提交时间")]
		public DateTime PostTime { get; set; }
 		/// <summary>
		/// 回调状态:监控状态:polling:监控中，shutdown:结束，a
		/// </summary>
		[Description("回调状态:监控状态:polling:监控中，shutdown:结束，a")]
		public string CallBackStatus { get; set; }
 		/// <summary>
		/// 回调消息:监控状态相关消息，如:3天查询无记录，60天
		/// </summary>
		[Description("回调消息:监控状态相关消息，如:3天查询无记录，60天")]
		public string CallBackMessage { get; set; }

        /// <summary>
        /// 签名字符
        /// </summary>
        [Description("签名字符")]
        public string Salt { get; set; }
	}
}

	