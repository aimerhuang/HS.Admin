
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 分销商产品特殊价格表
	/// </summary>
    /// <remarks>
    /// 2013-09-10 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsSpecialPrice
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
		/// 商品编号
		/// </summary>
		[Description("商品编号")]
		public int ProductSysNo { get; set; }
 		/// <summary>
		/// 商品价格
		/// </summary>
		[Description("商品价格")]
		public decimal Price { get; set; }
 		/// <summary>
		/// 状态:启用(1),禁用(0)
		/// </summary>
		[Description("状态:启用(1),禁用(0)")]
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
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Description("线下门店价格")]
        public decimal ShopPrice { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Description("线下批发价格")]
        public decimal WholesalePrice { get; set; }
 	}
}

	