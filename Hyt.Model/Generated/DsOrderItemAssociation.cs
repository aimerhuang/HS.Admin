
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-09-10 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsOrderItemAssociation
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 分销商升舱订单明细系统编号
		/// </summary>
		[Description("分销商升舱订单明细系统编号")]
		public int DsOrderItemSysNo { get; set; }
 		/// <summary>
		/// 事物编号(订单)
		/// </summary>
		[Description("事物编号(订单)")]
		public string OrderTransactionSysNo { get; set; }
 		/// <summary>
		/// 商城订单明细编号
		/// </summary>
		[Description("商城订单明细编号")]
		public int SoOrderItemSysNo { get; set; }
 	}
}

	