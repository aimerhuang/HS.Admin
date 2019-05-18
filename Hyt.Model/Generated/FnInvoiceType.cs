
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-11-13 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class FnInvoiceType
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 发票名称
		/// </summary>
		[Description("发票名称")]
		public string Name { get; set; }
 		/// <summary>
		/// 税收比例
		/// </summary>
		[Description("税收比例")]
		public decimal Percentage { get; set; }
 		/// <summary>
		/// 退票超时时间(天)
		/// </summary>
		[Description("退票超时时间(天)")]
		public int OverTime { get; set; }
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

	