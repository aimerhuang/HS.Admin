
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 搭配商品
	/// </summary>
    /// <remarks>
    /// 2013-08-27 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class PdProductCollocation
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 商品系统编号
		/// </summary>
		[Description("商品系统编号")]
		public int ProductSysNo { get; set; }
 		/// <summary>
		/// 关联关系码:使用主商品作为关联关系码，查询是将主商
		/// </summary>
		[Description("关联关系码:使用主商品作为关联关系码，查询是将主商")]
		public int Code { get; set; }
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

	