
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 分销商EAS关联
	/// </summary>
    /// <remarks>
    /// 2013-10-10 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsEasAssociation
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
		/// 卖家昵称
		/// </summary>
		[Description("卖家昵称")]
		public string SellerNick { get; set; }
 		/// <summary>
		/// EAS编号
		/// </summary>
		[Description("EAS编号")]
		public string Code { get; set; }
 		/// <summary>
		/// 状态:启用(1),禁用(0)
		/// </summary>
		[Description("状态:启用(1),禁用(0)")]
		public int Status { get; set; }
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
 		/// <summary>
		/// 最后更新人
		/// </summary>
		[Description("最后更新人")]
		public int LastUpdateBy { get; set; }
 		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Description("最后更新时间")]
		public DateTime LastUpdateDate { get; set; }
 	}
}

	