
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 优惠券卡号
	/// </summary>
    /// <remarks>
    /// 2014-01-06 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class SpCouponCard
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 优惠券卡类型系统编号
		/// </summary>
		[Description("优惠券卡类型系统编号")]
		public int CardTypeSysNo { get; set; }
 		/// <summary>
		/// 优惠卡号码
		/// </summary>
		[Description("优惠卡号码")]
		public string CouponCardNo { get; set; }
 		/// <summary>
		/// 激活时间
		/// </summary>
		[Description("激活时间")]
		public DateTime ActivationTime { get; set; }
 		/// <summary>
		/// 终止时间
		/// </summary>
		[Description("终止时间")]
		public DateTime TerminationTime { get; set; }
        /// <summary>
        /// 状态:启用(1),禁用(0)
        /// </summary>
        [Description("状态:启用(1),禁用(0)")]
        public int Status { get; set; }
 	}
}

	