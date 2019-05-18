
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
	public partial class PdPrice
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
		public int ProductSysNo { get; set; }
 		/// <summary>
		/// 商品价格
		/// </summary>
		[Description("商品价格")]
		public decimal Price { get; set; }
 		/// <summary>
		/// 价格来源：基础价格（0）、会员等级价格（10）、经销
		/// </summary>
		[Description("价格来源：基础价格（0）、会员等级价格（10）、经销")]
		public int PriceSource { get; set; }
 		/// <summary>
		/// 来源编号
		/// </summary>
		[Description("来源编号")]
		public int SourceSysNo { get; set; }
 		/// <summary>
		/// 状态：有效（1）、无效（0）
		/// </summary>
		[Description("状态：有效（1）、无效（0）")]
		public int Status { get; set; }
 	}
}

	