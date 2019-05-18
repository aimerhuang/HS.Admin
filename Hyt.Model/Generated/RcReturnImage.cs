
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-09-13 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class RcReturnImage
	{
	  		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 退换货系统编号
		/// </summary>
		[Description("退换货系统编号")]
		public int ReturnSysNo { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string ImageUrl { get; set; }
 	}
}

	