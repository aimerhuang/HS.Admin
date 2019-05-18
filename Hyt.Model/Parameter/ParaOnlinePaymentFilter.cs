using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 财务管理网上支付查询参数
    /// </summary>
    /// <remarks>2013-07-16 朱家宏 创建</remarks>
    public struct ParaOnlinePaymentFilter
    {

        private int _pageSize;

        /// <summary>
        /// 当前页号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_pageSize == 0)
                {
                    _pageSize = 10;
                }
                return _pageSize;
            }
            set { _pageSize = value; }
        }

        /// <summary>
        /// 订单编号
        /// </summary>
        public int? OrderSysNo { get; set; }

        /// <summary>
        /// 支付方式编号
        /// </summary>
        public IList<int> PaymentTypeSysNos { get; set; }

        /// <summary>
        /// 交易时间(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 交易时间(止)
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
