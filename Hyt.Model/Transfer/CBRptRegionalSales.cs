
using System;
namespace Hyt.Model.Transfer
{
    /// <summary>
	/// 区域销售统计
	/// </summary>
    /// <remarks>
    /// 2014-08-11 余勇
    /// </remarks>
	[Serializable]
    public partial class CBRptRegionalSales
	{
	  
		/// <summary>
		/// 序号
		/// </summary>
        public int RowNumber { get; set; }

        // <summary>
        /// 办事处
        /// </summary>
        public string AreaName { get; set; }
 
		/// <summary>
		/// 省
		/// </summary>
        public string Province { get; set; }
 
		/// <summary>
		/// 市
		/// </summary>
		public string City { get; set; }
 
		/// <summary>
		/// 区
		/// </summary>
        public string Area { get; set; }
 
        /// <summary>
        /// 商城百城达订单数
        /// </summary>
        public int CountOfHytBcd { get; set; }

        /// <summary>
        /// 商城百城达订单金额
        /// </summary>
        public decimal SummationOfHytBcd { get; set; }

        /// <summary>
        /// 商城第三方订单数
        /// </summary>
        public int CountOfHytDsf { get; set; }

        /// <summary>
        /// 商城第三方订单金额
        /// </summary>
        public decimal SummationOfHytDsf { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        public int RowCount { get; set; }
	}
}

	