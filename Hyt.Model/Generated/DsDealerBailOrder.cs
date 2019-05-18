
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 分销商保证金订单
	/// </summary>
    /// <remarks>
    /// 2016-05-14 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsDealerBailOrder
	{
	  	/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 联系人
		/// </summary>
		[Description("联系人")]
		public string ContactName { get; set; }
 		/// <summary>
		/// 联系方式
		/// </summary>
		[Description("联系方式")]
		public string ContactWay { get; set; }
 		/// <summary>
		/// 公司名称
		/// </summary>
		[Description("公司名称")]
		public decimal Money { get; set; }
 		/// <summary>
		/// 购买需求
		/// </summary>
		[Description("购买需求")]
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string Extend { get; set; }
 		/// <summary>
		/// 处理时间
		/// </summary>
		[Description("处理时间")]
		public DateTime HandleDate { get; set; }
 		/// <summary>
		/// 备注
		/// </summary>
		[Description("备注")]
		public string Remarks { get; set; }
 		/// <summary>
		/// 处理人
		/// </summary>
		[Description("处理人")]
		public int HandlerSysNo { get; set; }
 		/// <summary>
		/// 提交时间
		/// </summary>
		[Description("提交时间")]
		public DateTime CreateDate { get; set; }
 		/// <summary>
		/// 状态：待审核（10）、待支付（20）、 已完成（30）、
		/// </summary>
		[Description("状态：待审核（10）、待支付（20）、 已完成（30）、")]
		public int Status { get; set; }
 		/// <summary>
		/// 10:待支付 20:已支付 30：支付异常
		/// </summary>
		[Description("10:待支付 20:已支付 30：支付异常")]
		public int PayStatus { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayTypeSysNo { get; set; }
 	}
}

	