
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// CPS商品
	/// </summary>
    /// <remarks>
    /// 2013-10-14 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class UnCpsProduct
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 联盟广告系统编号
		/// </summary>
		[Description("联盟广告系统编号")]
		public int AdvertisementSysNo { get; set; }
 		/// <summary>
		/// 商品系统编号
		/// </summary>
		[Description("商品系统编号")]
		public int ProductSysNo { get; set; }
 		/// <summary>
		/// 状态:启用(1),禁用(0)
		/// </summary>
		[Description("状态:启用(1),禁用(0)")]
		public int Status { get; set; }
 	}
}

	