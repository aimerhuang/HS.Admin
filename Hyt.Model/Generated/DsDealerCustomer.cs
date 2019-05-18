
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 分销商客户关系
	/// </summary>
    /// <remarks>
    /// 2013-09-10 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsDealerCustomer
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 分销商系统编号
		/// </summary>
		[Description("分销商系统编号")]
		public int DealerSysNo { get; set; }
 		/// <summary>
		/// 客户系统编号
		/// </summary>
		[Description("客户系统编号")]
		public int UserSysNo { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreatedDate { get; set; }
 	}
}

	