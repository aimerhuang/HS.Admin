
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
	public partial class CrCustomerLevel
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 等级名称
		/// </summary>
		[Description("等级名称")]
		public string LevelName { get; set; }
 		/// <summary>
		/// 等级描述
		/// </summary>
		[Description("等级描述")]
		public string LevelDescription { get; set; }
 		/// <summary>
		/// 等级经验值下限
		/// </summary>
		[Description("等级经验值下限")]
		public int LowerLimit { get; set; }
 		/// <summary>
		/// 等级经验值上限
		/// </summary>
		[Description("等级经验值上限")]
		public int UpperLimit { get; set; }
 		/// <summary>
		/// 惠源币是否可用于支付货款：支持（1）、不支持（0）
		/// </summary>
		[Description("惠源币是否可用于支付货款：支持（1）、不支持（0）")]
		public int CanPayForProduct { get; set; }
 		/// <summary>
		/// 惠源币支付货款上限
		/// </summary>
		[Description("惠源币支付货款上限")]
		public int ProductPaymentUpperLimit { get; set; }
 		/// <summary>
		/// 惠源币支付货款比例
		/// </summary>
		[Description("惠源币支付货款比例")]
		public decimal ProductPaymentPercentage { get; set; }
 		/// <summary>
		/// 惠源币是否可用于支付服务：支持（1）、不支持（0）
		/// </summary>
		[Description("惠源币是否可用于支付服务：支持（1）、不支持（0）")]
		public int CanPayForService { get; set; }
 		/// <summary>
		/// 惠源币支付服务上限
		/// </summary>
		[Description("惠源币支付服务上限")]
		public int ServicePaymentUpperLimit { get; set; }
 		/// <summary>
		/// 惠源币支付服务比例
		/// </summary>
		[Description("惠源币支付服务比例")]
		public decimal ServicePaymentPercentage { get; set; }
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

	