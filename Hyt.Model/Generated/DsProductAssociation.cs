
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 商品关联关系对应表
	/// </summary>
    /// <remarks>
    /// 2013-09-10 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsProductAssociation
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 分销商商城系统编号
		/// </summary>
		[Description("分销商商城系统编号")]
		public int DealerMallSysNo { get; set; }
 		/// <summary>
		/// 商城商品编码
		/// </summary>
		[Description("商城商品编码")]
		public string MallProductId { get; set; }
 		/// <summary>
		/// 商城商品属性
		/// </summary>
		[Description("商城商品属性")]
		public string MallProductAttr { get; set; }
 		/// <summary>
		/// 商城商品编码
		/// </summary>
		[Description("商城商品编码")]
		public int HytProductSysNo { get; set; }
        /// <summary>
        /// 状态：上架（1）、下架（0）
        /// </summary>
        [Description("状态：上架（1）、下架（0）")]
        public int Status { get; set; }
 	}
}

	