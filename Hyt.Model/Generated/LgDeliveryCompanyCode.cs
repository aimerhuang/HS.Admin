
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 配送物流公司编码
	/// </summary>
    /// <remarks>
    /// 2014-04-04 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class LgDeliveryCompanyCode
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 快递公司名称
		/// </summary>
		[Description("快递公司名称")]
		public string CompanyName { get; set; }
 		/// <summary>
		/// 快递公司的编码
		/// </summary>
		[Description("快递公司的编码")]
		public string CompanyCode { get; set; }
 		/// <summary>
		/// 配送方式系统编号
		/// </summary>
		[Description("配送方式系统编号")]
		public int DeliveryTypeSysNo { get; set; }
 	}
}

	