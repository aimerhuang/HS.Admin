
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-12-11 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class rp_仓库内勤
	{
	  		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public int 仓库编号 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 仓库 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public int 内勤编号 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 内勤 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public int 处理单量_百城达 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public int 处理单量_第三方 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public DateTime 统计日期 { get; set; }
 	}
}

	