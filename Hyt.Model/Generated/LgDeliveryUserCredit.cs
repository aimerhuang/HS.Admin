
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
	public partial class LgDeliveryUserCredit
	{
	  	/// <summary>
		/// 配送员编号
		/// </summary>
		[Description("配送员编号")]
		public int DeliveryUserSysNo { get; set; }
 		/// <summary>
		/// 仓库编号
		/// </summary>
		[Description("仓库编号")]
		public int WarehouseSysNo { get; set; }
 		/// <summary>
		/// 配送信用额度
		/// </summary>
		[Description("配送信用额度")]
		public decimal DeliveryCredit { get; set; }
 		/// <summary>
		/// 配送可用额度
		/// </summary>
		[Description("配送可用额度")]
		public decimal RemainingDeliveryCredit { get; set; }
 		/// <summary>
		/// 借货信用额度
		/// </summary>
		[Description("借货信用额度")]
		public decimal BorrowingCredit { get; set; }
 		/// <summary>
		/// 借货可用额度
		/// </summary>
		[Description("借货可用额度")]
		public decimal RemainingBorrowingCredit { get; set; }
 		/// <summary>
		/// 是否允许借货：是（1）、否（0）
		/// </summary>
		[Description("是否允许借货：是（1）、否（0）")]
		public int IsAllowBorrow { get; set; }
 		/// <summary>
		/// 是否允许配送：是（1）、否（0）
		/// </summary>
		[Description("是否允许配送：是（1）、否（0）")]
		public int IsAllowDelivery { get; set; }
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

	