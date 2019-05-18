
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
	public partial class PdProductStatistics
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
		/// 销量
		/// </summary>
		[Description("销量")]
		public int Sales { get; set; }
 		/// <summary>
		/// 喜欢
		/// </summary>
		[Description("喜欢")]
		public int Liking { get; set; }
 		/// <summary>
		/// 收藏
		/// </summary>
		[Description("收藏")]
		public int Favorites { get; set; }
 		/// <summary>
		/// 评论数
		/// </summary>
		[Description("评论数")]
		public int Comments { get; set; }
 		/// <summary>
		/// 晒单数
		/// </summary>
		[Description("晒单数")]
		public int Shares { get; set; }
 		/// <summary>
		/// 咨询数
		/// </summary>
		[Description("咨询数")]
		public int Question { get; set; }
 		/// <summary>
		/// 总评分
		/// </summary>
		[Description("总评分")]
		public int TotalScore { get; set; }
 		/// <summary>
		/// 平均评分
		/// </summary>
		[Description("平均评分")]
		public decimal AverageScore { get; set; }
 	}
}

	