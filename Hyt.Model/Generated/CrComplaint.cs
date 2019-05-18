
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
	public partial class CrComplaint
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 订单编号
		/// </summary>
		[Description("订单编号")]
		public int OrderSysNo { get; set; }
 		/// <summary>
		/// 投诉类型：商品（10）、订单（20）、物流（30）、售后
		/// </summary>
		[Description("投诉类型：商品（10）、订单（20）、物流（30）、售后")]
		public int ComplainType { get; set; }
 		/// <summary>
		/// 投诉内容
		/// </summary>
		[Description("投诉内容")]
		public string ComplainContent { get; set; }
 		/// <summary>
		/// 状态：待处理（10）、处理中（20）、已处理（30）、作
		/// </summary>
		[Description("状态：待处理（10）、处理中（20）、已处理（30）、作")]
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

	