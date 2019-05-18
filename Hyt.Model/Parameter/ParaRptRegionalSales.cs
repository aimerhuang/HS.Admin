using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 区域销量统计
    /// </summary>
    /// <remarks>2014-08-11 余勇 创建</remarks>
    public class ParaRptRegionalSales
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
        /// 报表月份
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 地区编号
        /// </summary>
        public string Area { get; set; }
    }
}
