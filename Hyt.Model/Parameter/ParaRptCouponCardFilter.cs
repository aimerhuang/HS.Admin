using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 报表-优惠卡统计参数
    /// </summary>
    /// <remarks>2014-02-26 朱家宏 创建</remarks>
    public struct ParaRptCouponCardFilter
    {
        private DateTime? _beginDate;
        private DateTime? _endDate;
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
        /// 开始日期(起)
        /// </summary>
        public DateTime? BeginDate
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Month))
                {
                    _beginDate = DateTime.Parse(Month.Trim());
                }
                return _beginDate;
            }
        }

        /// <summary>
        /// 结束日期(止)
        /// </summary>
        public DateTime? EndDate
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Month))
                {
                    _endDate = DateTime.Parse(Month.Trim()).AddMonths(1);
                }
                return _endDate;
            }
        }

        /// <summary>
        /// 优惠卡类型
        /// </summary>
        public int? CouponCardTypeSysNo { get; set; }

        /// <summary>
        /// 地区编号
        /// </summary>
        public string Warehouses { get; set; }

        /// <summary>
        /// 报表月份
        /// </summary>
        public string Month { get; set; }
    }
}
