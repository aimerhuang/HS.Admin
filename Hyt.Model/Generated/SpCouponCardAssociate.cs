
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 优惠卡关联
	/// </summary>
    /// <remarks>
    /// 2014-01-06 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class SpCouponCardAssociate
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
		/// 优惠券系统编号
		/// </summary>
		[Description("优惠券系统编号")]
		public int CouponSysNo { get; set; }
 		/// <summary>
		/// 绑定次数
		/// </summary>
		[Description("绑定次数")]
		public int BindNumber { get; set; }
 	}
}

	