
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
	public partial class SpPromotionOverlay
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 促销系统编号
		/// </summary>
		[Description("促销系统编号")]
		public int PromotionSysNo { get; set; }
 		/// <summary>
		/// 
		/// </summary>
        [Description("叠加编号")]
		public int OverlayCode { get; set; }
 	}
}

	