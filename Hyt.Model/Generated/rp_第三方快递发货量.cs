
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-12-30 杨浩 T4生成
    /// </remarks>
	[Serializable]
    public partial class rp_第三方快递发货量
	{
	  		/// <summary>
		/// 
		/// </summary>
		[Description("")]
        public int StockSysNo { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
        public string StockName { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
        public string CompanyName { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
        public string ExpressNo { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
        public string Remarks { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
        public DateTime CreateDate { get; set; }
 	}
}

	