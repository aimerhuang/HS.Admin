
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 新闻商品关联表
	/// </summary>
    /// <remarks>
    /// 2014-01-14 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class FeNewsProductAssociation
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 新闻系统编号
		/// </summary>
		[Description("新闻系统编号")]
		public int NewsSysNo { get; set; }
 		/// <summary>
		/// 产品系统编号
		/// </summary>
		[Description("产品系统编号")]
		public int ProductSysNo { get; set; }
 		/// <summary>
		/// 显示序号
		/// </summary>
		[Description("显示序号")]
		public int DisplayOrder { get; set; }
 	}
}

	