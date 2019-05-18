
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
	public partial class SoCoupon
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int OrderSysNo { get; set; }
 		/// <summary>
		/// 优惠券系统编号
		/// </summary>
		[Description("优惠券系统编号")]
		public int CouponSysNo { get; set; }
 		/// <summary>
		/// 状态：初始（10）、已扣回（20）
		/// </summary>
		[Description("状态：初始（10）、已扣回（20）")]
		public int Status { get; set; }
 	}
}

	