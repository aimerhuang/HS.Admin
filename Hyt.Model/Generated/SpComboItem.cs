
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
	public partial class SpComboItem
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 组合套餐系统编号
		/// </summary>
		[Description("组合套餐系统编号")]
		public int ComboSysNo { get; set; }
 		/// <summary>
		/// 商品系统编号
		/// </summary>
		[Description("商品系统编号")]
		public int ProductSysNo { get; set; }
 		/// <summary>
		/// 商品名称
		/// </summary>
		[Description("商品名称")]
		public string ProductName { get; set; }
 		/// <summary>
		/// 优惠金额
		/// </summary>
		[Description("优惠金额")]
		public decimal DiscountAmount { get; set; }
 		/// <summary>
		/// 是否是主商品
		/// </summary>
		[Description("是否是主商品")]
		public int IsMaster { get; set; }
 	}
}

	