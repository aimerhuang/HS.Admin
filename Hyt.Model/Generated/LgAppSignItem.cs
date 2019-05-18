
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// App签收明细表
	/// </summary>
    /// <remarks>
    /// 2014-01-14 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class LgAppSignItem
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// App签收状态系统编号
		/// </summary>
		[Description("App签收状态系统编号")]
		public int AppSignStatusSysNo { get; set; }
 		/// <summary>
		/// 单据明细编号,(出库单/取件单明细系统编号)
		/// </summary>
		[Description("单据明细编号,(出库单/取件单明细系统编号)")]
		public int NoteItemSysNo { get; set; }
 		/// <summary>
		/// 签收数量
		/// </summary>
		[Description("签收数量")]
		public int SignQuantity { get; set; }
 	}
}

	