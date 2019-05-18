
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 分销商城类型
	/// </summary>
    /// <remarks>
    /// 2013-09-10 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsMallType
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 商城代号
		/// </summary>
		[Description("商城代号")]
		public string MallCode { get; set; }
 		/// <summary>
		/// 商城名称
		/// </summary>
		[Description("商城名称")]
		public string MallName { get; set; }
 		/// <summary>
		/// 是否使用预存款:是(1);否(0)
		/// </summary>
		[Description("是否使用预存款:是(1);否(0)")]
		public int IsPreDeposit { get; set; }
 		/// <summary>
		/// 状态:启用(1),禁用(0)
		/// </summary>
		[Description("状态:启用(1),禁用(0)")]
		public int Status { get; set; }
 	}
}

	