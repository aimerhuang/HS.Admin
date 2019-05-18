
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
	public partial class PdProductImage
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
		/// 图片地址
		/// </summary>
		[Description("图片地址")]
		public string ImageUrl { get; set; }
 		/// <summary>
		/// 状态:显示(1),隐藏(0)
		/// </summary>
		[Description("状态:显示(1),隐藏(0)")]
		public int Status { get; set; }
 		/// <summary>
		/// 显示顺序
		/// </summary>
		[Description("显示顺序")]
		public int DisplayOrder { get; set; }
 	}
}

	