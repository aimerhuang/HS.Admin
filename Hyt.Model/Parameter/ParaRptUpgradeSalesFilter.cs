using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 升舱销量参数
    /// </summary>
    /// <remarks>2014-04-16 朱家宏 创建</remarks>
    public class ParaRptUpgradeSalesFilter
    {
        private int _pageSize;
        private DateTime? _statsDate;

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
        /// 报表月份
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 开始日期(起)
        /// </summary>
        public DateTime? StatsDate
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Month))
                {
                    _statsDate = DateTime.Parse(Month.Trim());
                }
                return _statsDate;
            }
        }

    }

}
