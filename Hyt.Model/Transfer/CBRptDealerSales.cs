
using System;
namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 会员涨势统计
	/// </summary>
    /// <remarks>
    /// 2016-02-4 王耀发
    /// </remarks>
	[Serializable]
    public partial class CBRptDealerSales
	{
        /// <summary>
        /// 分销商系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 分销商名称
        /// </summary>
        public string DealerName { get; set; }
        
		/// <summary>
		/// 会员数
		/// </summary>
        public int CustomerNums { get; set; }
        
        /// <summary>
		/// 会员数
		/// </summary>
        public int ACustomerNums { get; set; }

        /// <summary>
        /// 营业额
        /// </summary>
        public decimal SumOrderAmount { get; set; }

        /// <summary>
        /// 会员总营业额
        /// </summary>
        public decimal AllSumOrderAmount { get; set; }

	}
}

	