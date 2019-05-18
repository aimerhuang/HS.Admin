
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
    public partial class CBOutputRptDealerSales
	{

        /// <summary>
        /// 分销商名称
        /// </summary>
        public string DealerName { get; set; }
        
		/// <summary>
		/// 会员数
		/// </summary>
        public int CustomerNums { get; set; }

        /// <summary>
        /// 营业额
        /// </summary>
        public decimal SumOrderAmount { get; set; }

	}
}

	