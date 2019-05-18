
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 优惠券领取日志
	/// </summary>
    /// <remarks>
    /// 2013-12-27 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class SpCouponReceiveLog
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 优惠券系统编号
		/// </summary>
		[Description("优惠券系统编号")]
		public int CouponSysNo { get; set; }
        /// <summary>
        /// 原优惠券系统编号
        /// </summary>
        [Description("原优惠券系统编号")]
        public int OriginalCouponSysNo { get; set; }
 		/// <summary>
		/// 主题代码
		/// </summary>
		[Description("主题代码")]
		public string SubjectCode { get; set; }
 		/// <summary>
		/// 优惠卡号码
		/// </summary>
		[Description("优惠卡号码")]
		public string CouponCardNo { get; set; }
 		/// <summary>
		/// 发放人系统编号
		/// </summary>
		[Description("发放人系统编号")]
		public int OriginatorSysNo { get; set; }
 		/// <summary>
		/// 转赠人客户编号
		/// </summary>
		[Description("转赠人客户编号")]
		public int DonatorSysNo { get; set; }
 		/// <summary>
		/// 领取人客户编号
		/// </summary>
		[Description("领取人客户编号")]
		public int RecipientSysNo { get; set; }
 		/// <summary>
		/// 领取时间
		/// </summary>
		[Description("领取时间")]
		public DateTime ReceiveTime { get; set; }
 	}
}

	