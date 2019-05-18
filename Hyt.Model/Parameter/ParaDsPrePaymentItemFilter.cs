using System;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 交易记录查询参数
    /// </summary>
    /// <remarks>2013-09-06 黄志勇 创建</remarks>
    public struct ParaDsPrePaymentItemFilter
    {
        private int _pageSize;
        private DateTime? _endDate;

        /// <summary>
        /// 当前页号
        /// </summary>
        public int PageIndex { get; set; }

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
        /// 分销商系统编号
        /// </summary>
        public int DealerSysNo { get; set; }
       
        /// <summary>
        /// 创建时间(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 创建时间(止)
        /// </summary>
        public DateTime? EndDate
        {
            get
            {
                //查询日期上限+1
                return _endDate == null ? (DateTime?)null : _endDate.Value.AddDays(1);
            }
            set { _endDate = value; }
        }
    }
}
