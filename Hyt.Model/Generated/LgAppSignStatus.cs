
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// App签收状态表
	/// </summary>
    /// <remarks>
    /// 2014-01-14 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class LgAppSignStatus
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 配送单系统编号
		/// </summary>
		[Description("配送单系统编号")]
		public int DeliverySysNo { get; set; }
 		/// <summary>
		/// 单据类型：出库单（10）、取货单（20）
		/// </summary>
		[Description("单据类型：出库单（10）、取货单（20）")]
		public int NoteType { get; set; }
 		/// <summary>
		/// 单据编号
		/// </summary>
		[Description("单据编号")]
		public int NoteSysNo { get; set; }
 		/// <summary>
		/// 状态:拒收(20),未送达/未取件(30),部分签收(40),已签
		/// </summary>
		[Description("状态:拒收(20),未送达/未取件(30),部分签收(40),已签")]
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
 	}
}

	