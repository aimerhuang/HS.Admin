
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-09-17 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class ReportDsorderDetail
	{
	  		/// <summary>
		/// 商城类型
		/// </summary>
		[Description("商城类型")]
		public int 商城类型 { get; set; }
 		/// <summary>
		/// 商城名称
		/// </summary>
		[Description("商城名称")]
		public string 商城名称 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public int 订单编号 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public DateTime 升舱付款时间 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public DateTime 商城订单时间 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 所属分支机构 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 客户所在城市 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 升舱来源店面 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 物流类型 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 产品名称 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public decimal 付款金额 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 发货时间 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 未发货原因 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 备注 { get; set; }
 	}
}

	