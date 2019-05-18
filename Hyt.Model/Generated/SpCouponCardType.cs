
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 优惠券卡类型
	/// </summary>
    /// <remarks>
    /// 2014-01-06 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class SpCouponCardType
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 类型名称
		/// </summary>
		[Description("类型名称")]
		public string TypeName { get; set; }
 		/// <summary>
		/// 类型描述
		/// </summary>
		[Description("类型描述")]
		public string TypeDescription { get; set; }
 		/// <summary>
		/// 有效时间起
		/// </summary>
		[Description("有效时间起")]
		public DateTime StartTime { get; set; }
 		/// <summary>
		/// 有效时间止
		/// </summary>
		[Description("有效时间止")]
		public DateTime EndTime { get; set; }
 		/// <summary>
		/// 状态:启用(1),禁用(0)
		/// </summary>
		[Description("状态:启用(1),禁用(0)")]
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

	