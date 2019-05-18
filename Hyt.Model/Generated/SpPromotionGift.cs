
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
	//[Serializable]
	public partial class SpPromotionGift
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
		/// 加购价
		/// </summary>
		[Description("加购价")]
		public decimal PurchasePrice { get; set; }
        /// <summary>
        /// 所需最小金额
        /// </summary>
        [Description("所需最小金额")]
        public decimal RequirementMinAmount { get; set; }
 		/// <summary>
        /// 所需最大金额
		/// </summary>
        [Description("所需最大金额")]
		public decimal RequirementMaxAmount { get; set; }
 		/// <summary>
		/// 最大销售数量
		/// </summary>
		[Description("最大销售数量")]
		public int MaxSaleQuantity { get; set; }
 		/// <summary>
		/// 已销售数量
		/// </summary>
		[Description("已销售数量")]
		public int UsedSaleQuantity { get; set; }
 	}
}

	