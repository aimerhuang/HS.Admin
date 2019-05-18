
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
	public partial class CrBulkPurchase
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 联系人
		/// </summary>
		[Description("联系人")]
		public string ContactName { get; set; }
 		/// <summary>
		/// 联系方式
		/// </summary>
		[Description("联系方式")]
		public string ContactWay { get; set; }
 		/// <summary>
		/// 公司名称
		/// </summary>
		[Description("公司名称")]
		public string CompanyName { get; set; }
 		/// <summary>
		/// 提交时间
		/// </summary>
		[Description("提交时间")]
		public DateTime CommitDate { get; set; }
 		/// <summary>
		/// 处理人
		/// </summary>
		[Description("处理人")]
		public int HandlerSysNo { get; set; }
 		/// <summary>
		/// 处理时间
		/// </summary>
		[Description("处理时间")]
		public DateTime HandleDate { get; set; }
 		/// <summary>
		/// 购买需求
		/// </summary>
		[Description("购买需求")]
		public string Demand { get; set; }
 		/// <summary>
		/// 备注
		/// </summary>
		[Description("备注")]
		public string Remarks { get; set; }
 		/// <summary>
		/// 状态：待处理（10）、已处理（20）、作废（－10）
		/// </summary>
		[Description("状态：待处理（10）、已处理（20）、作废（－10）")]
		public int Status { get; set; }
 	}
}

	