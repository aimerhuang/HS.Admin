
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 分销商城快递代码
	/// </summary>
    /// <remarks>
    /// 2014-03-25 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsMallExpressCode
	{
	  	/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 分销商城类型系统编号
		/// </summary>
		[Description("分销商城类型系统编号")]
		public int MallTypeSysNo { get; set; }
 		/// <summary>
		/// 配送方式系统编号
		/// </summary>
		[Description("配送方式系统编号")]
		public int DeliveryType { get; set; }
 		/// <summary>
		/// 第三方快递代码
		/// </summary>
		[Description("第三方快递代码")]
		public string ExpressCode { get; set; }
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
 	}
}

	