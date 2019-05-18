
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
	public partial class WhPickUpCode
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 出库单系统编号
		/// </summary>
		[Description("出库单系统编号")]
		public int StockOutSysNo { get; set; }
 		/// <summary>
		/// 事物编号
		/// </summary>
		[Description("事物编号")]
		public string TransactionSysNo { get; set; }
 		/// <summary>
		/// 手机号码
		/// </summary>
		[Description("手机号码")]
		public string MobilePhoneNumber { get; set; }
 		/// <summary>
		/// 验证码
		/// </summary>
		[Description("验证码")]
		public string Code { get; set; }
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

	