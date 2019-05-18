using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 销量统计参数
    /// </summary>
    /// <remarks>2014-04-09 朱家宏 创建</remarks>
    public class ParaRptSalesFilter
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
        /// 开始日期(起)
        /// </summary>
        public string BeginDate { get; set; }

        /// <summary>
        /// 结束日期(止)
        /// </summary>
        public string EndDate { get; set; }

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
