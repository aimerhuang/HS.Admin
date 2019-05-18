
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
	public partial class WhProductLendPrice
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 借货单明细系统编号
		/// </summary>
		[Description("借货单明细系统编号")]
		public int ProductLendItemSysNo { get; set; }
 		/// <summary>
		/// 商品价格
		/// </summary>
		[Description("商品价格")]
		public decimal Price { get; set; }
 		/// <summary>
		/// 价格来源：基础价格（0）、会员等级价格（10）、配送
		/// </summary>
		[Description("价格来源：基础价格（0）、会员等级价格（10）、配送")]
		public int PriceSource { get; set; }
 		/// <summary>
		/// 来源编号
		/// </summary>
		[Description("来源编号")]
		public int SourceSysNo { get; set; }
 	}
}

	