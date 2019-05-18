
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-08-29 杨浩 T4生成
    /// </remarks>
	//[Serializable]
	public partial class CrShoppingCartItem
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 是否选中:是(1),否(0)
		/// </summary>
		[Description("是否选中:是(1),否(0)")]
		public int IsChecked { get; set; }
 		/// <summary>
		/// 商品编号
		/// </summary>
		[Description("商品编号")]
		public int ProductSysNo { get; set; }
 		/// <summary>
		/// 商品名称
		/// </summary>
		[Description("商品名称")]
		public string ProductName { get; set; }
 		/// <summary>
		/// 订购数量
		/// </summary>
		[Description("订购数量")]
		public int Quantity { get; set; }
        /// <summary>
        /// 商品原单价
        /// </summary>
        [Description("商品原单价")]
        public decimal OriginPrice { get; set; }
 		/// <summary>
		/// 是否锁定
		/// </summary>
		[Description("是否锁定")]
		public int IsLock { get; set; }
 		/// <summary>
		/// 是否过期重置
		/// </summary>
		[Description("是否过期重置")]
		public int IsExpireReset { get; set; }
 		/// <summary>
		/// 可使用促销
		/// </summary>
		[Description("可使用促销")]
		public string Promotions { get; set; }
 		/// <summary>
		/// 已使用促销
		/// </summary>
		[Description("已使用促销")]
		public string UsedPromotions { get; set; }
 		/// <summary>
		/// 组代码
		/// </summary>
		[Description("组代码")]
		public string GroupCode { get; set; }
 		/// <summary>
		/// 主商品系统编号
		/// </summary>
		[Description("主商品系统编号")]
		public int MasterProductSysNo { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreateDate { get; set; }
 		/// <summary>
		/// 来源：PC网站（10）、信营全球购B2B2C3G网站（15）、门
		/// </summary>
		[Description("来源：PC网站（10）、信营全球购B2B2C3G网站（15）、门")]
		public int Source { get; set; }
 		/// <summary>
		/// 商品销售类型：普通(10),团购(20),秒杀(30),抢购(40),
		/// </summary>
		[Description("商品销售类型：普通(10),团购(20),秒杀(30),抢购(40),")]
		public int ProductSalesType { get; set; }
 		/// <summary>
		/// 商品销售类型编号
		/// </summary>
		[Description("商品销售类型编号")]
		public int ProductSalesTypeSysNo { get; set; }
 	}
}

	