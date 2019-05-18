using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 2014-04-08  朱家宏 创建
    /// </summary>
    public class CBRptSales
    {
        private int _totalCount;
        private decimal _totalSummation;

        /// <summary>
        /// 月份
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 办事处
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// 门店订单数
        /// </summary>
        public int CountOfStore { get; set; }

        /// <summary>
        /// 门店订单金额
        /// </summary>
        public decimal SummationOfStore { get; set; }
        /// <summary>
        /// 门店退单数
        /// </summary>
        public int CountOfStoreByReturn { get; set; }
        /// <summary>
        /// 门店退单金额
        /// </summary>
        public decimal SummationOfStoreByReturn { get; set; }

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
        /// 升舱百城达订单数
        /// </summary>
        public int CountOfScBcd { get; set; }

        /// <summary>
        /// 升舱第三方订单数
        /// </summary>
        public int CountOfScDsf { get; set; }

        /// <summary>
        /// 业务员下单数
        /// </summary>
        public int CountOfSalesman { get; set; }

        /// <summary>
        /// 业务员下单金额
        /// </summary>
        public decimal SummationOfSalesman { get; set; }

        /// <summary>
        /// 总单数 门店+百城+第三方
        /// </summary>
        /// <remarks>
        /// 2014-04-10 余勇 修改 总单数= 门店+百城+第三方
        /// </remarks>
        public int TotalCount
        {
            get { return CountOfHytBcd + CountOfHytDsf + CountOfStore - CountOfStoreByReturn; }
            set { _totalCount = value; }
        }

        /// <summary>
        /// 总金额 门店+百城+第三方
        /// </summary>
        /// <remarks>
        /// 2014-04-10 余勇 修改 总金额= 门店+百城+第三方
        /// </remarks>
        public decimal TotalSummation
        {
            get { return SummationOfHytBcd + SummationOfHytDsf + SummationOfStore - SummationOfStoreByReturn; }
            set { _totalSummation = value; }
        }

        /// <summary>
        /// 办事处编号
        /// </summary>
        public int AreaSysNo { get; set; }
    }
}
